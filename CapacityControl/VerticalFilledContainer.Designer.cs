namespace FilledContainer
{
    partial class VerticalFilledContainer
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
            this.pnlGloss = new System.Windows.Forms.Panel();
            this.pnlValue = new System.Windows.Forms.Panel();
            this.pnlValueGloss = new System.Windows.Forms.Panel();
            this.pnlOverflow = new System.Windows.Forms.Panel();
            this.pnlValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlGloss
            // 
            this.pnlGloss.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlGloss.Location = new System.Drawing.Point(0, 0);
            this.pnlGloss.Name = "pnlGloss";
            this.pnlGloss.Size = new System.Drawing.Size(10, 148);
            this.pnlGloss.TabIndex = 0;
            // 
            // pnlValue
            // 
            this.pnlValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.pnlValue.Controls.Add(this.pnlValueGloss);
            this.pnlValue.Location = new System.Drawing.Point(26, 98);
            this.pnlValue.Name = "pnlValue";
            this.pnlValue.Size = new System.Drawing.Size(106, 34);
            this.pnlValue.TabIndex = 1;
            // 
            // pnlValueGloss
            // 
            this.pnlValueGloss.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlValueGloss.Location = new System.Drawing.Point(0, 0);
            this.pnlValueGloss.Name = "pnlValueGloss";
            this.pnlValueGloss.Size = new System.Drawing.Size(10, 34);
            this.pnlValueGloss.TabIndex = 1;
            // 
            // pnlOverflow
            // 
            this.pnlOverflow.BackColor = System.Drawing.Color.Brown;
            this.pnlOverflow.BackgroundImage = global::FilledContainer.Properties.Resources.gtk_dialog_warning;
            this.pnlOverflow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlOverflow.Location = new System.Drawing.Point(26, 34);
            this.pnlOverflow.Name = "pnlOverflow";
            this.pnlOverflow.Size = new System.Drawing.Size(93, 58);
            this.pnlOverflow.TabIndex = 2;
            this.pnlOverflow.Visible = false;
            this.pnlOverflow.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlOverflow_Paint);
            // 
            // MonoFilledContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlOverflow);
            this.Controls.Add(this.pnlValue);
            this.Controls.Add(this.pnlGloss);
            this.Name = "MonoFilledContainer";
            this.Size = new System.Drawing.Size(105, 148);
            this.Load += new System.EventHandler(this.MonoFilledContainer_Load);
            this.pnlValue.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlGloss;
        private System.Windows.Forms.Panel pnlValue;
        private System.Windows.Forms.Panel pnlValueGloss;
        private System.Windows.Forms.Panel pnlOverflow;
    }
}
