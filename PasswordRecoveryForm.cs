using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public partial class PasswordRecoveryForm : Form
    {
        private string _recoveryEmail = "";

        public PasswordRecoveryForm()
        {
            InitializeComponent();
        }

        private void btnSendCode_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email address.", "Email Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if email exists in DB
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd  = new MySqlCommand(
                    "SELECT COUNT(*) FROM users WHERE email=@em AND status='active'", conn);
                cmd.Parameters.AddWithValue("@em", email);
                long count = (long)(cmd.ExecuteScalar() ?? 0L);

                if (count == 0)
                {
                    MessageBox.Show("No active account found with that email address.",
                        "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _recoveryEmail = email;
            MessageBox.Show($"A verification code has been sent to {email}.\n\nDemo: use code 123456",
                "Code Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            panelMain.Visible = false;
            panelCode.Visible = true;
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Please enter the verification code.", "Code Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Demo: accept "123456" as valid code
            if (code != "123456")
            {
                MessageBox.Show("Invalid verification code. Demo: use 123456",
                    "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Prompt for new password
            using var resetDlg = new ResetPasswordDialog(_recoveryEmail);
            if (resetDlg.ShowDialog() == DialogResult.OK)
            {
                // Auto-login — open Dashboard directly
                this.Hide();
                var dashboard = new DashboardForm(resetDlg.LoggedInFullName, resetDlg.LoggedInRole, resetDlg.LoggedInUserId);
                dashboard.ShowDialog();
                this.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e) => this.Close();
    }

    //Inline reset-password dialog 
    public class ResetPasswordDialog : Form
    {
        private readonly string _email;
        private readonly TextBox txtNew     = new TextBox();
        private readonly TextBox txtConfirm = new TextBox();
        private readonly Button  btnOk      = new Button();
        private readonly Button  btnCancel  = new Button();

        // Exposed after successful reset for auto-login
        public string LoggedInFullName { get; private set; } = "";
        public string LoggedInRole     { get; private set; } = "staff";
        public int    LoggedInUserId   { get; private set; } = 0;

        public ResetPasswordDialog(string email)
        {
            _email = email;
            this.Text            = "Reset Password";
            this.ClientSize      = new System.Drawing.Size(380, 200);
            this.BackColor       = System.Drawing.Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterParent;

            var font = new System.Drawing.Font("Segoe UI", 10F);
            var brown = System.Drawing.Color.FromArgb(139, 90, 43);

            AddLabel("New Password:", 24, 20, font);
            txtNew.PasswordChar = '*';
            txtNew.Font = font;
            txtNew.SetBounds(24, 44, 332, 26);

            AddLabel("Confirm Password:", 24, 82, font);
            txtConfirm.PasswordChar = '*';
            txtConfirm.Font = font;
            txtConfirm.SetBounds(24, 106, 332, 26);

            btnOk.Text = "Reset"; btnOk.Font = font;
            btnOk.BackColor = brown; btnOk.ForeColor = System.Drawing.Color.White;
            btnOk.FlatStyle = FlatStyle.Flat; btnOk.FlatAppearance.BorderSize = 0;
            btnOk.SetBounds(24, 148, 120, 34);
            btnOk.Click += BtnOk_Click;

            btnCancel.Text = "Cancel"; btnCancel.Font = font;
            btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 130, 120);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat; btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.SetBounds(154, 148, 100, 34);
            btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

            this.Controls.AddRange(new Control[] { txtNew, txtConfirm, btnOk, btnCancel });
        }

        private void AddLabel(string text, int x, int y, System.Drawing.Font font)
        {
            var lbl = new Label { Text = text, Font = font, AutoSize = true,
                Location = new System.Drawing.Point(x, y),
                ForeColor = System.Drawing.Color.FromArgb(80, 65, 50) };
            this.Controls.Add(lbl);
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            if (txtNew.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtNew.Text != txtConfirm.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using var conn = DatabaseConnection.GetConnection();

                // Update password
                using (var cmd = new MySqlCommand(
                    "UPDATE users SET password_hash=SHA2(@p,256) WHERE email=@em", conn))
                {
                    cmd.Parameters.AddWithValue("@p",  txtNew.Text);
                    cmd.Parameters.AddWithValue("@em", _email);
                    cmd.ExecuteNonQuery();
                }

                // Fetch full_name, role, and user_id for auto-login
                using (var cmd = new MySqlCommand(
                    "SELECT user_id, full_name, role FROM users WHERE email=@em", conn))
                {
                    cmd.Parameters.AddWithValue("@em", _email);
                    using var r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        LoggedInUserId   = Convert.ToInt32(r["user_id"]);
                        LoggedInFullName = r["full_name"].ToString() ?? _email;
                        LoggedInRole     = r["role"].ToString() ?? "staff";
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
