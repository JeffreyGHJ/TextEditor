namespace WFControlLibrary
{
    partial class namesControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.namesLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // namesLabel
            // 
            this.namesLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.namesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.namesLabel.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.namesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.namesLabel.Location = new System.Drawing.Point(0, 0);
            this.namesLabel.Name = "namesLabel";
            this.namesLabel.Size = new System.Drawing.Size(168, 222);
            this.namesLabel.TabIndex = 0;
            this.namesLabel.Text = "Group 4:\r\nJeremiah Mirander\r\nAlex Batista\r\nDaniel Perez\r\nIsumy Aguila\r\nYeilys Fundora\r\nSebastian Rodriguez\r\nJeffrey Hernadez";
            this.namesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // namesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.namesLabel);
            this.Name = "namesControl";
            this.Size = new System.Drawing.Size(168, 222);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label namesLabel;
    }
}
