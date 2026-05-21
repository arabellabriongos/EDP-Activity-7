using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public class UserManagementForm : Form
    {
        // Controls
        private readonly DataGridView dgvUsers   = new DataGridView();
        private readonly TextBox txtSearch       = new TextBox();
        private readonly Button  btnSearch       = new Button();
        private readonly Button  btnAdd          = new Button();
        private readonly Button  btnEdit         = new Button();
        private readonly Button  btnToggleStatus = new Button();
        private readonly Button  btnDelete       = new Button();
        private readonly Button  btnXClose       = new Button();
        private readonly Label   lblTitle        = new Label();
        private readonly Label   lblSearch       = new Label();
        private readonly Label   lblUserTotal    = new Label();
        // White card panel that holds the table + summary
        private readonly Panel pnlCard           = new Panel();
        // Status filter radio buttons
        private readonly RadioButton rdoAll      = new RadioButton();
        private readonly RadioButton rdoActive   = new RadioButton();
        private readonly RadioButton rdoInactive = new RadioButton();

        private static readonly Color Brown   = Color.FromArgb(139, 90, 43);
        private static readonly Color PageBg  = Color.FromArgb(235, 237, 242);
        private static readonly Color CardBg  = Color.White;

        public UserManagementForm()
        {
            BuildUI();
            LoadUsers();
        }

        // UI Construction
        private void BuildUI()
        {
            this.Text            = "User Management";
            this.ClientSize      = new Size(900, 580);
            this.BackColor       = PageBg;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.MinimumSize     = new Size(800, 500);
            this.WindowState     = FormWindowState.Maximized;

            lblTitle.Text      = "User Management";
            lblTitle.Font      = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(74, 53, 37);
            lblTitle.AutoSize  = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Location  = new Point(24, 20);

            lblSearch.Text      = "Search:";
            lblSearch.Font      = new Font("Segoe UI", 10F);
            lblSearch.ForeColor = Color.FromArgb(100, 85, 70);
            lblSearch.AutoSize  = true;
            lblSearch.Location  = new Point(24, 68);

            txtSearch.Font        = new Font("Segoe UI", 10F);
            txtSearch.Location    = new Point(24, 88);
            txtSearch.Size        = new Size(300, 26);
            txtSearch.PlaceholderText = "Search by username, email or name…";
            txtSearch.KeyDown    += (s, e) => { if (e.KeyCode == Keys.Enter) LoadUsers(txtSearch.Text.Trim()); };

            StyleButton(btnSearch, "Search", Brown, Color.White);
            btnSearch.Location = new Point(334, 87);
            btnSearch.Size     = new Size(90, 28);
            btnSearch.Click   += (_, __) => LoadUsers(txtSearch.Text.Trim());

            // Status filter radio buttons — right of Search
            rdoAll.Text      = "All";
            rdoAll.Font      = new Font("Segoe UI", 9.5F);
            rdoAll.Location  = new Point(436, 90);
            rdoAll.AutoSize  = true;
            rdoAll.Checked   = true;
            rdoAll.CheckedChanged += (_, __) => { if (rdoAll.Checked) LoadUsers(txtSearch.Text.Trim()); };

            rdoActive.Text      = "Active";
            rdoActive.Font      = new Font("Segoe UI", 9.5F);
            rdoActive.Location  = new Point(490, 90);
            rdoActive.AutoSize  = true;
            rdoActive.ForeColor = Color.FromArgb(40, 120, 60);
            rdoActive.CheckedChanged += (_, __) => { if (rdoActive.Checked) LoadUsers(txtSearch.Text.Trim()); };

            rdoInactive.Text      = "Inactive";
            rdoInactive.Font      = new Font("Segoe UI", 9.5F);
            rdoInactive.Location  = new Point(560, 90);
            rdoInactive.AutoSize  = true;
            rdoInactive.ForeColor = Color.FromArgb(180, 60, 60);
            rdoInactive.CheckedChanged += (_, __) => { if (rdoInactive.Checked) LoadUsers(txtSearch.Text.Trim()); };

            StyleButton(btnAdd, "+ Add User", Brown, Color.White);
            btnAdd.Location = new Point(0, 0);
            btnAdd.Size     = new Size(120, 34);
            btnAdd.Click   += BtnAdd_Click;

            StyleButton(btnEdit, "✏ Edit", Color.FromArgb(80, 120, 80), Color.White);
            btnEdit.Location = new Point(0, 0);
            btnEdit.Size     = new Size(100, 34);
            btnEdit.Click   += BtnEdit_Click;

            StyleButton(btnToggleStatus, "Change Status", Color.FromArgb(160, 100, 40), Color.White);
            btnToggleStatus.Location = new Point(0, 0);
            btnToggleStatus.Size     = new Size(140, 34);
            btnToggleStatus.Click   += BtnToggleStatus_Click;

            StyleButton(btnDelete, "🗑 Delete", Color.FromArgb(180, 60, 60), Color.White);
            btnDelete.Location = new Point(0, 0);
            btnDelete.Size     = new Size(100, 34);
            btnDelete.Click   += BtnDelete_Click;

            // Close button — top right, same height as action buttons
            StyleButton(btnXClose, "Close", Color.FromArgb(140, 130, 120), Color.White);
            btnXClose.Location = new Point(this.ClientSize.Width - 120, 18);
            btnXClose.Size     = new Size(100, 34);
            btnXClose.Anchor   = AnchorStyles.Top | AnchorStyles.Right;
            btnXClose.Click   += (_, __) => this.Close();

            // White card panel
            pnlCard.BackColor = CardBg;
            pnlCard.Padding   = new Padding(0);

            // DataGridView
            ProductsForm.StyleDgv(dgvUsers);
            dgvUsers.CellFormatting += DgvUsers_CellFormatting;

            // Summary label inside card
            lblUserTotal.Font      = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblUserTotal.ForeColor = Color.FromArgb(74, 53, 37);
            lblUserTotal.BackColor = Color.FromArgb(247, 244, 238);
            lblUserTotal.AutoSize  = false;
            lblUserTotal.TextAlign = ContentAlignment.MiddleLeft;
            lblUserTotal.Padding   = new Padding(14, 0, 0, 0);

            pnlCard.Controls.Add(dgvUsers);
            pnlCard.Controls.Add(lblUserTotal);

            this.Controls.AddRange(new Control[] {
                lblTitle, lblSearch, txtSearch, btnSearch,
                rdoAll, rdoActive, rdoInactive,
                btnAdd, btnEdit, btnToggleStatus, btnDelete, pnlCard, btnXClose
            });

            this.Resize += (_, __) => UpdateLayout();
            this.Load   += (_, __) => UpdateLayout();
        }

        private static void StyleButton(Button b, string text, Color back, Color fore)
        {
            b.Text      = text;
            b.BackColor = back;
            b.ForeColor = fore;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font      = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            b.UseVisualStyleBackColor = false;
        }

        private void UpdateLayout()
        {
            int m = 96; // 1 inch margin
            var host = Parent ?? (Control)this;
            int W = host.ClientSize.Width;
            int H = host.ClientSize.Height;
            int filterTop   = lblTitle.Visible ? 68 : 14;
            int controlsTop = lblTitle.Visible ? 88 : 34;
            int actionTop   = controlsTop + 42;
            lblTitle.Location  = new Point(m, 20);
            btnXClose.Location = new Point(W - m - btnXClose.Width, 20);
            lblSearch.Location = new Point(m, filterTop);
            txtSearch.Location = new Point(m, controlsTop);
            btnSearch.Location = new Point(m + 310, controlsTop);
            rdoAll.Location      = new Point(m + 412, controlsTop + 2);
            rdoActive.Location   = new Point(m + 466, controlsTop + 2);
            rdoInactive.Location = new Point(m + 536, controlsTop + 2);
            btnAdd.Location          = new Point(m, actionTop);
            btnEdit.Location         = new Point(m + 128, actionTop);
            btnToggleStatus.Location = new Point(m + 236, actionTop);
            btnDelete.Location       = new Point(m + 384, actionTop);

            // Card: starts below action row, fills to bottom
            int cardTop = actionTop + 44;
            pnlCard.SetBounds(m, cardTop, W - m * 2, H - cardTop - m);

            // DGV inside card: full width, height = content or card height minus summary
            int summaryH = 70;
            int maxDgvH  = pnlCard.Height - summaryH;
            int dgvH     = ProductsForm.DgvContentHeight(dgvUsers, Math.Max(60, maxDgvH));
            dgvUsers.SetBounds(0, 0, pnlCard.Width, dgvH);

            // Summary directly below dgv, same width as card
            lblUserTotal.SetBounds(0, dgvUsers.Bottom, pnlCard.Width, summaryH);
        }

        // Data Loading
        private void LoadUsers(string search = "")
        {
            dgvUsers.Rows.Clear();
            dgvUsers.Columns.Clear();

            dgvUsers.Columns.Add("user_id",    "ID");
            dgvUsers.Columns.Add("username",   "Username");
            dgvUsers.Columns.Add("full_name",  "Full Name");
            dgvUsers.Columns.Add("email",      "Email");
            dgvUsers.Columns.Add("role",       "Role");
            dgvUsers.Columns.Add("status",     "Status");
            dgvUsers.Columns.Add("created_at", "Created");

            dgvUsers.Columns["user_id"].FillWeight    = 40;
            dgvUsers.Columns["username"].FillWeight   = 100;
            dgvUsers.Columns["full_name"].FillWeight  = 130;
            dgvUsers.Columns["email"].FillWeight      = 160;
            dgvUsers.Columns["role"].FillWeight       = 60;
            dgvUsers.Columns["status"].FillWeight     = 70;
            dgvUsers.Columns["created_at"].FillWeight = 110;

            // Determine status filter
            string statusFilter = rdoActive.Checked   ? "active"
                                : rdoInactive.Checked ? "inactive"
                                : "";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                string sql = @"SELECT user_id, username, full_name, email, role, status,
                                      DATE_FORMAT(created_at,'%b %d, %Y') AS created_at
                               FROM users
                               WHERE (@s = '' OR username LIKE @s OR email LIKE @s OR full_name LIKE @s)
                                 AND (@st = '' OR status = @st)
                               ORDER BY user_id";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@s",  string.IsNullOrEmpty(search) ? "" : $"%{search}%");
                cmd.Parameters.AddWithValue("@st", statusFilter);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dgvUsers.Rows.Add(
                        reader["user_id"],
                        reader["username"],
                        reader["full_name"],
                        reader["email"],
                        reader["role"],
                        reader["status"],
                        reader["created_at"]
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lblUserTotal.Text = $"   Total Users: {dgvUsers.Rows.Count}";
        }

        // Color inactive rows red-tinted
        private void DgvUsers_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvUsers.Rows[e.RowIndex];
            if (row.Cells["status"].Value?.ToString() == "inactive")
            {
                e.CellStyle.ForeColor = Color.FromArgb(180, 80, 80);
            }
        }

        // Get selected user_id
        private int? GetSelectedUserId()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            return Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["user_id"].Value);
        }

        // Add User
        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using var dlg = new UserEditDialog(null);
            if (dlg.ShowDialog() == DialogResult.OK)
                LoadUsers(txtSearch.Text.Trim());
        }

        // Edit User
        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            int? id = GetSelectedUserId();
            if (id == null) return;
            using var dlg = new UserEditDialog(id.Value);
            if (dlg.ShowDialog() == DialogResult.OK)
                LoadUsers(txtSearch.Text.Trim());
        }

        // Toggle status
        private void BtnToggleStatus_Click(object? sender, EventArgs e)
        {
            int? id = GetSelectedUserId();
            if (id == null) return;

            string currentStatus = dgvUsers.SelectedRows[0].Cells["status"].Value?.ToString() ?? "active";
            string newStatus     = currentStatus == "active" ? "inactive" : "active";
            string username      = dgvUsers.SelectedRows[0].Cells["username"].Value?.ToString() ?? "";

            var confirm = MessageBox.Show(
                $"Set account '{username}' to {newStatus}?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd  = new MySqlCommand(
                    "UPDATE users SET status=@s WHERE user_id=@id", conn);
                cmd.Parameters.AddWithValue("@s",  newStatus);
                cmd.Parameters.AddWithValue("@id", id.Value);
                cmd.ExecuteNonQuery();
                LoadUsers(txtSearch.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Delete User
        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            int? id = GetSelectedUserId();
            if (id == null) return;

            string username = dgvUsers.SelectedRows[0].Cells["username"].Value?.ToString() ?? "";

            var confirm = MessageBox.Show(
                $"Are you sure you want to permanently delete account '{username}'?\n\nThis cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd  = new MySqlCommand(
                    "DELETE FROM users WHERE user_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id.Value);
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Account '{username}' has been deleted.", "Deleted",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers(txtSearch.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Add / Edit dialog
    public class UserEditDialog : Form
    {
        private readonly int? _userId;

        private readonly TextBox txtUsername  = new TextBox();
        private readonly TextBox txtFullName  = new TextBox();
        private readonly TextBox txtEmail     = new TextBox();
        private readonly TextBox txtPassword  = new TextBox();
        private readonly ComboBox cmbRole     = new ComboBox();
        private readonly ComboBox cmbStatus   = new ComboBox();
        private readonly Button  btnSave      = new Button();
        private readonly Button  btnCancel    = new Button();

        private static readonly Color Brown = Color.FromArgb(139, 90, 43);

        public UserEditDialog(int? userId)
        {
            _userId = userId;
            BuildUI();
            if (userId.HasValue) LoadUser(userId.Value);
        }

        private void BuildUI()
        {
            this.Text            = _userId.HasValue ? "Edit User" : "Add User";
            this.ClientSize      = new Size(420, 380);
            this.BackColor       = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterParent;

            int lx = 24, fx = 24, fw = 372, ly = 20, gy = 28;

            void AddRow(string label, Control ctrl, ref int y)
            {
                var lbl = new Label
                {
                    Text      = label,
                    Font      = new Font("Segoe UI", 9.5F),
                    ForeColor = Color.FromArgb(80, 65, 50),
                    AutoSize  = true,
                    Location  = new Point(lx, y)
                };
                ctrl.Location = new Point(fx, y + gy - 4);
                ctrl.Width    = fw;
                ctrl.Font     = new Font("Segoe UI", 10F);
                this.Controls.Add(lbl);
                this.Controls.Add(ctrl);
                y += gy + 30;
            }

            int y = 20;
            AddRow("Username",  txtUsername, ref y);
            AddRow("Full Name", txtFullName, ref y);
            AddRow("Email",     txtEmail,    ref y);

            txtPassword.PasswordChar = '*';
            AddRow(_userId.HasValue ? "New Password (leave blank to keep)" : "Password", txtPassword, ref y);

            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Items.AddRange(new object[] { "admin", "staff" });
            cmbRole.SelectedIndex = 1;
            AddRow("Role", cmbRole, ref y);

            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new object[] { "active", "inactive" });
            cmbStatus.SelectedIndex = 0;
            AddRow("Status", cmbStatus, ref y);

            btnSave.Text      = "Save";
            btnSave.BackColor = Brown;
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.Size      = new Size(120, 36);
            btnSave.Location  = new Point(fx, y + 6);
            btnSave.Click    += BtnSave_Click;

            btnCancel.Text      = "Cancel";
            btnCancel.BackColor = Color.FromArgb(140, 130, 120);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Font      = new Font("Segoe UI", 10F);
            btnCancel.Size      = new Size(100, 36);
            btnCancel.Location  = new Point(fx + 130, y + 6);
            btnCancel.Click    += (_, __) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.ClientSize = new Size(420, y + 60);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void LoadUser(int userId)
        {
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd  = new MySqlCommand(
                    "SELECT username,full_name,email,role,status FROM users WHERE user_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", userId);
                using var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    txtUsername.Text       = r["username"].ToString();
                    txtFullName.Text       = r["full_name"].ToString();
                    txtEmail.Text          = r["email"].ToString();
                    cmbRole.SelectedItem   = r["role"].ToString();
                    cmbStatus.SelectedItem = r["status"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string username  = txtUsername.Text.Trim();
            string fullName  = txtFullName.Text.Trim();
            string email     = txtEmail.Text.Trim();
            string password  = txtPassword.Text;
            string role      = cmbRole.SelectedItem?.ToString() ?? "staff";
            string status    = cmbStatus.SelectedItem?.ToString() ?? "active";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Username, Full Name and Email are required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Enter a valid email address.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!_userId.HasValue && string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password is required for new accounts.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!string.IsNullOrEmpty(password) && password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                if (_userId.HasValue)
                {
                    // Update
                    string sql = string.IsNullOrEmpty(password)
                        ? "UPDATE users SET username=@u,full_name=@fn,email=@em,role=@r,status=@s WHERE user_id=@id"
                        : "UPDATE users SET username=@u,full_name=@fn,email=@em,role=@r,status=@s,password_hash=SHA2(@p,256) WHERE user_id=@id";
                    using var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u",  username);
                    cmd.Parameters.AddWithValue("@fn", fullName);
                    cmd.Parameters.AddWithValue("@em", email);
                    cmd.Parameters.AddWithValue("@r",  role);
                    cmd.Parameters.AddWithValue("@s",  status);
                    cmd.Parameters.AddWithValue("@id", _userId.Value);
                    if (!string.IsNullOrEmpty(password))
                        cmd.Parameters.AddWithValue("@p", password);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User updated successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Insert
                    using var cmd = new MySqlCommand(
                        "INSERT INTO users (username,full_name,email,password_hash,role,status) VALUES (@u,@fn,@em,SHA2(@p,256),@r,@s)",
                        conn);
                    cmd.Parameters.AddWithValue("@u",  username);
                    cmd.Parameters.AddWithValue("@fn", fullName);
                    cmd.Parameters.AddWithValue("@em", email);
                    cmd.Parameters.AddWithValue("@p",  password);
                    cmd.Parameters.AddWithValue("@r",  role);
                    cmd.Parameters.AddWithValue("@s",  status);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User added successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Username or email already exists.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


