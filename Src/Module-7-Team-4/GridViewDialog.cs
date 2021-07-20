using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module_7_Team_4
{
    public partial class GridViewDialog : Form
    {
        public event EventHandler Apply;

        public IList<Text> DataSource
        {
            get { return (IList<Text>) this.dataGridView.DataSource; }
            set { this.dataGridView.DataSource = value; }
        }

        public GridViewDialog(Document document)
        {
            InitializeComponent();
        }

        //Remove columns for properties that should not be editable
        private void GridViewDialog_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn c in dataGridView.Columns)
            {
                if (c.Name == "TextBounds" 
                    || c.Name == "LocationSetByUser" 
                    || c.Name == "CenterPoint" 
                    || c.Name == "HitTestRegion" 
                    || c.Name == "FontChanged")
                {
                    c.Visible = false;
                }
            }
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null)
            {
                DataSource.Insert(0, new Text("[DEFAULT TEXT]", CreateGraphics()));
            }
            else
            {
                DataSource.Insert(dataGridView.CurrentRow.Index, new Text("[DEFAULT TEXT]", CreateGraphics()));
            }

            ApplyHelper(sender, e);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if ( dataGridView.CurrentRow != null && DataSource[dataGridView.CurrentRow.Index] != null )
            {
                DataSource.Remove(DataSource[dataGridView.CurrentRow.Index]);
                ApplyHelper(sender, e);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            DataSource.Add(new Text("[DEFAULT TEXT]", CreateGraphics()));
            ApplyHelper(sender, e);
        }

        private void ApplyHelper(object sender, EventArgs e)
        {
            if (Apply != null)
            {
                Apply(this, EventArgs.Empty);
            }
        }
    }
}
