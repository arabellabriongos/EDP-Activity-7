namespace BrewAndBiteCafe
{
    partial class AboutForm
    {
        private System.ComponentModel.IContainer components = null;

        // Legacy controls kept for compatibility - layout is built in code
        private System.Windows.Forms.Panel panelMain = new System.Windows.Forms.Panel();
        private System.Windows.Forms.PictureBox picLogo = new System.Windows.Forms.PictureBox();
        private System.Windows.Forms.Label lblAppName = new System.Windows.Forms.Label();
        private System.Windows.Forms.Label lblAppNameLine2 = new System.Windows.Forms.Label();
        private System.Windows.Forms.Label lblVersion = new System.Windows.Forms.Label();
        private System.Windows.Forms.Label lblDeveloper = new System.Windows.Forms.Label();
        private System.Windows.Forms.Label lblCourse = new System.Windows.Forms.Label();
        private System.Windows.Forms.Label lblSchool = new System.Windows.Forms.Label();
        private System.Windows.Forms.Label lblDescription = new System.Windows.Forms.Label();
        private System.Windows.Forms.Label lblCopyright = new System.Windows.Forms.Label();
        private System.Windows.Forms.Button btnClose = new System.Windows.Forms.Button();

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 480);
            this.Name = "AboutForm";
            this.Text = "About";
            this.ResumeLayout(false);
        }
    }
}
