namespace WFControlLibrary
{
    partial class BaseDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainPanel = new System.Windows.Forms.Panel();
            this.classInfo = new WFControlLibrary.classInfo();
            this.namesControl = new WFControlLibrary.namesControl();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.Color.Transparent;
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 109);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(453, 251);
            this.MainPanel.TabIndex = 2;
            this.MainPanel.ParentChanged += new System.EventHandler(this.MainPanel_ParentChanged);
            // 
            // classInfo
            // 
            this.classInfo.BackColor = System.Drawing.Color.Beige;
            this.classInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.classInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.classInfo.Location = new System.Drawing.Point(0, 0);
            this.classInfo.Name = "classInfo";
            this.classInfo.Size = new System.Drawing.Size(453, 109);
            this.classInfo.TabIndex = 1;
            // 
            // namesControl
            // 
            this.namesControl.BackColor = System.Drawing.Color.Beige;
            this.namesControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.namesControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.namesControl.Location = new System.Drawing.Point(0, 360);
            this.namesControl.Name = "namesControl";
            this.namesControl.Size = new System.Drawing.Size(453, 148);
            this.namesControl.TabIndex = 0;
            // 
            // BaseDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 508);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.classInfo);
            this.Controls.Add(this.namesControl);
            this.Name = "BaseDialog";
            this.Text = "BaseDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private namesControl namesControl;
        private classInfo classInfo;
        protected System.Windows.Forms.Panel MainPanel;
    }
}