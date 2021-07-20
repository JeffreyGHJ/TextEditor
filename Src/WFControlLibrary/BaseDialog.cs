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
    public partial class BaseDialog : Form
    {
        public BaseDialog()
        {
            InitializeComponent();
        }

        private void MainPanel_ParentChanged(object sender, EventArgs e)
        {
            
            UpdatePanelImageAndColor(sender);
        }
        //updates the panel image and color
        private void UpdatePanelImageAndColor(object sender)
        {
            Control parent = sender as Control;
            if(parent != null)
            {
                if (parent.BackgroundImage != null)
                    MainPanel.BackgroundImage = parent.BackgroundImage;
                MainPanel.BackColor = parent.BackColor;
            }     
        }
    }
}
