namespace FilledContainer
{
    partial class demo
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
            this.txtFree = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.filledContainer = new FilledContainer.VerticalFilledContainer();
            this.SuspendLayout();
            // 
            // txtFree
            // 
            this.txtFree.Location = new System.Drawing.Point(45, 269);
            this.txtFree.Name = "txtFree";
            this.txtFree.Size = new System.Drawing.Size(61, 20);
            this.txtFree.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Value";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(114, 267);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(92, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Demo";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // filledContainer
            // 
            this.filledContainer.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.filledContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filledContainer.ForeColor = System.Drawing.Color.SkyBlue;
            this.filledContainer.GlossyBackground = true;
            this.filledContainer.Location = new System.Drawing.Point(235, 12);
            this.filledContainer.Name = "filledContainer";
            this.filledContainer.Size = new System.Drawing.Size(35, 278);
            this.filledContainer.TabIndex = 7;
            this.filledContainer.Value = 10F;
            // 
            // demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 317);
            this.Controls.Add(this.filledContainer);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFree);
            this.Name = "demo";
            this.Text = "demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdate;
        private VerticalFilledContainer filledContainer;
    }
}