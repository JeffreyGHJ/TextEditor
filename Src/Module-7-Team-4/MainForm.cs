using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Module_7_Team_4
{
    public partial class MainForm : Form
    {
        Document document = new Document();

        private bool mousePressed = false;
        private Text selectedText = null;
        private Point mouseDownPos = Point.Empty;
        private Point textStartPos = Point.Empty;
        private String currentPath = String.Empty;
        private ImageAttributes attr;
        private ColorMap[] map;

        Boolean isImage { get; set; }

        public Document Document
        {
            get { return this.document; }
            set { this.document = value; }
        }

        public Text SelectedText
        {
            get { return this.selectedText; }
            set { this.selectedText = value; Document.SelectedText = value; }
        }

        public MainForm()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

             //exception handling for the animated cursor
              try
              {
                  string cursorFileName = "Vulpix";
                  this.Cursor = DynamicCursor.Create(cursorFileName);
              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }
              

        }

    
    
    private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
           // g.PageUnit = GraphicsUnit.Display;
            if (currentPath.Contains(".png"))
            {
                using (Bitmap bmp = new Bitmap(currentPath))
                {
                    
                    if(attr == null)
                    {

                        g.DrawImage(bmp, new PointF(0, 0));

                    }
                    else
                    {
                        Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                        g.DrawImage(bmp, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);

                    }

                }
            }
                
            //Document class handles painting, but requires a Graphics object so we pass one from the MainForm
            Document.Paint(g, this.ClientRectangle);
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            //Calling the method  CapLockKeyStatus to check show the status of the Capslock Key in the status bar

            if (e.Button == MouseButtons.Left)
            {
                mousePressed = true;
                mouseDownPos = e.Location;
                Console.WriteLine("MouseDown on MainForm - MousePosition(Screen): " + MousePosition + " - MouseDownPos: " + mouseDownPos);

                //Retrieve a reference to a text object if you clicked on one
                SelectedText = Document.GetTextAtPoint(mouseDownPos);
                this.ActiveControl = null;

                //Save a reference point of the selected text's original location for calculating movement
                if (SelectedText != null)
                {
                    textStartPos = selectedText.Location;
                    UpdateStatusBar("Selected Text: " + selectedText.StoredText + " - Location: " + selectedText.Location, selectedText.BackgroundColor);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                SelectedText = Document.GetTextAtPoint(e.Location);

                if (SelectedText != null)
                {
                    TextDialog textDialog = new TextDialog(); //changed constructor
                    textDialog.Apply += new EventHandler(Apply);
                    textDialog.document = this.Document;
                    textDialog.docPos = this.Document.TextList.IndexOf(SelectedText);
                    textDialog.Show(this);
                    Document.EditingText = true;
                    //this.Invalidate();
                }

                //DoDragDrop(Clipboard.GetDataObject(), DragDropEffects.Copy | DragDropEffects.Move);

                /*if (drag == DragDropEffects.Move)
                {
                    //if moved, delete text
                    this.Text = "";
                }*/
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePressed == true)
            {
                //If text selected, update its position using Document method
                if (SelectedText != null)
                {
                    Document.MoveText(SelectedText, textStartPos, e.Location, mouseDownPos);
                    SelectedText.LocationSetByUser = true;
                    Invalidate();
                }
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePressed = false;
                textStartPos = Point.Empty;
                Console.WriteLine("MouseUp on MainForm - MousePosition(Screen): " + MousePosition + " - MouseUpPos: " + e.Location);
            }
        }

        public void Form_DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(string)))
            {
                // if acceptable set the DragDropEffect 
                e.Effect = DragDropEffects.Copy;

            }
            else
            {
                //data not acceptible set to none
                e.Effect = DragDropEffects.None;
                //check
            }
        }
        //drop handler
        public void Form_DragDrop(object sender, DragEventArgs e)
        {
            //review where to put the text
            //this.Text = (string)e.Data.GetData(typeof(string));
            string allText = (string)e.Data.GetData(typeof(string));
            string[] words = allText.Split(' ');
           /* string[] listFile = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach(string file in listFile)
            {
                file.Split(' ');
                this.Text += file;

                Graphics g = this.CreateGraphics();

                Text textFile = new Text(file, g);

                this.Document.AddText(textFile);

            }*/
            //int pointStart = 0;
            //int pointV = 0;

            Console.WriteLine(allText);
            System.Diagnostics.Debug.WriteLine(allText);

            foreach (var word in words)
            {
                Graphics g = this.CreateGraphics();

                Text text = new Text(word, g);

                this.Document.AddText(text);

            }

            this.Invalidate(true);

        }
        //create enumeration for key state

        public void Form_DragOver(object sender, DragEventArgs e)
        {
            //(e.Data.GetDataPresent(DataFormats.FileDrop))
            KeyState state = (KeyState)e.KeyState;
            //if (e.Data.GetDataPresent(typeof(String)))
            if (e.Data.GetDataPresent(typeof(string)) && (e.AllowedEffect & DragDropEffects.Copy) != 0)
            {
                if ((state & KeyState.Control) == KeyState.Control)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
            else
            {
                //no data present
                e.Effect = DragDropEffects.None;
            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                // dlg.Filter = "Text Files|*.txt*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    currentPath = dlg.FileName;
                    Console.WriteLine("File Selected: " + currentPath);
                    if (currentPath.Contains(".txt"))
                    {
                        ExtractTextFromCurrentPath();

                    }
                    else
                    {
                        Deserialize(dlg.FileName);
                        foreach (Text itr in document.TextList)
                        {
                            itr.TextChanged = true;
                        }
                        Invalidate();
                    }

                    UpdateStatusBar(currentPath + " has been opened!", Color.LightBlue);
                    //ExtractTextFromCurrentPath();

                }
            }
        }

        // Modified code from: https://stackoverflow.com/questions/4805656/how-do-i-parse-a-text-file-using-c
        private void ExtractTextFromCurrentPath()
        {
            if (currentPath == String.Empty)
            {
                Console.WriteLine("Cannot Extract Text - File Path Not Specified");
                return;
            }

            using (Stream fileStream = File.Open(currentPath, FileMode.Open))
            using (StreamReader reader = new StreamReader(fileStream))
            using (Graphics g = CreateGraphics()) //Need graphics to create a rectangle around text using MeasureString
            {
                string line = reader.ReadLine(); // ReadLine() will return null at end of input stream
                line.Trim();

                while (line != null)
                {
                    string[] words = line.Split(' ');

                    foreach (string word in words)
                    {
                        Text text = new Text(word, g);
                        this.Document.AddText(text);
                        //Console.WriteLine(word);
                    }

                    Text newLine = new Text(Environment.NewLine, g); //Add a NewLine text object to the Document between each call to ReadLine
                    Document.AddText(newLine);

                    line = reader.ReadLine();
                }

                Invalidate(true);   //Repaint after all text objects are added to the Document
            }
        }

        private void SaveAs()
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Derp Files (*.drrp)|*.drrp";
                dlg.DefaultExt = "drrp";
                dlg.AddExtension = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    currentPath = dlg.FileName;
                    Serialize(dlg.FileName);
                    UpdateStatusBar(dlg.FileName + " has been saved!", Color.LightGreen);
                }
            }

            //this.SaveToolStripMenuItem.Enabled = true;
        }

        private void Save()
        {
            if (!string.IsNullOrEmpty(currentPath))
            {
                if (!currentPath.Contains(".drrp"))
                {

                    this.currentPath = currentPath.Substring(0, (currentPath.LastIndexOf('.') + 1));
                    currentPath += "drrp";

                }
                UpdateStatusBar(currentPath + " has been saved!", Color.LightGreen);
                Serialize(currentPath);


            }

        }

        /*private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                //dlg.Filter = "Text Files|*.txt*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Deserialize(dlg.FileName);
                }
            }

            //this.SaveToolStripMenuItem.Enabled = true;
        }*/

        public void Serialize(string fileName)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                IFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, document);
            }
        }

        public void Deserialize(string fileName)
        {
            Document newDoc = new Document();
            using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                this.currentPath = (fileName);

                IFormatter formatter = new BinaryFormatter();

                if (stream.Length != 0)
                {
                    //if stream is not empty, Serialize
                    this.document = formatter.Deserialize(stream) as Document;

                    /* foreach (Text shape in document.TextList)
                     {

                         shape.Init();

                     }
                     */
                    this.Invalidate(true);
                }
                else
                {
                    MessageBox.Show("Stream is empty. Fill the Display!");
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Search search = new Search();
            search.Show();
        }

        //Method CapLockKeyStatus to show the status of the Capslock Key in the status bar
        public void UpdateStatusBar(String text, Color sColor)
        {
            StatusStripLabel.Text = text;
            StatusStripLabel.BackColor = sColor;

            timerTool.Start();

        }
        public void ResetStatusBar()
        {
            StatusStripLabel.Text = "Ready..";
            StatusStripLabel.BackColor = Color.Transparent;
            timerTool.Stop();
        }

        private void toolTimer_tick(object sender, EventArgs e)
        {
            ResetStatusBar();
        }

        private void openTextDialogToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Document.TextList.Count != 0)
            {
                TextDialog textDialog = new TextDialog();
                textDialog.Apply += new EventHandler(Apply);
                textDialog.document = this.Document;
                textDialog.docPos = 0;
                textDialog.Show(this);
            }
        }

        //This function allows plain text to be imported into the main form using an openfiledialog to select a textfile.
        private void importDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream stream;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = openFileDialog.OpenFile()) != null)
                {
                    string strFileName = openFileDialog.FileName;
                    string fileImported = File.ReadAllText(strFileName);

                    //richTextBoxMainForm.Text = fileImported;
                    UpdateStatusBar(currentPath + " has been opened!", Color.LightBlue);
                }
            }
        }

 

        //Method moves the text with the arrow keys
        private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            //detect caps lock key
            if (e.KeyCode == Keys.CapsLock)
            {
                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    UpdateStatusBar("CAPS LOCK ON", Color.Red);
                }
                else
                {
                    UpdateStatusBar("CAPS LOCK OFF", Color.Green);
                }
            }

            if (selectedText != null)
            {
                this.ActiveControl = null;//if theres a new selected item then remove focus from other controls
                //Checks keydown and position of text before it reaches the bounds
                if (e.KeyCode == Keys.Up && SelectedText.Location.Y >= 25) //25 pixels as an offset for the menubar
                {

                    e.IsInputKey = true; // this line disables textbox autofocus
                    document.MoveTextKeys(SelectedText, 0, -5); //moves text
                    Invalidate();
                }
                else if (e.KeyCode == Keys.Right && SelectedText.Location.X <= this.Width - (SelectedText.TextBounds.Width))
                {
                    e.IsInputKey = true;
                    document.MoveTextKeys(SelectedText, 5, 0);
                    Invalidate();
                }
                else if (e.KeyCode == Keys.Left && SelectedText.Location.X >= 0)
                {
                    e.IsInputKey = true;
                    document.MoveTextKeys(SelectedText, -5, 0);
                    Invalidate();
                }
                else if (e.KeyCode == Keys.Down && SelectedText.Location.Y <= this.Height - (SelectedText.TextBounds.Height * 3))
                {
                    e.IsInputKey = true;
                    document.MoveTextKeys(SelectedText, 0, 5);
                    Invalidate();
                }
            }
        }

        private void addTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AddTextDialog addText = new AddTextDialog())
            {
                if (addText.ShowDialog() == DialogResult.OK)
                {
                    string allText = addText.FormText;
                    string[] words = allText.Split(' ');
                    //int pointStart = 0;
                    //int pointV = 0;

                    Console.WriteLine(allText);
                    System.Diagnostics.Debug.WriteLine(allText);

                    foreach (var word in words)
                    {
                        //FontFamily fontF = new FontFamily("Times New Roman");
                        // Font font = new Font(fontF, 10);
                        Graphics g = this.CreateGraphics();

                        System.Diagnostics.Debug.WriteLine(word);

                        Text text = new Text(word, g);

                        this.Document.AddText(text);

                        System.Diagnostics.Debug.WriteLine(Document.TextList.Count);

                        //SolidBrush solidBrush = new SolidBrush(Color.DarkGreen);


                        // g.DrawString(word, font, solidBrush, new PointF(pointStart, pointV));

                        //pointStart += 80;
                        //pointV = 60;
                    }

                    this.Invalidate(true);
                    UpdateStatusBar("Text Added..", Color.Transparent);
                }
            }
        }
        /*
        private void AddTextbutton_Click(object sender, EventArgs e)
        { 
            using (AddTextDialog addText = new AddTextDialog())
            {

                if (addText.ShowDialog() == DialogResult.OK)
                {

                    string allText = addText.FormText;
                    string[] words = allText.Split(' ');
                    //int pointStart = 0;
                    //int pointV = 0;

                   // Console.WriteLine(allText);
                   // System.Diagnostics.Debug.WriteLine(allText);

                    foreach (var word in words)
                    {

                        //FontFamily fontF = new FontFamily("Times New Roman");
                        // Font font = new Font(fontF, 10);
                        Graphics g = this.CreateGraphics();

                        Text text = new Text(word, g);

                        this.Document.AddText(text);

                        //SolidBrush solidBrush = new SolidBrush(Color.DarkGreen);


                        // g.DrawString(word, font, solidBrush, new PointF(pointStart, pointV));

                        //pointStart += 80;
                        //pointV = 60;

                    }

                    this.Invalidate(true);
                }
            }
        }*/

        private void aboutDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WFControlLibrary.AboutDialog about = new WFControlLibrary.AboutDialog();
            about.Show();
        }

        private void oathDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WFControlLibrary.OathDialog oath = new WFControlLibrary.OathDialog();
            oath.Show();
        }

        private void saveAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (SaveFileDialog dlg = new SaveFileDialog())
            {

                dlg.Filter = "PNG Files (*.png)|*.png";
                dlg.DefaultExt = "png";
                dlg.AddExtension = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    var b = new Bitmap(this.Width, this.Height);
                    this.DrawToBitmap(b, new Rectangle(0, 0, this.Width, this.Height));

                    Point p = this.PointToScreen(Point.Empty);

                    Bitmap target = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                    using (Graphics g = Graphics.FromImage(target))
                    {

                        if( attr == null)
                        {

                         g.DrawImage(b, 0, 0,
                                    new Rectangle(p.X - this.Location.X, p.Y - this.Location.Y,
                                                  target.Width, target.Height),
                                   GraphicsUnit.Pixel);

                        }
                        else
                        {

                            Rectangle rectangle = new Rectangle(p.X - this.Location.X, p.Y - this.Location.Y,
                                                  target.Width, target.Height);

                            g.DrawImage(b, new Rectangle(0, 0, rectangle.Width, rectangle.Height), p.X - this.Location.X, p.Y - this.Location.Y, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, attr);//wat

                        }

                    }
                    b.Dispose();
                    target.Save(dlg.FileName);
                    UpdateStatusBar(dlg.FileName + " has been saved!", Color.LightGreen);
                    target.Dispose();
                }
            }
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "PNG Files|*.png*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    currentPath = dlg.FileName;

                    map = null;
                    attr = null;
                    this.Invalidate(true);

                    //ExtractTextFromCurrentPath();
                    UpdateStatusBar(currentPath + "has been opened!", Color.LightBlue);
                }
            }
        }

        private void openDataGridViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridViewDialog gridViewDlg = new GridViewDialog(this.Document);
            gridViewDlg.Apply += new EventHandler(Apply);
            gridViewDlg.DataSource = Document.TextList;
            gridViewDlg.Owner = this;
            gridViewDlg.Show();
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {

            if (this.currentPath.Contains(".png") && e.Button == MouseButtons.Right)
            {
                if (this.attr == null)
                {

                    this.attr = new ImageAttributes();

                }
                    

                using (Bitmap bmp = new Bitmap(currentPath))
                {

                    


                        if (map == null)
                        {

                            map = new ColorMap[1];
                            map[0] = new ColorMap();
                            map[0].OldColor = bmp.GetPixel(e.X, e.Y);
                            map[0].NewColor = bmp.GetPixel(e.X, e.Y);

                            using (ColorDialog dlg = new ColorDialog())
                            {

                                dlg.Color = bmp.GetPixel(e.X, e.Y);

                                if (dlg.ShowDialog() == DialogResult.OK)
                                {

                                    map[0].NewColor = dlg.Color;

                                }


                            }

                        }
                        else
                        {

                            if (findColor(bmp.GetPixel(e.X, e.Y)))
                            {

                                using (ColorDialog dlg = new ColorDialog())
                                {

                                    dlg.Color = bmp.GetPixel(e.X, e.Y);

                                    if (dlg.ShowDialog() == DialogResult.OK)
                                    {

                                        map[this.findColorIndex(bmp.GetPixel(e.X, e.Y))].NewColor = dlg.Color;

                                    }


                                }

                            }
                            else
                            {
                                ColorMap[] newMap = new ColorMap[map.Length + 1];

                                Array.Copy(map, newMap, map.Length);

                                newMap[map.Length] = new ColorMap();
                                newMap[map.Length].OldColor = bmp.GetPixel(e.X, e.Y);
                                newMap[map.Length].NewColor = bmp.GetPixel(e.X, e.Y);

                                using (ColorDialog dlg = new ColorDialog())
                                {

                                    dlg.Color = bmp.GetPixel(e.X, e.Y);

                                    if (dlg.ShowDialog() == DialogResult.OK)
                                    {

                                    newMap[map.Length].NewColor = dlg.Color;


                                    }


                                }

                                map = new ColorMap[newMap.Length];

                               Array.Copy(newMap, map, newMap.Length);//does this work?

                            }

                        }


                    

                   


                }

                this.attr.SetRemapTable(map);

                this.Invalidate(true);
                    
            }

        }

        private Boolean findColor(Color color)
        {

            foreach(ColorMap colorMap in map)
            {

                if (colorMap.OldColor == color)
                    return true;
                

            }

            return false;

        }

        private int findColorIndex(Color color)
        {

            for(int i = 0; i<map.Length; i++)
            {


                if(map[i].OldColor == color)
                {

                    return i;

                }

            }

            return -1;

        }

        private void Mainform_Closing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to exit?\n\n All the current changes will be lost",
                                                   "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void CutToolStripButton_Click(object sender, EventArgs e)
        {
            if (selectedText.Equals(""))
            {
                Clipboard.SetText(selectedText.ToString());
            }

            document.RemoveText(selectedText);

            this.Invalidate();
            UpdateStatusBar("Cut..", Color.Transparent);
        }

        private void PasteToolStripButton_Click(object sender, EventArgs e)
        {
            
            
            Clipboard.GetDataObject();
            UpdateStatusBar("Pasted..", Color.Transparent);
            // document.AddText(selectedText);
        }

        private void CopyToolStripButton_Click(object sender, EventArgs e)
        {
            if (selectedText != null)
            {
                Clipboard.SetText(selectedText.ToString());
                UpdateStatusBar("Copied..", Color.Transparent);
            }
            else
            {
                MessageBox.Show("Select text to copy");
            }
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (document.TextList.Count > 0)
            {
                DialogResult result = MessageBox.Show("Do you want to save your work?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SaveToolStripButton_Click(sender, e);
                    document.ClearText();
                }
                else if (result == DialogResult.No)
                {
                    document.ClearText();
                }
                Invalidate();

            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        void Apply(object sender, EventArgs e)
        {

            this.Invalidate(true);

        }
    }
}
  
