using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WFControlLibrary
{
    public partial class BaseForm : Form
    {
        //Used to store the start position of mouse on MouseDown on base form.
        Point mouseStartPos = Point.Empty;

        public BaseForm()
        {
            InitializeComponent();
           
        }


        private void BaseForm_Load(object sender, EventArgs e)
        {
            DesktopLocation = new Point(1, 1);
            //this.IsMdiContainer = false;
            
        }

        //Opens the color dialog
        private void ColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog cdlg = new ColorDialog())
            {
                //Store previous back color
                Color previousColor = this.BackColor;
                if( cdlg.ShowDialog(this) == DialogResult.OK)
                {
                    this.BackColor = cdlg.Color;
                }else
                {
                    this.BackColor = previousColor;
                }
            }

        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Dispose of the form
            Dispose();
        }

        //Store mouse vector on mouse down (start position)
        private void BaseForm_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Focus();
                mouseStartPos = new Point(e.X, e.Y);
            }
        }

        //Move the form with the mouse drag
        private void BaseForm_MouseMove(object sender, MouseEventArgs e)
        {
            if(mouseStartPos == Point.Empty)
            {
                return;
            }
            //Vector difference between two points, the start point and the end point with the top left point considered.
            Point location = new Point(this.Left + e.X - mouseStartPos.X, this.Top + e.Y - mouseStartPos.Y); 
            this.Location = location;
        }

        //Reset mouse start pos which is set on mouse down
        private void BaseForm_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                mouseStartPos = Point.Empty; //Resetting..
            }
        }

        private void ColorsToolStripMenuItemBase_Click(object sender, EventArgs e)
        {
            ColorsToolStripMenuItem_Click(sender, e);
        }

        private void FileToolStripMenuItemBase_Click(object sender, EventArgs e)
        {
            CloseToolStripMenuItem_Click(sender, e);
        }
        //modify
        private void CloseChildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
