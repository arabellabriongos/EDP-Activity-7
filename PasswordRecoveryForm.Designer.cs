namespace BrewAndBiteCafe
{
    partial class PasswordRecoveryForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInstruction;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnSendCode;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel panelCode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Label lblResult;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMain  = new System.Windows.Forms.Panel();
            this.lblTitle   = new System.Windows.Forms.Label();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.lblEmail   = new System.Windows.Forms.Label();
            this.txtEmail   = new System.Windows.Forms.TextBox();
            this.btnSendCode = new System.Windows.Forms.Button();
            this.btnBack    = new System.Windows.Forms.Button();
            this.panelCode  = new System.Windows.Forms.Panel();
            this.lblCode    = new System.Windows.Forms.Label();
            this.txtCode    = new System.Windows.Forms.TextBox();
            this.btnVerify  = new System.Windows.Forms.Button();
            this.lblResult  = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.panelCode.SuspendLayout();
            this.SuspendLayout();

            // panelMain
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Controls.Add(this.lblInstruction);
            this.panelMain.Controls.Add(this.lblEmail);
            this.panelMain.Controls.Add(this.txtEmail);
            this.panelMain.Controls.Add(this.btnSendCode);
            this.panelMain.Controls.Add(this.btnBack);
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name     = "panelMain";
            this.panelMain.Size     = new System.Drawing.Size(460, 320);
            this.panelMain.TabIndex = 0;

            // lblTitle — left-aligned
            this.lblTitle.AutoSize  = true;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(74, 53, 37);
            this.lblTitle.Location  = new System.Drawing.Point(40, 28);
            this.lblTitle.Name      = "lblTitle";
            this.lblTitle.TabIndex  = 0;
            this.lblTitle.Text      = "Password Recovery";

            // lblInstruction — left-aligned, above email label
            this.lblInstruction.AutoSize  = false;
            this.lblInstruction.Font      = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblInstruction.ForeColor = System.Drawing.Color.FromArgb(120, 100, 80);
            this.lblInstruction.Location  = new System.Drawing.Point(40, 72);
            this.lblInstruction.Name      = "lblInstruction";
            this.lblInstruction.Size      = new System.Drawing.Size(380, 36);
            this.lblInstruction.TabIndex  = 1;
            this.lblInstruction.Text      = "Enter your registered email address to receive a verification code.";
            this.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            // lblEmail
            this.lblEmail.AutoSize  = true;
            this.lblEmail.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(74, 53, 37);
            this.lblEmail.Location  = new System.Drawing.Point(40, 118);
            this.lblEmail.Name      = "lblEmail";
            this.lblEmail.TabIndex  = 2;
            this.lblEmail.Text      = "Email";

            // txtEmail
            this.txtEmail.Font     = new System.Drawing.Font("Segoe UI", 11F);
            this.txtEmail.Location = new System.Drawing.Point(40, 140);
            this.txtEmail.Name     = "txtEmail";
            this.txtEmail.Size     = new System.Drawing.Size(380, 27);
            this.txtEmail.TabIndex = 3;

            // btnSendCode — rounded via FlatStyle, normal width
            this.btnSendCode.BackColor            = System.Drawing.Color.FromArgb(139, 90, 43);
            this.btnSendCode.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendCode.FlatAppearance.BorderSize  = 0;
            this.btnSendCode.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(139, 90, 43);
            this.btnSendCode.Font                 = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSendCode.ForeColor            = System.Drawing.Color.White;
            this.btnSendCode.Location             = new System.Drawing.Point(40, 184);
            this.btnSendCode.Name                 = "btnSendCode";
            this.btnSendCode.Size                 = new System.Drawing.Size(380, 38);
            this.btnSendCode.TabIndex             = 4;
            this.btnSendCode.Text                 = "Send Verification Code";
            this.btnSendCode.UseVisualStyleBackColor = false;
            this.btnSendCode.Click               += new System.EventHandler(this.btnSendCode_Click);

            // btnBack — link-style (no background)
            this.btnBack.BackColor            = System.Drawing.Color.Transparent;
            this.btnBack.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.FlatAppearance.BorderSize  = 0;
            this.btnBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBack.Font                 = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnBack.ForeColor            = System.Drawing.Color.FromArgb(139, 90, 43);
            this.btnBack.Location             = new System.Drawing.Point(40, 236);
            this.btnBack.Name                 = "btnBack";
            this.btnBack.Size                 = new System.Drawing.Size(380, 28);
            this.btnBack.TabIndex             = 5;
            this.btnBack.Text                 = "← Back to Login";
            this.btnBack.TextAlign            = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click               += new System.EventHandler(this.btnBack_Click);

            // panelCode
            this.panelCode.BackColor = System.Drawing.Color.White;
            this.panelCode.Controls.Add(this.lblCode);
            this.panelCode.Controls.Add(this.txtCode);
            this.panelCode.Controls.Add(this.btnVerify);
            this.panelCode.Controls.Add(this.lblResult);
            this.panelCode.Location = new System.Drawing.Point(0, 0);
            this.panelCode.Name     = "panelCode";
            this.panelCode.Size     = new System.Drawing.Size(460, 320);
            this.panelCode.TabIndex = 6;
            this.panelCode.Visible  = false;

            // lblCode
            this.lblCode.AutoSize  = true;
            this.lblCode.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCode.ForeColor = System.Drawing.Color.FromArgb(74, 53, 37);
            this.lblCode.Location  = new System.Drawing.Point(40, 60);
            this.lblCode.Name      = "lblCode";
            this.lblCode.TabIndex  = 0;
            this.lblCode.Text      = "Verification Code";

            // txtCode
            this.txtCode.Font      = new System.Drawing.Font("Segoe UI", 13F);
            this.txtCode.Location  = new System.Drawing.Point(40, 84);
            this.txtCode.Name      = "txtCode";
            this.txtCode.Size      = new System.Drawing.Size(380, 32);
            this.txtCode.TabIndex  = 1;
            this.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            // btnVerify — rounded
            this.btnVerify.BackColor            = System.Drawing.Color.FromArgb(139, 90, 43);
            this.btnVerify.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerify.FlatAppearance.BorderSize  = 0;
            this.btnVerify.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(139, 90, 43);
            this.btnVerify.Font                 = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnVerify.ForeColor            = System.Drawing.Color.White;
            this.btnVerify.Location             = new System.Drawing.Point(40, 134);
            this.btnVerify.Name                 = "btnVerify";
            this.btnVerify.Size                 = new System.Drawing.Size(380, 38);
            this.btnVerify.TabIndex             = 2;
            this.btnVerify.Text                 = "Verify Code";
            this.btnVerify.UseVisualStyleBackColor = false;
            this.btnVerify.Click               += new System.EventHandler(this.btnVerify_Click);

            // lblResult
            this.lblResult.AutoSize  = true;
            this.lblResult.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblResult.ForeColor = System.Drawing.Color.Green;
            this.lblResult.Location  = new System.Drawing.Point(40, 186);
            this.lblResult.Name      = "lblResult";
            this.lblResult.TabIndex  = 3;

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(460, 320);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.Name            = "PasswordRecoveryForm";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text            = "Password Recovery";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelCode.ResumeLayout(false);
            this.panelCode.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
