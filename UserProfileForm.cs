using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public class UserProfileForm : Form
    {
        private readonly int    _userId;
        private readonly string _role;

        public string UpdatedFullName { get; private set; } = "";

        // Controls
        private readonly Label    lblTitle       = new Label();
        private readonly Label    lblSubtitle    = new Label();
        private Label             lblFullNameLbl = new Label();
        private readonly TextBox  txtFullName    = new TextBox();
        private Label             lblEmailLbl    = new Label();
        private readonly TextBox  txtEmail       = new TextBox();
        private Label             lblUsernameLbl = new Label();
        private readonly TextBox  txtUsername    = new TextBox();
        private Label             lblRoleLbl     = new Label();
        private readonly TextBox  txtRole        = new TextBox();
        private readonly Label    lblSepPwd      = new Label();
        private Label             lblCurrentPwd  = new Label();
        private readonly TextBox  txtCurrentPwd  = new TextBox();
        private Label             lblNewPwd      = new Label();
        private readonly TextBox  txtNewPwd      = new TextBox();
        private Label             lblConfirmPwd  = new Label();
        private readonly TextBox  txtConfirmPwd  = new TextBox();
        private readonly Button   btnSave        = new Button();
        private readonly Button   btnCancel      = new Button();

        private static readonly Color Brown  = Color.FromArgb(139, 90, 43);
        private static readonly Color BgCard = Color.FromArgb(247, 244, 238);

        public UserProfileForm(int userId, string role)
        {
            _userId = userId;
            _role   = role;
            BuildUI();
            LoadProfile();
        }

        private void BuildUI()
        {
            this.Text            = "My Profile";
            this.ClientSize      = new Size(480, 580);
            this.BackColor       = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterParent;

            int lx = 30, fw = 420, y = 20;

            // Header
            lblTitle.Text      = "My Profile";
            lblTitle.Font      = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(74, 53, 37);
            lblTitle.AutoSize  = true;
            lblTitle.Location  = new Point(lx, y);
            y += 36;

            lblSubtitle.Text      = "Update your personal information and password.";
            lblSubtitle.Font      = new Font("Segoe UI", 9.5F);
            lblSubtitle.ForeColor = Color.FromArgb(120, 100, 80);
            lblSubtitle.AutoSize  = true;
            lblSubtitle.Location  = new Point(lx, y);
            y += 30;

            // Profile info
            y += 6;
            AddField("Full Name",  lblFullNameLbl, txtFullName,  lx, fw, ref y);
            AddField("Email",      lblEmailLbl,    txtEmail,     lx, fw, ref y);
            AddField("Username",   lblUsernameLbl, txtUsername,  lx, fw, ref y);
            AddField("Role",       lblRoleLbl,     txtRole,      lx, fw, ref y);
            txtRole.ReadOnly  = true;
            txtRole.BackColor = Color.FromArgb(240, 238, 234);
            txtRole.ForeColor = Color.FromArgb(120, 100, 80);

            // Password section
            y += 8;
            lblSepPwd.Text      = "─── Change Password (leave blank to keep current) ───";
            lblSepPwd.Font      = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblSepPwd.ForeColor = Color.FromArgb(150, 130, 110);
            lblSepPwd.AutoSize  = true;
            lblSepPwd.Location  = new Point(lx, y);
            y += 22;

            AddPasswordField("Current Password", lblCurrentPwd, txtCurrentPwd, lx, fw, ref y);
            AddPasswordField("New Password",     lblNewPwd,     txtNewPwd,     lx, fw, ref y);
            AddPasswordField("Confirm Password", lblConfirmPwd, txtConfirmPwd, lx, fw, ref y);

            // Buttons
            y += 10;
            btnSave.Text      = "Save Changes";
            btnSave.BackColor = Brown;
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.Size      = new Size(160, 38);
            btnSave.Location  = new Point(lx, y);
            btnSave.Click    += BtnSave_Click;

            btnCancel.Text      = "Cancel";
            btnCancel.BackColor = Color.FromArgb(140, 130, 120);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Font      = new Font("Segoe UI", 10F);
            btnCancel.Size      = new Size(110, 38);
            btnCancel.Location  = new Point(lx + 170, y);
            btnCancel.Click    += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

            this.ClientSize = new Size(480, y + 58);

            this.Controls.AddRange(new Control[] {
                lblTitle, lblSubtitle,
                lblFullNameLbl, txtFullName,
                lblEmailLbl,    txtEmail,
                lblUsernameLbl, txtUsername,
                lblRoleLbl,     txtRole,
                lblSepPwd,
                lblCurrentPwd,  txtCurrentPwd,
                lblNewPwd,      txtNewPwd,
                lblConfirmPwd,  txtConfirmPwd,
                btnSave, btnCancel
            });
        }

        private void AddField(string label, Label lbl, TextBox txt, int x, int w, ref int y)
        {
            lbl.Text      = label;
            lbl.Font      = new Font("Segoe UI", 9.5F);
            lbl.ForeColor = Color.FromArgb(80, 65, 50);
            lbl.AutoSize  = true;
            lbl.Location  = new Point(x, y);
            y += 20;
            txt.Font     = new Font("Segoe UI", 10.5F);
            txt.Location = new Point(x, y);
            txt.Size     = new Size(w, 28);
            y += 38;
        }

        private void AddPasswordField(string label, Label lbl, TextBox txt, int x, int w, ref int y)
        {
            AddField(label, lbl, txt, x, w, ref y);
            txt.PasswordChar = '*';
        }

        // Load profile
        private void LoadProfile()
        {
            if (_userId == 0)
            {
                txtRole.Text = _role;
                return;
            }
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd  = new MySqlCommand(
                    "SELECT username, full_name, email, role FROM users WHERE user_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", _userId);
                using var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    txtUsername.Text = r["username"].ToString();
                    txtFullName.Text = r["full_name"].ToString();
                    txtEmail.Text    = r["email"].ToString();
                    txtRole.Text     = r["role"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading profile:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Save
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email    = txtEmail.Text.Trim();
            string username = txtUsername.Text.Trim();
            string curPwd   = txtCurrentPwd.Text;
            string newPwd   = txtNewPwd.Text;
            string confPwd  = txtConfirmPwd.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Full Name, Email and Username are required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Enter a valid email address.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool changingPassword = !string.IsNullOrEmpty(newPwd) || !string.IsNullOrEmpty(curPwd);
            if (changingPassword)
            {
                if (string.IsNullOrEmpty(curPwd))
                {
                    MessageBox.Show("Enter your current password to set a new one.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (newPwd.Length < 6)
                {
                    MessageBox.Show("New password must be at least 6 characters.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (newPwd != confPwd)
                {
                    MessageBox.Show("New passwords do not match.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                using var conn = DatabaseConnection.GetConnection();

                // Verify current password if changing
                if (changingPassword)
                {
                    using var chk = new MySqlCommand(
                        "SELECT COUNT(*) FROM users WHERE user_id=@id AND password_hash=SHA2(@p,256)", conn);
                    chk.Parameters.AddWithValue("@id", _userId);
                    chk.Parameters.AddWithValue("@p",  curPwd);
                    long match = (long)(chk.ExecuteScalar() ?? 0L);
                    if (match == 0)
                    {
                        MessageBox.Show("Current password is incorrect.", "Wrong Password",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Build UPDATE
                string sql = changingPassword
                    ? "UPDATE users SET full_name=@fn, email=@em, username=@u, password_hash=SHA2(@np,256) WHERE user_id=@id"
                    : "UPDATE users SET full_name=@fn, email=@em, username=@u WHERE user_id=@id";

                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@fn", fullName);
                cmd.Parameters.AddWithValue("@em", email);
                cmd.Parameters.AddWithValue("@u",  username);
                cmd.Parameters.AddWithValue("@id", _userId);
                if (changingPassword)
                    cmd.Parameters.AddWithValue("@np", newPwd);

                cmd.ExecuteNonQuery();

                UpdatedFullName = fullName;
                MessageBox.Show("Profile updated successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Username or email already in use by another account.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving profile:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
