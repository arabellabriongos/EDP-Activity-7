using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public class InventoryForm : Form
    {
        private readonly DataGridView dgv  = new DataGridView();
        private readonly ComboBox cmbCat   = new ComboBox();
        private readonly Button btnEdit    = new Button();
        private readonly Button btnXClose  = new Button();
        private readonly Label lblTitle    = new Label();
        private readonly Label lblSummary  = new Label();
        // White card panel that holds the table + summary
        private readonly Panel pnlCard       = new Panel();

        private string? _selectedProductId = null;

        private static readonly Color Brown   = Color.FromArgb(139, 90, 43);
        private static readonly Color PageBg  = Color.FromArgb(235, 237, 242);
        private static readonly Color CardBg  = Color.White;

        public InventoryForm() { BuildUI(); LoadCategories(); LoadInventory(); }

        private void BuildUI()
        {
            Text = "Inventory"; BackColor = PageBg;
            ClientSize = new Size(800, 540); StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable; MinimumSize = new Size(650, 440);
            this.WindowState = FormWindowState.Maximized;

            lblTitle.Text = "Inventory"; lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(74, 53, 37); lblTitle.AutoSize = true; lblTitle.BackColor = Color.Transparent; lblTitle.Location = new Point(24, 18);

            // X Close button — top right
            MkBtn(btnXClose, "Close", Color.FromArgb(140, 130, 120), new Point(ClientSize.Width - 120, 18), new Size(100, 34));
            btnXClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXClose.Click += (_, __) => Close();

            AddLbl("Filter by Category:", new Point(24, 66));
            cmbCat.Font = new Font("Segoe UI", 10F); cmbCat.Location = new Point(24, 86);
            cmbCat.Size = new Size(200, 26); cmbCat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCat.SelectedIndexChanged += (_, __) => LoadInventory();

            MkBtn(btnEdit, "✏ Edit Stock", Color.FromArgb(80, 120, 80), new Point(0, 0), new Size(120, 34));
            btnEdit.Click += BtnEdit_Click;

            // White card panel
            pnlCard.BackColor = CardBg;
            pnlCard.Padding   = new Padding(0);

            // Summary label inside card
            lblSummary.Font      = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(74, 53, 37);
            lblSummary.BackColor = Color.FromArgb(247, 244, 238);
            lblSummary.AutoSize  = false;
            lblSummary.TextAlign = ContentAlignment.MiddleLeft;
            lblSummary.Padding   = new Padding(14, 0, 0, 0);

            ProductsForm.StyleDgv(dgv);
            dgv.SelectionChanged += (_, __) => {
                if (dgv.SelectedRows.Count > 0)
                    _selectedProductId = dgv.SelectedRows[0].Cells["product_id"].Value?.ToString();
                else
                    _selectedProductId = null;
            };

            pnlCard.Controls.Add(dgv);
            pnlCard.Controls.Add(lblSummary);

            Controls.AddRange(new Control[] { lblTitle, btnXClose, cmbCat, btnEdit, pnlCard });

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

            cmbCat.Location  = new Point(m, controlsTop);
            btnEdit.Location = new Point(m + 212, controlsTop);

            // Card: starts below filter row, fills to bottom
            int cardTop = controlsTop + 44;
            pnlCard.SetBounds(m, cardTop, W - m * 2, H - cardTop - m);

            // DGV inside card: full width, height = content or card height minus summary
            int summaryH = 70;
            int maxDgvH  = pnlCard.Height - summaryH;
            int dgvH     = ProductsForm.DgvContentHeight(dgv, Math.Max(60, maxDgvH));
            dgv.SetBounds(0, 0, pnlCard.Width, dgvH);

            // Summary directly below dgv, same width as card
            lblSummary.SetBounds(0, dgv.Bottom, pnlCard.Width, summaryH);
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedProductId == null) {
                MessageBox.Show("Select a product to edit stock.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            string productName = dgv.SelectedRows[0].Cells["product_name"].Value?.ToString() ?? _selectedProductId;
            string currentStock = dgv.SelectedRows[0].Cells["stock"].Value?.ToString() ?? "0";

            using var dlg = new StockEditDialog(_selectedProductId, productName, currentStock);
            if (dlg.ShowDialog() == DialogResult.OK) LoadInventory();
        }

        private void LoadCategories()
        {
            cmbCat.Items.Clear(); cmbCat.Items.Add("All Categories");
            try {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand("SELECT category_name FROM category ORDER BY category_name", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read()) cmbCat.Items.Add(r["category_name"].ToString());
            } catch { }
            cmbCat.SelectedIndex = 0;
        }

        private void LoadInventory()
        {
            dgv.Rows.Clear(); dgv.Columns.Clear();
            dgv.Columns.Add("product_id",   "Product ID");
            dgv.Columns.Add("product_name", "Product Name");
            dgv.Columns.Add("category",     "Category");
            dgv.Columns.Add("stock",        "Stock");
            dgv.Columns.Add("status",       "Status");
            dgv.Columns["product_id"].FillWeight   = 80;
            dgv.Columns["product_name"].FillWeight = 200;
            dgv.Columns["category"].FillWeight     = 130;
            dgv.Columns["stock"].FillWeight        = 70;
            dgv.Columns["status"].FillWeight       = 100;

            string cat = cmbCat.SelectedIndex > 0 ? cmbCat.SelectedItem!.ToString()! : "";
            int lowCount = 0, outCount = 0;
            try {
                using var conn = DatabaseConnection.GetConnection();
                const string sql = @"SELECT p.product_id, p.product_name, c.category_name, p.stock,
                                            CASE WHEN p.stock = 0   THEN 'Out of Stock'
                                                 WHEN p.stock <= 10 THEN 'Low Stock'
                                                 ELSE 'In Stock' END AS status
                                     FROM product p JOIN category c ON p.category_id=c.category_id
                                     WHERE @c='' OR c.category_name=@c
                                     ORDER BY p.stock ASC, c.category_name, p.product_name";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@c", cat);
                using var r = cmd.ExecuteReader();
                while (r.Read()) {
                    string status = r["status"].ToString()!;
                    int rowIdx = dgv.Rows.Add(r["product_id"], r["product_name"],
                                              r["category_name"], r["stock"], status);
                    if (status == "Out of Stock") {
                        dgv.Rows[rowIdx].DefaultCellStyle.ForeColor = Color.FromArgb(180, 50, 50);
                        outCount++;
                    } else if (status == "Low Stock") {
                        dgv.Rows[rowIdx].DefaultCellStyle.ForeColor = Color.FromArgb(180, 100, 0);
                        lowCount++;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lblSummary.Text = $"   Total: {dgv.Rows.Count}\n   Low Stock: {lowCount}\n   Out of Stock: {outCount}";
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

    // ══════════════════════════════════════════════════════════════════════════
    // Stock Edit dialog
    // ══════════════════════════════════════════════════════════════════════════
    public class StockEditDialog : Form
    {
        private readonly string _productId;
        private readonly TextBox txtStock  = new TextBox();
        private readonly Button btnSave    = new Button();
        private readonly Button btnCancel  = new Button();
        private static readonly Color Brown = Color.FromArgb(139, 90, 43);

        public StockEditDialog(string productId, string productName, string currentStock)
        {
            _productId = productId;
            BuildUI(productName, currentStock);
        }

        private void BuildUI(string productName, string currentStock)
        {
            Text = "Edit Stock"; BackColor = Color.White;
            ClientSize = new Size(360, 180);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label {
                Text = $"Product: {productName}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 53, 37),
                AutoSize = true, Location = new Point(24, 20)
            });
            Controls.Add(new Label {
                Text = "New Stock Quantity:",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(80, 65, 50),
                AutoSize = true, Location = new Point(24, 56)
            });
            txtStock.Font = new Font("Segoe UI", 10F);
            txtStock.Location = new Point(24, 76);
            txtStock.Size = new Size(312, 26);
            txtStock.Text = currentStock;
            Controls.Add(txtStock);

            btnSave.Text = "Save"; btnSave.BackColor = Brown; btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat; btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.Size = new Size(110, 34); btnSave.Location = new Point(24, 116);
            btnSave.Click += BtnSave_Click;

            btnCancel.Text = "Cancel"; btnCancel.BackColor = Color.FromArgb(140, 130, 120);
            btnCancel.ForeColor = Color.White; btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.Size = new Size(100, 34); btnCancel.Location = new Point(144, 116);
            btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(btnSave);
            Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtStock.Text.Trim(), out int newStock) || newStock < 0) {
                MessageBox.Show("Enter a valid stock quantity (0 or more).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            try {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand("UPDATE product SET stock=@s WHERE product_id=@id", conn);
                cmd.Parameters.AddWithValue("@s",  newStock);
                cmd.Parameters.AddWithValue("@id", _productId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Stock updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

