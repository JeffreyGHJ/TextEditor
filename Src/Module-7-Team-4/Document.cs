using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module_7_Team_4
{
    [Serializable]
    public class Document
    {
        //private List<Text> textList = new List<Text>();
        private Point textInsertionPoint = new Point(30, 25); // Y coordinate set to 25 because of the space take by the menu strip

        private bool editingText = false;

        private Text selectedText;

        [NonSerialized]BindingSource documentBindingSource = new BindingSource();

        IList<Text> textList;

        public IList<Text> TextList
        {
            get { return this.textList; }
            set { this.textList = value; }
        }

        private Point TextInsertionPoint
        {
            get { return this.textInsertionPoint; }
            set { this.textInsertionPoint = value; }
        }

        public bool EditingText
        { 
            get { return this.editingText; }
            set { this.editingText = value; }
        }

        public Text SelectedText
        {
            get { return this.selectedText; }
            set { this.selectedText = value; }
        }

        public void AddText(Text text)
        {
            //TextList.Add(text);
            TextList.Add(text);
            text.ZOrder = TextList.IndexOf(text);
            text.PropertyChanged += ZOrder_PropertyChanged;
        }

        public void RemoveText(Text text)
        {
            //TextList.Add(text);
            TextList.Remove(text);
            text.ZOrder = TextList.IndexOf(text);
            text.PropertyChanged += ZOrder_PropertyChanged;
        }

        //Clears the text from the list
        public void ClearText()
        {
            TextList.Clear();
        }
        private void ZOrder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ZOrder")
            {
                Text text = (Text)sender;
                Console.WriteLine(text.StoredText + " - ZOrder_PropertyChanged");

                if (text.ZOrder > this.TextList.Count - 1)
                {
                    text.ZOrder = this.TextList.Count - 1;
                }
                else if ( text.ZOrder != TextList.IndexOf(text) )
                {
                    Console.WriteLine(text.StoredText + " - ZOrder Changed to: " + text.ZOrder);

                    this.TextList.Remove(text);
                    this.TextList.Insert(text.ZOrder, text);
                }
            }
        }
      
        public Text getText(int index)
        {
            return TextList.ElementAt(index);
        }

        public Document()
        {
            this.documentBindingSource.DataSource = typeof(Text);       // Set the data type for the BindingSource
            this.TextList = (IList<Text>)this.documentBindingSource.List;  // Get a reference to the list managed by the BindingSource
        }

        public void Paint(Graphics g, Rectangle clientRect)
        {
            TextInsertionPoint = new Point(30, 25);  //InsertionPoint acts like a cursor, when redrawing the list start at the top corner
            
            if(TextList.Count != 0)
            {
                foreach (Text text in TextList)
                {
                    text.ZOrder = TextList.IndexOf(text);

                    if (text.TextChanged)
                    {
                        text.SetNewBounds(g);
                        text.TextChanged = false;
                    }

                    CalculateLocation(text, clientRect);    //CalculateLocation determines text location and updates TextInsertionPoint

                    if (text.StoredText == Environment.NewLine)
                    {
                        continue;   //Do not need to draw a new line; perform next foreach iteration
                    }
                    else
                    {
                        drawText(text, g);
                    }
                }
            }
        }

        private void CalculateLocation(Text text, Rectangle clientRect)
        {
            if (text.StoredText == Environment.NewLine)
            {
                TextInsertionPoint = new Point(30, TextInsertionPoint.Y + (int)text.TextFont.GetHeight());   //move InsertionPoint to new line
            }
            else //if (text.LocationSetByUser == false)    //Text hasn't been moved by the mouse so its position will be set algorithmically
            {
                if (textInsertionPoint.X + text.TextBounds.Width > clientRect.Width)    //Text is too wide for this spot in client area
                {
                    TextInsertionPoint = new Point(30, TextInsertionPoint.Y + (int)text.TextFont.GetHeight());   //move text to new line

                    if (text.LocationSetByUser == false)
                    {
                        text.Location = TextInsertionPoint;
                    }
                }

                if (text.LocationSetByUser == false)
                {
                    text.Location = TextInsertionPoint;
                }
                TextInsertionPoint = new Point(TextInsertionPoint.X + (int)text.TextBounds.Width, TextInsertionPoint.Y);    //After setting postion for this text object, move the InsertionPoint to the right edge of the placed text
            }
        }

        private void drawText(Text text, Graphics g)
        {
            //Set drawing properties
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            g.SmoothingMode = SmoothingMode.AntiAlias;
          
            //Set transform
            Matrix matrix = new Matrix();
            matrix.RotateAt(text.TextRotation, text.CenterPoint);
            g.Transform = matrix;

            if (editingText && text != SelectedText) // if the text object to be drawn is not the one being edited, then make it transparent
            {
                //Draw Transparent
                using (Brush textBrush = new SolidBrush(Color.FromArgb( 64, text.TextColor.R, text.TextColor.G, text.TextColor.B)))
                {
                    GraphicsPath textPath = getStringPath(text, g.DpiY, format);
                    GraphicsPath backgroundPath = new GraphicsPath();
                    backgroundPath.AddRectangle(text.TextBounds);
                    g.FillPath(textBrush, textPath);
                }
            }
            else // Draw opaque
            {
                //Draw
                using (Brush textBrush = new SolidBrush(text.TextColor))
                using (Brush backgroundBrush = new SolidBrush(text.BackgroundColor))
                {
                    GraphicsPath textPath = getStringPath(text, g.DpiY, format);
                    GraphicsPath backgroundPath = new GraphicsPath();
                    backgroundPath.AddRectangle(text.TextBounds);
                    g.FillPath(backgroundBrush, backgroundPath);
                    g.FillPath(textBrush, textPath);
                    //g.DrawPath(Pens.Black, path);   //Outline the text (optional feature)
                    if (editingText)
                    {
                        g.DrawRectangle(Pens.Black, Rectangle.Round(text.TextBounds)); // if Editing, draw box around text for better visibility

                    }
                }
            }
        }

        // Modified function taken from book pg(250 - 251)
        GraphicsPath getStringPath(Text text, float dpi, StringFormat format)
        {
            GraphicsPath path = new GraphicsPath();
            float emSize = dpi * text.TextFont.SizeInPoints / 72;
            path.AddString(text.StoredText, text.TextFont.FontFamily, (int)text.TextFont.Style, emSize, text.TextBounds, format);
            return path;
        }

        public Text GetTextAtPoint(Point mouseDownPos)
        {
            foreach (Text text in TextList)
            {
                if (text.StoredText == Environment.NewLine)
                {
                    continue;
                }
                else if (text.HitTestRegion.IsVisible(mouseDownPos))
                {
                    Console.WriteLine("Selected Text: " + text.StoredText + " - Location: " + text.Location);
                    return text;
                }
            }

            return null;    //If no text was found under the point, return null
        }

        public void MoveText(Text selectedText, Point textStartPos, Point mouseMovePos, Point mouseDownPos)
        {
            Point newLocation = new Point(
                (int)(textStartPos.X + mouseMovePos.X - mouseDownPos.X),
                (int)(textStartPos.Y + mouseMovePos.Y - mouseDownPos.Y));

            selectedText.Location = newLocation;
        }

        public void MoveTextKeys(Text selectedText,int x, int y)
        {
            selectedText.Location = (new Point(selectedText.Location.X + x, selectedText.Location.Y + y));
        }
    }
}
