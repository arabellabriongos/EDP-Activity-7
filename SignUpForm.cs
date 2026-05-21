using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar        = chkShowPassword.Checked ? '\0' : '*';
            txtConfirmPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string email           = txtEmail.Text.Trim();
            string username        = txtUsername.Text.Trim();
            string fullName        = txtFullName.Text.Trim();
            string password        = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Sign Up Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Weak Password",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Password Mismatch",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                const string sql = @"INSERT INTO users (username, email, password_hash, full_name, role, status)
                                     VALUES (@u, @em, SHA2(@p, 256), @fn, 'staff', 'active')";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u",  username);
                cmd.Parameters.AddWithValue("@em", email);
                cmd.Parameters.AddWithValue("@p",  password);
                cmd.Parameters.AddWithValue("@fn", fullName);

                cmd.ExecuteNonQuery();
                long newUserId = cmd.LastInsertedId;

                // Auto-login — open Dashboard directly
                this.Hide();
                var dashboard = new DashboardForm(fullName, "staff", (int)newUserId);
                dashboard.ShowDialog();
                this.Close();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Username or email already exists. Please choose another.",
                    "Duplicate Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();

        private void lblLoginLink_Click(object sender, EventArgs e) => this.Close();
    }
}
