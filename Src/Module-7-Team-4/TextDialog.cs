using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module_7_Team_4
{
    public partial class TextDialog : Form, ITextDialog
    {
        //use error provider as needed..
        private ErrorProvider errorProvider = new ErrorProvider();

        public event EventHandler Apply;

        private Document doc;

        public Document document { get { return doc; } set { doc = value; } }

        public int docPos { get { return this.BindingManager.Position; } set { this.BindingManager.Position = value; } }

        // IList<Text> ListOfStrings;
        BindingManagerBase BindingManager
        {
            get { return this.BindingContext[this.document.TextList]; }
        }

        Text Current
        {
            get { return (Text)BindingManager.Current; }
        }

      /*  MainForm MainForm
        {
            get { return (MainForm)this.Owner; }
        }*/

        void BindTextData()
        {

            //System.Diagnostics.Debug.WriteLine(this.document.TextList.ElementAt(0));

            //text.StoredText;
            StoredTextLabel.DataBindings.Add("Text", this.document.TextList, "StoredText", true);
            //text.Location;
            locationBoxX.DataBindings.Add("Text", this.document.TextList, "Location", true);
            //text.TextColor;
            TextColorBox.DataBindings.Add("Text", this.document.TextList, "TextColor");
            //text.BackgroundColor;
            BackgroundColorBox.DataBindings.Add("Text", this.document.TextList, "BackgroundColor");
            //text.TextFont;
            TextFontBox.DataBindings.Add("Text", this.document.TextList, "TextFont");
            //text.TextRotation;
            rotationBoxX.DataBindings.Add("Text", this.document.TextList, "TextRotation");
            //text.ZOrder;
            ZOrderTextBox.DataBindings.Add("Text", this.document.TextList, "ZOrder");
        }

        //Default construction, we are required to pass in a Document so we can grab the first element (if opened without a text context.)
        public TextDialog()
        {
            InitializeComponent();
            //Assign the text list to a list we have databinded in the BindingManager
            
            //BindTextData();
            //RefreshItems();
        }
       /* //Use this Constructor specifically for context right click logic.
        public TextDialog(Document stringDoc, Text textFromRegion)
        {
            InitializeComponent();
            //this.Paint += new PaintEventHandler(set_background);
            //ListOfStrings = (IList<Text>)stringDoc.TextList;
            BindTextData();
            //Logic to set that specific position on the data manager here..
            this.BindingManager.Position = stringDoc.TextList.IndexOf(textFromRegion);
            RefreshItems();
        }
        */
        //Handle navigating the binding manager with the data needed.
        void moveFirstButton_Click(object sender, EventArgs e)
        {
            this.BindingManager.Position = 0;
            document.SelectedText = (Text)this.BindingManager.Current;
            ApplyHelper(sender, e); //MainForm.Invalidate();
            RefreshItems();
            ApplyHelper(sender, e);
        }
        void movePreviousButton_Click(object sender, EventArgs e)
        {
            // No need to worry about being < 0
            --this.BindingManager.Position;

            skipEmptyLines(false); // set boolean parameter to false when navigating backwards

            document.SelectedText = (Text)this.BindingManager.Current;

            //MainForm.SelectedText = (Text)this.BindingManager.Current;
            ApplyHelper(sender, e); //MainForm.Invalidate();
            RefreshItems();
            ApplyHelper(sender, e);
        }
        void moveNextButton_Click(object sender, EventArgs e)
        {
            // No need to worry about being > BindingManager.Count
            ++this.BindingManager.Position;

            skipEmptyLines(true); // set boolean parameter to false when navigating forwards

            document.SelectedText = (Text)this.BindingManager.Current;

            //MainForm.SelectedText = (Text)this.BindingManager.Current;
            ApplyHelper(sender, e); //MainForm.Invalidate();
            RefreshItems();
            ApplyHelper(sender, e);
        }
        void moveLastButton_Click(object sender, EventArgs e)
        {
            this.BindingManager.Position = this.BindingManager.Count - 1;
            document.SelectedText = (Text)this.BindingManager.Current;

            //MainForm.SelectedText = (Text)this.BindingManager.Current;
            ApplyHelper(sender, e);//MainForm.Invalidate();
            RefreshItems();
            ApplyHelper(sender, e);
        }

        private void skipEmptyLines(bool forward)
        {
            bool skip = true;
            while (skip)
            {
                Text t = (Text)this.BindingManager.Current;

                if ((t.StoredText == Environment.NewLine || t.StoredText == ""))
                {
                    if ( forward )
                    {
                        if (BindingManager.Position < BindingManager.Count - 1)
                        {
                            ++this.BindingManager.Position;
                        }
                        else // The text object at the end of the list is an empty line and will continue trying to skip itself indefinitely
                        {
                            skip = false;   // set loop condition to false in order to prevent infinite loop
                        }
                    }
                    else // if forward == false, then we are navigating backward
                    {
                        if (BindingManager.Position > 0)
                        {
                            --this.BindingManager.Position;
                        }
                        else // The text object at the end of the list is an empty line and will continue trying to skip itself indefinitely
                        {
                            skip = false;   // set loop condition to false in order to prevent infinite loop
                        }
                    }
                }
                else
                {
                    skip = false; // We have finished navigating to a non-empty text object, stop looping
                }
            }
        }

        void RefreshItems()
        {
            int count = this.BindingManager.Count;
            //Get position
            int position = this.BindingManager.Position + 1;

            //Refresh the text boxes
            StoredTextLabel.Text = Current.StoredText.ToString();
            TextColorBox.Text = Current.TextColor.Name.ToString();
            BackgroundColorBox.Text = Current.BackgroundColor.Name.ToString();
            TextFontBox.Text = Current.TextFont.Name.ToString() + ", " + Current.TextFont.SizeInPoints +"pt";
            rotationBoxX.Text = Current.TextRotation.ToString();
            ZOrderTextBox.Text = Current.ZOrder.ToString();

            this.BeginButton.Enabled = (position > 1);
            this.PreviousButton.Enabled = (position > 1);
            this.ForwardButton.Enabled = (position < count);
            this.EndButton.Enabled = (position < count);
        }

        private void UpdateLocationButton_Click(object sender, EventArgs e)
        {
            //Current.Location = new Point(Int32.Parse(text);
            RefreshItems();
            ApplyHelper(sender, e); //this.Owner.Invalidate();
        }

        private void UpdateColorBttn_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = Color.Azure;
            //if we chose a new color and hit okay..
            if(DialogResult.OK == dlg.ShowDialog())
            {
                Current.TextColor = dlg.Color;
            }
            dlg.Dispose();
            RefreshItems();
            ApplyHelper(sender, e); //this.Owner.Invalidate();
        }

        private void UpdateBGColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = Color.Azure;
            //if we chose a new color and hit okay..
            if (DialogResult.OK == dlg.ShowDialog())
            {
                Current.BackgroundColor = dlg.Color;
            }
            dlg.Dispose();
            RefreshItems();
            ApplyHelper(sender, e); //this.Owner.Invalidate();
        }

        private void UpdateFontButton_Click(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            //if we chose a new font..
            if (DialogResult.OK == dlg.ShowDialog())
            {
                Current.TextFont = dlg.Font;
            }
            dlg.Dispose();
            RefreshItems();
            ApplyHelper(sender, e); //this.Owner.Invalidate();
        }

        private void rotationButton_Click(object sender, EventArgs e)
        {
            RefreshItems();

            ApplyHelper(sender, e);
            //this.Owner.Invalidate();
        }

        private void LocationBox_Validating(object sender, CancelEventArgs e)
        {
            
            errorProvider.SetError(locationBoxX, null);
            if (locationBoxX.Text == "")
            {
                errorProvider.SetError(locationBoxX, "Please enter 2 numbers with a ',' in between..");
                e.Cancel = true;
              
            }
            foreach(char glyph in locationBoxX.Text)
            {
                if(char.IsLetter(glyph) && glyph != ',')
                {
                    errorProvider.SetError(locationBoxX, "Please enter 2 numbers with a ',' in between..");
                    e.Cancel = true;
                    break;
                }

            }

        }

        private void TextRotationBox_Validating(object sender, CancelEventArgs e)
        {
            float helper;
            errorProvider.SetError(rotationBoxX, null);
            bool result = float.TryParse(rotationBoxX.Text, out helper);
            if(!result)
            {  
                errorProvider.SetError(rotationBoxX, "Please enter a proper float value..");
                e.Cancel = true;
            }
        }

        private void ZOrderTextBox_Validating(object sender, CancelEventArgs e)
        {
            int helper;
            errorProvider.SetError(ZOrderTextBox, null);
            bool result = int.TryParse(ZOrderTextBox.Text, out helper);
            if (!result)
            {
             
                errorProvider.SetError(ZOrderTextBox, "Please enter a proper integer value..");
                e.Cancel = true;
            }
        }

        private void TextDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MainForm.Document.EditingText = false;
            this.document.EditingText = false;
            ApplyHelper(sender, e);
            //MainForm.Invalidate();
        }

        private void ApplyHelper(object sender, EventArgs e)
        {

            if(Apply != null)
            {

                Apply(this, EventArgs.Empty);

            }

        }

        private void TextDialog_Shown(object sender, EventArgs e)
        {

            BindTextData();
            RefreshItems();
            document.SelectedText = (Text)this.BindingManager.Current;
            ApplyHelper(sender, e);

        }
    }
}
