namespace FilledContainer
{
    partial class FilledContainer
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
            this.components = new System.ComponentModel.Container();
            this.plot = new ZedGraph.ZedGraphControl();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // plot
            // 
            this.plot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plot.Location = new System.Drawing.Point(0, 0);
            this.plot.Name = "plot";
            this.plot.PanButtons = System.Windows.Forms.MouseButtons.None;
            this.plot.PanButtons2 = System.Windows.Forms.MouseButtons.None;
            this.plot.ScrollGrace = 0D;
            this.plot.ScrollMaxX = 0D;
            this.plot.ScrollMaxY = 0D;
            this.plot.ScrollMaxY2 = 0D;
            this.plot.ScrollMinX = 0D;
            this.plot.ScrollMinY = 0D;
            this.plot.ScrollMinY2 = 0D;
            this.plot.Size = new System.Drawing.Size(139, 349);
            this.plot.TabIndex = 0;
            this.plot.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            this.plot.Load += new System.EventHandler(this.plot_Load);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // FilledContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.plot);
            this.Name = "FilledContainer";
            this.Size = new System.Drawing.Size(139, 349);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl plot;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
