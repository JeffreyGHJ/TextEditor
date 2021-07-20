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
    public partial class AddTextDialog : Form, IAddText
    {

        public string FormText { get; set; }

        public AddTextDialog()
        {
            InitializeComponent();
            //this.Paint += new PaintEventHandler(set_background);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {

            FormText = this.textBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();


        }

        //Method CopyFromRichTextBoxToClipBoard_Click allows plain text to be copied to clipboard
        private void CopyFromRichTextBoxToClipBoard_Click(object sender, EventArgs e)
        {
            if (textBox.SelectedText == "")
            {
                MessageBox.Show("Please, select the text.");
            }
            else
            {
                Clipboard.SetText(textBox.SelectedText);
            }
        }
        //Method PasteToRichTextBoxFromClipboard_Click allows plain text to be copied from clipboard
        private void PasteToRichTextBoxFromClipboard_Click(object sender, EventArgs e)
        {
            textBox.AppendText("\r\n" + Clipboard.GetText());
        }

        public void Add(string line)
        {
            this.textBox.Text += line;
        }
        /*
        private void set_background(Object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            Rectangle gradient_rectangle = new Rectangle(0, 0, Width, Height);

            //define gradient properties
            Brush b = new LinearGradientBrush(gradient_rectangle, Color.FromArgb(200, 200, 200), Color.FromArgb(200, 100, 100), 65f);

            //apply gradient         
            graphics.FillRectangle(b, gradient_rectangle);
        }
        */
    }
}
