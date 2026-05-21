namespace BrewAndBiteCafe
{
    partial class SignUpForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Button btnSignUp;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLoginLink;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMain          = new System.Windows.Forms.Panel();
            this.lblTitle           = new System.Windows.Forms.Label();
            this.lblEmail           = new System.Windows.Forms.Label();
            this.txtEmail           = new System.Windows.Forms.TextBox();
            this.lblUsername        = new System.Windows.Forms.Label();
            this.txtUsername        = new System.Windows.Forms.TextBox();
            this.lblFullName        = new System.Windows.Forms.Label();
            this.txtFullName        = new System.Windows.Forms.TextBox();
            this.lblPassword        = new System.Windows.Forms.Label();
            this.txtPassword        = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.chkShowPassword    = new System.Windows.Forms.CheckBox();
            this.btnSignUp          = new System.Windows.Forms.Button();
            this.btnCancel          = new System.Windows.Forms.Button();
            this.lblLoginLink       = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();

            // panelMain
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Controls.Add(this.lblEmail);
            this.panelMain.Controls.Add(this.txtEmail);
            this.panelMain.Controls.Add(this.lblUsername);
            this.panelMain.Controls.Add(this.txtUsername);
            this.panelMain.Controls.Add(this.lblFullName);
            this.panelMain.Controls.Add(this.txtFullName);
            this.panelMain.Controls.Add(this.lblPassword);
            this.panelMain.Controls.Add(this.txtPassword);
            this.panelMain.Controls.Add(this.lblConfirmPassword);
            this.panelMain.Controls.Add(this.txtConfirmPassword);
            this.panelMain.Controls.Add(this.chkShowPassword);
            this.panelMain.Controls.Add(this.btnSignUp);
            this.panelMain.Controls.Add(this.btnCancel);
            this.panelMain.Controls.Add(this.lblLoginLink);
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name     = "panelMain";
            this.panelMain.Size     = new System.Drawing.Size(450, 640);
            this.panelMain.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize  = true;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(80, 60, 40);
            this.lblTitle.Location  = new System.Drawing.Point(130, 20);
            this.lblTitle.Name      = "lblTitle";
            this.lblTitle.TabIndex  = 0;
            this.lblTitle.Text      = "Sign Up";

            // lblEmail
            this.lblEmail.AutoSize  = true;
            this.lblEmail.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(80, 60, 40);
            this.lblEmail.Location  = new System.Drawing.Point(50, 82);
            this.lblEmail.Name      = "lblEmail";
            this.lblEmail.TabIndex  = 1;
            this.lblEmail.Text      = "Email";

            // txtEmail
            this.txtEmail.Font     = new System.Drawing.Font("Segoe UI", 12F);
            this.txtEmail.Location = new System.Drawing.Point(50, 106);
            this.txtEmail.Name     = "txtEmail";
            this.txtEmail.Size     = new System.Drawing.Size(350, 30);
            this.txtEmail.TabIndex = 2;

            // lblUsername
            this.lblUsername.AutoSize  = true;
            this.lblUsername.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(80, 60, 40);
            this.lblUsername.Location  = new System.Drawing.Point(50, 152);
            this.lblUsername.Name      = "lblUsername";
            this.lblUsername.TabIndex  = 3;
            this.lblUsername.Text      = "Username";

            // txtUsername
            this.txtUsername.Font     = new System.Drawing.Font("Segoe UI", 12F);
            this.txtUsername.Location = new System.Drawing.Point(50, 176);
            this.txtUsername.Name     = "txtUsername";
            this.txtUsername.Size     = new System.Drawing.Size(350, 30);
            this.txtUsername.TabIndex = 4;

            // lblFullName
            this.lblFullName.AutoSize  = true;
            this.lblFullName.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFullName.ForeColor = System.Drawing.Color.FromArgb(80, 60, 40);
            this.lblFullName.Location  = new System.Drawing.Point(50, 222);
            this.lblFullName.Name      = "lblFullName";
            this.lblFullName.TabIndex  = 5;
            this.lblFullName.Text      = "Full Name";

            // txtFullName
            this.txtFullName.Font     = new System.Drawing.Font("Segoe UI", 12F);
            this.txtFullName.Location = new System.Drawing.Point(50, 246);
            this.txtFullName.Name     = "txtFullName";
            this.txtFullName.Size     = new System.Drawing.Size(350, 30);
            this.txtFullName.TabIndex = 6;

            // lblPassword
            this.lblPassword.AutoSize  = true;
            this.lblPassword.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(80, 60, 40);
            this.lblPassword.Location  = new System.Drawing.Point(50, 292);
            this.lblPassword.Name      = "lblPassword";
            this.lblPassword.TabIndex  = 7;
            this.lblPassword.Text      = "Password";

            // txtPassword
            this.txtPassword.Font         = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPassword.Location     = new System.Drawing.Point(50, 316);
            this.txtPassword.Name         = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size         = new System.Drawing.Size(350, 30);
            this.txtPassword.TabIndex     = 8;

            // lblConfirmPassword
            this.lblConfirmPassword.AutoSize  = true;
            this.lblConfirmPassword.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblConfirmPassword.ForeColor = System.Drawing.Color.FromArgb(80, 60, 40);
            this.lblConfirmPassword.Location  = new System.Drawing.Point(50, 362);
            this.lblConfirmPassword.Name      = "lblConfirmPassword";
            this.lblConfirmPassword.TabIndex  = 9;
            this.lblConfirmPassword.Text      = "Confirm Password";

            // txtConfirmPassword
            this.txtConfirmPassword.Font         = new System.Drawing.Font("Segoe UI", 12F);
            this.txtConfirmPassword.Location     = new System.Drawing.Point(50, 386);
            this.txtConfirmPassword.Name         = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size         = new System.Drawing.Size(350, 30);
            this.txtConfirmPassword.TabIndex     = 10;

            // chkShowPassword
            this.chkShowPassword.AutoSize  = true;
            this.chkShowPassword.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.chkShowPassword.Location  = new System.Drawing.Point(50, 428);
            this.chkShowPassword.Name      = "chkShowPassword";
            this.chkShowPassword.TabIndex  = 11;
            this.chkShowPassword.Text      = "Show Password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);

            // btnSignUp
            this.btnSignUp.BackColor            = System.Drawing.Color.FromArgb(139, 90, 43);
            this.btnSignUp.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignUp.Font                 = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnSignUp.ForeColor            = System.Drawing.Color.White;
            this.btnSignUp.Location             = new System.Drawing.Point(50, 464);
            this.btnSignUp.Name                 = "btnSignUp";
            this.btnSignUp.Size                 = new System.Drawing.Size(350, 44);
            this.btnSignUp.TabIndex             = 12;
            this.btnSignUp.Text                 = "Sign Up";
            this.btnSignUp.UseVisualStyleBackColor = false;
            this.btnSignUp.Click               += new System.EventHandler(this.btnSignUp_Click);

            // btnCancel (hidden)
            this.btnCancel.Visible  = false;
            this.btnCancel.Name     = "btnCancel";
            this.btnCancel.TabIndex = 13;

            // lblLoginLink
            this.lblLoginLink.AutoSize  = false;
            this.lblLoginLink.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.lblLoginLink.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLoginLink.ForeColor = System.Drawing.Color.FromArgb(139, 90, 43);
            this.lblLoginLink.Location  = new System.Drawing.Point(40, 518);
            this.lblLoginLink.Name      = "lblLoginLink";
            this.lblLoginLink.Size      = new System.Drawing.Size(370, 20);
            this.lblLoginLink.TabIndex  = 14;
            this.lblLoginLink.Text      = "Already have an account? Login";
            this.lblLoginLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoginLink.Click    += new System.EventHandler(this.lblLoginLink_Click);

            // SignUpForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(450, 554);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox         = false;
            this.MinimizeBox         = false;
            this.Name                = "SignUpForm";
            this.Text                = "Sign Up";
            this.StartPosition       = System.Windows.Forms.FormStartPosition.CenterParent;
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
