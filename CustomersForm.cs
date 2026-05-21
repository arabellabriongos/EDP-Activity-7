using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public class CustomersForm : Form
    {
        private readonly DataGridView dgv  = new DataGridView();
        private readonly TextBox txtSearch = new TextBox();
        private readonly Button btnSearch  = new Button();
        private readonly Button btnAdd     = new Button();
        private readonly Button btnEdit    = new Button();
        private readonly Button btnDelete  = new Button();
        private readonly Button btnXClose  = new Button();
        private readonly Label lblTitle    = new Label();
        private readonly Label lblTotal    = new Label();
        private readonly Panel pnlCard       = new Panel();

        private string? _selectedCustomerId = null;

        private static readonly Color Brown   = Color.FromArgb(139, 90, 43);
        private static readonly Color PageBg  = Color.FromArgb(235, 237, 242);
        private static readonly Color CardBg  = Color.White;

        public CustomersForm() { BuildUI(); LoadCustomers(); }

        private void BuildUI()
        {
            Text = "Customers"; BackColor = PageBg;
            ClientSize = new Size(760, 520); StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable; MinimumSize = new Size(600, 420);
            this.WindowState = FormWindowState.Maximized;

            lblTitle.Text = "Customers"; lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(74, 53, 37); lblTitle.AutoSize = true; lblTitle.BackColor = Color.Transparent; lblTitle.Location = new Point(24, 18);

            MkBtn(btnXClose, "Close", Color.FromArgb(140, 130, 120), new Point(ClientSize.Width - 120, 18), new Size(100, 34));
            btnXClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXClose.Click += (_, __) => Close();

            AddLbl("Search:", new Point(24, 66));
            txtSearch.Font = new Font("Segoe UI", 10F); txtSearch.Location = new Point(24, 86);
            txtSearch.Size = new Size(240, 26); txtSearch.PlaceholderText = "Name or ID…";
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadCustomers(); };

            MkBtn(btnSearch, "🔍 Search", Brown,                       new Point(0, 0), new Size(110, 34));
            btnSearch.Click += (_, __) => LoadCustomers();

            MkBtn(btnAdd,    "+ Add",      Brown,                       new Point(0, 0), new Size(100, 34));
            btnAdd.Click += (_, __) => { new CustomerAddDialog().ShowDialog(); LoadCustomers(); };

            MkBtn(btnEdit,   "✏ Edit",    Color.FromArgb(80, 120, 80), new Point(0, 0), new Size(100, 34));
            btnEdit.Click += (_, __) => {
                if (_selectedCustomerId == null) { MessageBox.Show("Select a customer to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                new CustomerEditDialog(_selectedCustomerId).ShowDialog(); LoadCustomers();
            };

            MkBtn(btnDelete, "🗑 Delete",  Color.FromArgb(180, 60, 60), new Point(0, 0), new Size(100, 34));
            btnDelete.Click += BtnDelete_Click;

            pnlCard.BackColor = CardBg;
            pnlCard.Padding   = new Padding(0);

            lblTotal.Font      = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(74, 53, 37);
            lblTotal.BackColor = Color.FromArgb(247, 244, 238);
            lblTotal.AutoSize  = false;
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            lblTotal.Padding   = new Padding(14, 0, 0, 0);

            ProductsForm.StyleDgv(dgv);
            dgv.SelectionChanged += (_, __) => {
                if (dgv.SelectedRows.Count > 0)
                    _selectedCustomerId = dgv.SelectedRows[0].Cells["customer_id"].Value?.ToString();
                else
                    _selectedCustomerId = null;
            };

            pnlCard.Controls.Add(dgv);
            pnlCard.Controls.Add(lblTotal);

            Controls.AddRange(new Control[] { lblTitle, btnXClose, txtSearch, btnSearch, btnAdd, btnEdit, btnDelete, pnlCard });

            Resize += (_, __) => UpdateLayout();
            Load    += (_, __) => UpdateLayout();
        }

        private void UpdateLayout()
        {
            var host = Parent ?? (Control)this;
            int W = host.ClientSize.Width;
            int H = host.ClientSize.Height;
            int m = 96; // 1 inch margin

            lblTitle.Location  = new Point(m, 18);
            btnXClose.Location = new Point(W - m - btnXClose.Width, 18);

            int filterTop   = lblTitle.Visible ? 66 : 14;
            int controlsTop = lblTitle.Visible ? 86 : 34;

            foreach (Control c in Controls)
                if (c is Label l && l.AutoSize && (l.Location.Y == 66 || l.Location.Y == 14))
                    l.Location = new Point(m, filterTop);

            txtSearch.Location = new Point(m, controlsTop);
            btnSearch.Location = new Point(m + 252, controlsTop);
            btnAdd.Location    = new Point(m + 372, controlsTop);
            btnEdit.Location   = new Point(m + 482, controlsTop);
            btnDelete.Location = new Point(m + 592, controlsTop);

            // Card: starts below filter row, fills to bottom
            int cardTop = controlsTop + 44;
            pnlCard.SetBounds(m, cardTop, W - m * 2, H - cardTop - m);

            // DGV inside card: full width, height = content or card height minus summary
            int summaryH = 70;
            int maxDgvH  = pnlCard.Height - summaryH;
            int dgvH     = ProductsForm.DgvContentHeight(dgv, Math.Max(60, maxDgvH));
            dgv.SetBounds(0, 0, pnlCard.Width, dgvH);

            // Summary directly below dgv, same width as card
            lblTotal.SetBounds(0, dgv.Bottom, pnlCard.Width, summaryH);
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedCustomerId == null) {
                MessageBox.Show("Select a customer to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (MessageBox.Show($"Delete customer {_selectedCustomerId}?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand("DELETE FROM customer WHERE customer_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", _selectedCustomerId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Customer deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _selectedCustomerId = null;
                LoadCustomers();
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomers()
        {
            dgv.Rows.Clear(); dgv.Columns.Clear();
            dgv.Columns.Add("customer_id", "Customer ID");
            dgv.Columns.Add("fname",       "First Name");
            dgv.Columns.Add("lname",       "Last Name");
            dgv.Columns["customer_id"].FillWeight = 90;
            dgv.Columns["fname"].FillWeight       = 160;
            dgv.Columns["lname"].FillWeight       = 160;

            string s = txtSearch.Text.Trim();
            try {
                using var conn = DatabaseConnection.GetConnection();
                const string sql = @"SELECT customer_id, fname, lname FROM customer
                                     WHERE @s='' OR customer_id LIKE @s
                                        OR fname LIKE @s OR lname LIKE @s
                                        OR CONCAT(fname,' ',lname) LIKE @s
                                     ORDER BY lname, fname";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@s", string.IsNullOrEmpty(s) ? "" : $"%{s}%");
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    dgv.Rows.Add(r["customer_id"], r["fname"], r["lname"]);
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lblTotal.Text = $"   Total Customers: {dgv.Rows.Count}";
            UpdateLayout();
        }

        private void AddLbl(string t, Point p) =>
            Controls.Add(new Label { Text = t, Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(80,65,50), AutoSize = true, Location = p });

        private static void MkBtn(Button b, string t, Color back, Point loc, Size sz) {
            b.Text = t; b.BackColor = back; b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            b.Location = loc; b.Size = sz; b.UseVisualStyleBackColor = false;
        }
    }

    public class CustomerAddDialog : Form
    {
        private readonly TextBox txtId    = new TextBox();
        private readonly TextBox txtFname = new TextBox();
        private readonly TextBox txtLname = new TextBox();
        private readonly Button btnSave   = new Button();
        private readonly Button btnCancel = new Button();
        private static readonly Color Brown = Color.FromArgb(139, 90, 43);

        public CustomerAddDialog()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Add Customer"; BackColor = Color.White;
            ClientSize = new Size(380, 260);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            int lx = 24, fx = 24, fw = 332, y = 20, gy = 28;

            void AddRow(string label, Control ctrl, ref int cy)
            {
                Controls.Add(new Label {
                    Text = label, Font = new Font("Segoe UI", 9.5F),
                    ForeColor = Color.FromArgb(80, 65, 50), AutoSize = true,
                    Location = new Point(lx, cy)
                });
                ctrl.Location = new Point(fx, cy + gy - 4);
                ctrl.Width = fw;
                ctrl.Font = new Font("Segoe UI", 10F);
                Controls.Add(ctrl);
                cy += gy + 30;
            }

            AddRow("Customer ID (e.g. CST001)", txtId, ref y);
            AddRow("First Name", txtFname, ref y);
            AddRow("Last Name", txtLname, ref y);

            btnSave.Text = "Save"; btnSave.BackColor = Brown; btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat; btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.Size = new Size(120, 36); btnSave.Location = new Point(fx, y + 6);
            btnSave.Click += BtnSave_Click;

            btnCancel.Text = "Cancel"; btnCancel.BackColor = Color.FromArgb(140, 130, 120);
            btnCancel.ForeColor = Color.White; btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.Size = new Size(100, 36); btnCancel.Location = new Point(fx + 130, y + 6);
            btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

            ClientSize = new Size(380, y + 60);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string id    = txtId.Text.Trim();
            string fname = txtFname.Text.Trim();
            string lname = txtLname.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname)) {
                MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            try {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(
                    "INSERT INTO customer (customer_id, fname, lname) VALUES (@id, @f, @l)", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@f",  fname);
                cmd.Parameters.AddWithValue("@l",  lname);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Customer added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            } catch (MySqlException ex) when (ex.Number == 1062) {
                MessageBox.Show("A customer with that ID already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class CustomerEditDialog : Form
    {
        private readonly string _customerId;
        private readonly TextBox txtId    = new TextBox();
        private readonly TextBox txtFname = new TextBox();
        private readonly TextBox txtLname = new TextBox();
        private readonly Button btnSave   = new Button();
        private readonly Button btnCancel = new Button();
        private static readonly Color Brown = Color.FromArgb(139, 90, 43);

        public CustomerEditDialog(string customerId)
        {
            _customerId = customerId;
            BuildUI();
            LoadCustomer(customerId);
        }

        private void BuildUI()
        {
            Text = "Edit Customer"; BackColor = Color.White;
            ClientSize = new Size(380, 260);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            int lx = 24, fx = 24, fw = 332, y = 20, gy = 28;

            void AddRow(string label, Control ctrl, ref int cy)
            {
                Controls.Add(new Label {
                    Text = label, Font = new Font("Segoe UI", 9.5F),
                    ForeColor = Color.FromArgb(80, 65, 50), AutoSize = true,
                    Location = new Point(lx, cy)
                });
                ctrl.Location = new Point(fx, cy + gy - 4);
                ctrl.Width = fw;
                ctrl.Font = new Font("Segoe UI", 10F);
                Controls.Add(ctrl);
                cy += gy + 30;
            }

            txtId.ReadOnly = true;
            txtId.BackColor = Color.FromArgb(240, 240, 240);
            AddRow("Customer ID", txtId, ref y);
            AddRow("First Name", txtFname, ref y);
            AddRow("Last Name", txtLname, ref y);

            btnSave.Text = "Save"; btnSave.BackColor = Brown; btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat; btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.Size = new Size(120, 36); btnSave.Location = new Point(fx, y + 6);
            btnSave.Click += BtnSave_Click;

            btnCancel.Text = "Cancel"; btnCancel.BackColor = Color.FromArgb(140, 130, 120);
            btnCancel.ForeColor = Color.White; btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.Size = new Size(100, 36); btnCancel.Location = new Point(fx + 130, y + 6);
            btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

            ClientSize = new Size(380, y + 60);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
        }

        private void LoadCustomer(string customerId)
        {
            try {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand("SELECT customer_id, fname, lname FROM customer WHERE customer_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", customerId);
                using var r = cmd.ExecuteReader();
                if (r.Read()) {
                    txtId.Text = r["customer_id"].ToString();
                    txtFname.Text = r["fname"].ToString();
                    txtLname.Text = r["lname"].ToString();
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error loading customer:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string fname = txtFname.Text.Trim();
            string lname = txtLname.Text.Trim();

            if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname)) {
                MessageBox.Show("First Name and Last Name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            try {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(
                    "UPDATE customer SET fname=@f, lname=@l WHERE customer_id=@id", conn);
                cmd.Parameters.AddWithValue("@f", fname);
                cmd.Parameters.AddWithValue("@l", lname);
                cmd.Parameters.AddWithValue("@id", _customerId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Customer updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


