using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public partial class LoginForm : Form
    {
        private static readonly string CafeTitleText  = "Brew & Bite Cafe";
        private static readonly string LoginTitleText = "Login";
        private static readonly string UsernameText   = "Username";
        private static readonly string PasswordText   = "Password";

        public LoginForm()
        {
            InitializeComponent();
            Load += LoginForm_Load;

            lblCafeTitle.Text  = "";
            lblLoginTitle.Text = "";
            lblUsername.Text   = "";
            lblPassword.Text   = "";

            lblCafeTitle.Paint      += (s, e) => DrawOutlinedText(e, lblCafeTitle,      CafeTitleText,  Color.FromArgb(180, 110, 40), Color.FromArgb(50, 15, 0));
            lblLoginTitle.Paint     += (s, e) => DrawOutlinedText(e, lblLoginTitle,     LoginTitleText, Color.White,                  Color.FromArgb(50, 15, 0));
            lblUsername.Paint       += (s, e) => DrawOutlinedText(e, lblUsername,       UsernameText,   Color.White,                  Color.FromArgb(50, 15, 0));
            lblPassword.Paint       += (s, e) => DrawOutlinedText(e, lblPassword,       PasswordText,   Color.White,                  Color.FromArgb(50, 15, 0));
            chkShowPassword.Paint   += (s, e) => DrawOutlinedText(e, chkShowPassword,   chkShowPassword.Text,   Color.White, Color.FromArgb(50, 15, 0));
            lnkForgotPassword.Paint += (s, e) => DrawOutlinedText(e, lnkForgotPassword, lnkForgotPassword.Text, Color.White, Color.FromArgb(50, 15, 0));
            lnkSignUp.Paint         += (s, e) => DrawOutlinedText(e, lnkSignUp,         lnkSignUp.Text,         Color.White, Color.FromArgb(50, 15, 0));
        }

        private static void DrawOutlinedText(System.Windows.Forms.PaintEventArgs e,
            System.Windows.Forms.Control ctrl,
            string text,
            System.Drawing.Color fillColor,
            System.Drawing.Color outlineColor)
        {
            var g = e.Graphics;
            g.SmoothingMode     = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            var font = ctrl.Font;
            var sf   = new System.Drawing.StringFormat
            {
                Alignment     = System.Drawing.StringAlignment.Center,
                LineAlignment = System.Drawing.StringAlignment.Center
            };
            var rect = new System.Drawing.RectangleF(0, 0, ctrl.Width, ctrl.Height);

            using var outlineBrush = new System.Drawing.SolidBrush(outlineColor);
            for (int dx = -2; dx <= 2; dx++)
                for (int dy = -2; dy <= 2; dy++)
                    if (dx != 0 || dy != 0)
                        g.DrawString(text, font, outlineBrush,
                            new System.Drawing.RectangleF(rect.X + dx, rect.Y + dy, rect.Width, rect.Height), sf);

            using var fillBrush = new System.Drawing.SolidBrush(fillColor);
            g.DrawString(text, font, fillBrush, rect, sf);
        }

        private void LoginForm_Load(object? sender, EventArgs e)
        {

            string bgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "login_images.jpg");
            if (File.Exists(bgPath))
            {
                this.BackgroundImage = Image.FromFile(bgPath);
                this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.",
                    "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                const string sql = @"SELECT user_id, full_name, role
                                     FROM users
                                     WHERE username = @u
                                       AND password_hash = SHA2(@p, 256)
                                       AND status = 'active'";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int    userId   = Convert.ToInt32(reader["user_id"]);
                    string fullName = reader["full_name"].ToString() ?? username;
                    string role     = reader["role"].ToString() ?? "staff";

                    this.Hide();
                    var dashboard = new DashboardForm(fullName, role, userId);
                    dashboard.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password, or account is inactive.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection error:\n{ex.Message}\n\nMake sure MySQL is running.",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new PasswordRecoveryForm().ShowDialog();
        }

        private void lnkSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new SignUpForm().ShowDialog();
        }

        private void lnkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }
    }
}
