namespace BrewAndBiteCafe
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblCafeTitle;
        private System.Windows.Forms.Label lblLoginTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel lnkForgotPassword;
        private System.Windows.Forms.LinkLabel lnkSignUp;
        private System.Windows.Forms.LinkLabel lnkAbout;
        //
        private System.Windows.Forms.PictureBox picBackground;
        private System.Windows.Forms.PictureBox picCoffee;
        private System.Windows.Forms.Label lblCafeLine1;
        private System.Windows.Forms.Label lblCafeLine2;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelCard;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblCafeTitle      = new System.Windows.Forms.Label();
            this.lblLoginTitle     = new System.Windows.Forms.Label();
            this.lblUsername       = new System.Windows.Forms.Label();
            this.txtUsername       = new System.Windows.Forms.TextBox();
            this.lblPassword       = new System.Windows.Forms.Label();
            this.txtPassword       = new System.Windows.Forms.TextBox();
            this.chkShowPassword   = new System.Windows.Forms.CheckBox();
            this.btnLogin          = new System.Windows.Forms.Button();
            this.lnkForgotPassword = new System.Windows.Forms.LinkLabel();
            this.lnkSignUp         = new System.Windows.Forms.LinkLabel();
            this.lnkAbout          = new System.Windows.Forms.LinkLabel();
            this.picBackground     = new System.Windows.Forms.PictureBox();
            this.picCoffee         = new System.Windows.Forms.PictureBox();
            this.lblCafeLine1      = new System.Windows.Forms.Label();
            this.lblCafeLine2      = new System.Windows.Forms.Label();
            this.panelLeft         = new System.Windows.Forms.Panel();
            this.panelRight        = new System.Windows.Forms.Panel();
            this.panelCard         = new System.Windows.Forms.Panel();

            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCoffee)).BeginInit();
            this.SuspendLayout();


            // lblCafeTitle
            this.lblCafeTitle.AutoSize  = false;
            this.lblCafeTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblCafeTitle.Font      = new System.Drawing.Font("Lucida Handwriting", 17F, System.Drawing.FontStyle.Bold);
            this.lblCafeTitle.ForeColor = System.Drawing.Color.Transparent;
            this.lblCafeTitle.Location  = new System.Drawing.Point(250, 68);
            this.lblCafeTitle.Name      = "lblCafeTitle";
            this.lblCafeTitle.Size      = new System.Drawing.Size(400, 46);
            this.lblCafeTitle.TabIndex  = 1;
            this.lblCafeTitle.Text      = "Brew & Bite Cafe";
            this.lblCafeTitle.UseMnemonic = false;
            this.lblCafeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            //  lblLoginTitle 
            this.lblLoginTitle.AutoSize  = false;
            this.lblLoginTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginTitle.Font      = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblLoginTitle.ForeColor = System.Drawing.Color.Transparent;
            this.lblLoginTitle.Location  = new System.Drawing.Point(250, 118);
            this.lblLoginTitle.Name      = "lblLoginTitle";
            this.lblLoginTitle.Size      = new System.Drawing.Size(400, 50);
            this.lblLoginTitle.TabIndex  = 2;
            this.lblLoginTitle.Text      = "Login";
            this.lblLoginTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblUsername 
            this.lblUsername.AutoSize  = false;
            this.lblUsername.BackColor = System.Drawing.Color.Transparent;
            this.lblUsername.Font      = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.Transparent;
            this.lblUsername.Location  = new System.Drawing.Point(315, 186);
            this.lblUsername.Name      = "lblUsername";
            this.lblUsername.Size      = new System.Drawing.Size(120, 28);
            this.lblUsername.TabIndex  = 3;
            this.lblUsername.Text      = "Username";

            // txtUsername
            this.txtUsername.Font     = new System.Drawing.Font("Segoe UI", 12F);
            this.txtUsername.Location = new System.Drawing.Point(315, 218);
            this.txtUsername.Name     = "txtUsername";
            this.txtUsername.Size     = new System.Drawing.Size(270, 30);
            this.txtUsername.TabIndex = 4;

            // lblPassword 
            this.lblPassword.AutoSize  = false;
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Font      = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.Transparent;
            this.lblPassword.Location  = new System.Drawing.Point(315, 262);
            this.lblPassword.Name      = "lblPassword";
            this.lblPassword.Size      = new System.Drawing.Size(120, 28);
            this.lblPassword.TabIndex  = 5;
            this.lblPassword.Text      = "Password";

            // txtPassword
            this.txtPassword.Font         = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPassword.Location     = new System.Drawing.Point(315, 294);
            this.txtPassword.Name         = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size         = new System.Drawing.Size(270, 30);
            this.txtPassword.TabIndex     = 6;

            // chkShowPassword
            this.chkShowPassword.AutoSize            = false;
            this.chkShowPassword.BackColor           = System.Drawing.Color.Transparent;
            this.chkShowPassword.Font                = new System.Drawing.Font("Segoe UI", 10F);
            this.chkShowPassword.ForeColor           = System.Drawing.Color.Transparent;
            this.chkShowPassword.Location            = new System.Drawing.Point(315, 338);
            this.chkShowPassword.Name                = "chkShowPassword";
            this.chkShowPassword.Size                = new System.Drawing.Size(160, 26);
            this.chkShowPassword.TabIndex            = 7;
            this.chkShowPassword.Text                = "Show Password";
            this.chkShowPassword.UseVisualStyleBackColor = false;
            this.chkShowPassword.CheckedChanged     += new System.EventHandler(this.chkShowPassword_CheckedChanged);

            // btnLogin
            this.btnLogin.BackColor              = System.Drawing.Color.FromArgb(30, 18, 8);
            this.btnLogin.FlatStyle              = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Font                   = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor              = System.Drawing.Color.White;
            this.btnLogin.Location               = new System.Drawing.Point(315, 376);
            this.btnLogin.Name                   = "btnLogin";
            this.btnLogin.Size                   = new System.Drawing.Size(270, 46);
            this.btnLogin.TabIndex               = 8;
            this.btnLogin.Text                   = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click                 += new System.EventHandler(this.btnLogin_Click);

            // lnkForgotPassword
            this.lnkForgotPassword.ActiveLinkColor = System.Drawing.Color.White;
            this.lnkForgotPassword.BackColor       = System.Drawing.Color.Transparent;
            this.lnkForgotPassword.Font            = new System.Drawing.Font("Segoe UI", 10F);
            this.lnkForgotPassword.ForeColor       = System.Drawing.Color.Transparent;
            this.lnkForgotPassword.LinkColor       = System.Drawing.Color.Transparent;
            this.lnkForgotPassword.Location        = new System.Drawing.Point(360, 436);
            this.lnkForgotPassword.Name            = "lnkForgotPassword";
            this.lnkForgotPassword.Size            = new System.Drawing.Size(180, 22);
            this.lnkForgotPassword.TabIndex        = 9;
            this.lnkForgotPassword.TabStop         = true;
            this.lnkForgotPassword.Text            = "Forgot Password?";
            this.lnkForgotPassword.TextAlign       = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkForgotPassword.LinkClicked     += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkForgotPassword_LinkClicked);

            // lnkSignUp
            this.lnkSignUp.ActiveLinkColor = System.Drawing.Color.White;
            this.lnkSignUp.BackColor       = System.Drawing.Color.Transparent;
            this.lnkSignUp.Font            = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkSignUp.ForeColor       = System.Drawing.Color.Transparent;
            this.lnkSignUp.LinkColor       = System.Drawing.Color.Transparent;
            this.lnkSignUp.Location        = new System.Drawing.Point(295, 464);
            this.lnkSignUp.Name            = "lnkSignUp";
            this.lnkSignUp.Size            = new System.Drawing.Size(310, 22);
            this.lnkSignUp.TabIndex        = 10;
            this.lnkSignUp.TabStop         = true;
            this.lnkSignUp.Text            = "Don't have an account? Sign Up";
            this.lnkSignUp.TextAlign       = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkSignUp.LinkClicked     += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSignUp_LinkClicked);

            // lnkAbout (hidden)
            this.lnkAbout.BackColor = System.Drawing.Color.Transparent;
            this.lnkAbout.Location  = new System.Drawing.Point(360, 494);
            this.lnkAbout.Name      = "lnkAbout";
            this.lnkAbout.Size      = new System.Drawing.Size(180, 20);
            this.lnkAbout.TabIndex  = 11;
            this.lnkAbout.Text      = "About Program";
            this.lnkAbout.Visible   = false;

            // stubs (hidden)
            this.picBackground.Visible = false;
            this.picCoffee.Visible     = false;
            this.lblCafeLine1.Visible  = false;
            this.lblCafeLine2.Visible  = false;
            this.panelLeft.Visible     = false;
            this.panelRight.Visible    = false;
            this.panelCard.Visible     = false;

            // LoginForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(900, 600);
            this.BackColor           = System.Drawing.Color.Black;
            this.Controls.Add(this.lnkAbout);
            this.Controls.Add(this.lnkSignUp);
            this.Controls.Add(this.lnkForgotPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblLoginTitle);
            this.Controls.Add(this.lblCafeTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.Name            = "LoginForm";
            this.Text            = "Login";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;

            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCoffee)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
