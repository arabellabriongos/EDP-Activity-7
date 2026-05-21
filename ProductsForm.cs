using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public class ProductsForm : Form
    {
        private readonly DataGridView dgv    = new DataGridView();
        private readonly TextBox txtSearch   = new TextBox();
        private readonly ComboBox cmbCat     = new ComboBox();
        private readonly Button btnSearch    = new Button();
        private readonly Button btnAdd       = new Button();
        private readonly Button btnEdit      = new Button();
        private readonly Button btnDelete    = new Button();
        private readonly Button btnXClose    = new Button();
        private readonly Label lblTitle      = new Label();
        private readonly Label lblTotal      = new Label();
        // White card panel that holds the table + summary
        private readonly Panel pnlCard       = new Panel();

        private string? _selectedProductId = null;

        private static readonly Color Brown   = Color.FromArgb(139, 90, 43);
        private static readonly Color PageBg  = Color.FromArgb(235, 237, 242); // light gray page bg
        private static readonly Color CardBg  = Color.White;

        public ProductsForm() { BuildUI(); LoadCategories(); LoadProducts(); }

        private void BuildUI()
        {
            Text = "Products"; BackColor = PageBg;
            ClientSize = new Size(860, 560); StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable; MinimumSize = new Size(700, 450);
            this.WindowState = FormWindowState.Maximized;

            // Page title
            lblTitle.Text = "Products";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(74, 53, 37);
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Location = new Point(24, 18);

            // Close button
            MkBtn(btnXClose, "Close", Color.FromArgb(140, 130, 120), new Point(ClientSize.Width - 120, 18), new Size(100, 34));
            btnXClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXClose.Click += (_, __) => Close();

            // Filter controls (positioned in UpdateLayout)
            AddLbl("Search:", new Point(24, 66));
            txtSearch.Font = new Font("Segoe UI", 10F); txtSearch.Size = new Size(200, 28);
            txtSearch.PlaceholderText = "Product name…";
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadProducts(); };

            AddLbl("Category:", new Point(236, 66));
            cmbCat.Font = new Font("Segoe UI", 10F); cmbCat.Size = new Size(170, 28);
            cmbCat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCat.SelectedIndexChanged += (_, __) => LoadProducts();

            MkBtn(btnSearch, "🔍  Search", Brown,                       new Point(0, 0), new Size(110, 34));
            MkBtn(btnAdd,    "+ Add",      Brown,                       new Point(0, 0), new Size(100, 34));
            MkBtn(btnEdit,   "✏ Edit",    Color.FromArgb(80, 120, 80), new Point(0, 0), new Size(100, 34));
            MkBtn(btnDelete, "🗑 Delete",  Color.FromArgb(180, 60, 60), new Point(0, 0), new Size(100, 34));

            btnSearch.Click += (_, __) => LoadProducts();
            btnAdd.Click    += (_, __) => { new ProductEditDialog(null).ShowDialog(); LoadProducts(); };
            btnEdit.Click   += (_, __) => {
                if (_selectedProductId == null) { MessageBox.Show("Select a product to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                new ProductEditDialog(_selectedProductId).ShowDialog(); LoadProducts();
            };
            btnDelete.Click += BtnDelete_Click;

            // White card panel
            pnlCard.BackColor = CardBg;
            pnlCard.Padding   = new Padding(0);

            // Summary label inside card
            lblTotal.Font      = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(74, 53, 37);
            lblTotal.BackColor = Color.FromArgb(247, 244, 238);
            lblTotal.AutoSize  = false;
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            lblTotal.Padding   = new Padding(14, 0, 0, 0);

            StyleDgv(dgv);
            dgv.SelectionChanged += (_, __) => {
                if (dgv.SelectedRows.Count > 0)
                    _selectedProductId = dgv.SelectedRows[0].Cells["product_id"].Value?.ToString();
                else
                    _selectedProductId = null;
            };

            pnlCard.Controls.Add(dgv);
            pnlCard.Controls.Add(lblTotal);

            Controls.AddRange(new Control[] {
                lblTitle, btnXClose,
                txtSearch, cmbCat, btnSearch, btnAdd, btnEdit, btnDelete,
                pnlCard
            });

            Resize += (_, __) => UpdateLayout();
            Load   += (_, __) => UpdateLayout();
        }

        private void UpdateLayout()
        {
            var host = Parent ?? (Control)this;
            int W = host.ClientSize.Width;
            int H = host.ClientSize.Height;
            int m = 96; // margin from content edge

            // Title
            lblTitle.Location  = new Point(m, 18);
            btnXClose.Location = new Point(W - m - btnXClose.Width, 18);

            // Filter row
            int filterY  = lblTitle.Visible ? 62 : 14;
            int ctrlY    = lblTitle.Visible ? 82 : 34;

            // Reposition filter labels
            foreach (Control c in Controls)
                if (c is Label l && l.AutoSize && l.BackColor == Color.Transparent
                    && (l.Location.Y == 66 || l.Location.Y == 14 || l.Location.Y == filterY))
                {
                    // "Search:" is first, "Category:" is second
                    if (l.Text == "Search:")   l.Location = new Point(m, filterY);
                    if (l.Text == "Category:") l.Location = new Point(m + 212, filterY);
                }

            txtSearch.Location = new Point(m, ctrlY);
            cmbCat.Location    = new Point(m + 212, ctrlY);
            btnSearch.Location = new Point(m + 394, ctrlY);
            btnAdd.Location    = new Point(m + 514, ctrlY);
            btnEdit.Location   = new Point(m + 624, ctrlY);
            btnDelete.Location = new Point(m + 734, ctrlY);

            // Card: starts below filter row, fills to bottom
            int cardTop = ctrlY + 44;
            pnlCard.SetBounds(m, cardTop, W - m * 2, H - cardTop - m);

            // DGV inside card: full width, height = content or card height minus summary
            int summaryH = 52;
            int maxDgvH  = pnlCard.Height - summaryH;
            int dgvH     = DgvContentHeight(dgv, Math.Max(60, maxDgvH));
            dgv.SetBounds(0, 0, pnlCard.Width, dgvH);

            // Summary directly below dgv, same width as card
            lblTotal.SetBounds(0, dgv.Bottom, pnlCard.Width, summaryH);
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedProductId == null) {
                MessageBox.Show("Select a product to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (MessageBox.Show($"Delete product {_selectedProductId}?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand("DELETE FROM product WHERE product_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", _selectedProductId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Product deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _selectedProductId = null;
                LoadProducts();
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void LoadProducts()
        {
            dgv.Rows.Clear(); dgv.Columns.Clear();
            dgv.Columns.Add("product_id",   "Product ID");
            dgv.Columns.Add("product_name", "Product Name");
            dgv.Columns.Add("category",     "Category");
            dgv.Columns.Add("price",        "Price (₱)");
            dgv.Columns.Add("stock",        "Stock");
            dgv.Columns["product_id"].FillWeight   = 70;
            dgv.Columns["product_name"].FillWeight = 200;
            dgv.Columns["category"].FillWeight     = 130;
            dgv.Columns["price"].FillWeight        = 90;
            dgv.Columns["stock"].FillWeight        = 70;

            string search = txtSearch.Text.Trim();
            string cat    = cmbCat.SelectedIndex > 0 ? cmbCat.SelectedItem!.ToString()! : "";
            int lowStockCount = 0;
            try {
                using var conn = DatabaseConnection.GetConnection();
                const string sql = @"SELECT p.product_id, p.product_name, c.category_name, p.price, p.stock
                                     FROM product p JOIN category c ON p.category_id=c.category_id
                                     WHERE (@s='' OR p.product_name LIKE @s)
                                       AND (@c='' OR c.category_name=@c)
                                     ORDER BY c.category_name, p.product_name";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@s", string.IsNullOrEmpty(search) ? "" : $"%{search}%");
                cmd.Parameters.AddWithValue("@c", cat);
                using var r = cmd.ExecuteReader();
                while (r.Read()) {
                    int stock = Convert.ToInt32(r["stock"]);
                    int rowIdx = dgv.Rows.Add(r["product_id"], r["product_name"],
                                              r["category_name"],
                                              Convert.ToDecimal(r["price"]).ToString("N2"),
                                              stock);
                    if (stock <= 10) {
                        dgv.Rows[rowIdx].DefaultCellStyle.ForeColor = Color.FromArgb(180, 60, 60);
                        lowStockCount++;
                    }
                }
                lblTotal.Text = $"   Total Products: {dgv.Rows.Count}     Low Stock: {lowStockCount}";
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblTotal.Text = $"   Total Products: {dgv.Rows.Count}     Low Stock: 0";
            }
            UpdateLayout();
        }

        private void AddLbl(string t, Point p) =>
            Controls.Add(new Label {
                Text = t, Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(80,65,50), AutoSize = true,
                BackColor = Color.Transparent, Location = p
            });

        private static void MkBtn(Button b, string t, Color back, Point loc, Size sz) {
            b.Text = t; b.BackColor = back; b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            b.Location = loc; b.Size = sz; b.UseVisualStyleBackColor = false;
            b.Cursor = Cursors.Hand;
        }

        public static void StyleDgv(DataGridView d) {
            d.BackgroundColor = Color.White;
            d.BorderStyle     = BorderStyle.None;
            d.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            d.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            d.EnableHeadersVisualStyles = false;
            d.GridColor = Color.FromArgb(225, 220, 215);
            d.RowHeadersVisible = false;
            d.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            d.MultiSelect = false; d.ReadOnly = true;
            d.AllowUserToAddRows = false; d.AllowUserToDeleteRows = false;
            d.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Header — warm light brown
            d.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 224, 207);
            d.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(74, 53, 37);
            d.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
            d.ColumnHeadersDefaultCellStyle.Padding   = new Padding(8, 0, 0, 0);
            d.ColumnHeadersHeight = 38;
            d.DefaultCellStyle.Font      = new Font("Segoe UI", 10F);
            d.DefaultCellStyle.ForeColor = Color.FromArgb(60, 45, 30);
            d.DefaultCellStyle.Padding   = new Padding(8, 0, 0, 0);
            d.DefaultCellStyle.SelectionBackColor = Color.FromArgb(247, 240, 230);
            d.DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 45, 30);
            d.RowTemplate.Height = 34;
            d.RowsDefaultCellStyle.BackColor            = Color.White;
            d.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 250, 247);
        }

        /// <summary>Height to fit all rows + header, capped at maxH.</summary>
        public static int DgvContentHeight(DataGridView d, int maxH)
        {
            int h = d.ColumnHeadersHeight;
            foreach (DataGridViewRow r in d.Rows) h += r.Height;
            h += 2;
            return Math.Min(h, maxH);
        }
    }

    // Product Add / Edit dialog
    public class ProductEditDialog : Form
    {
        private readonly string? _productId;
        private readonly TextBox txtId       = new TextBox();
        private readonly TextBox txtName     = new TextBox();
        private readonly ComboBox cmbCategory = new ComboBox();
        private readonly TextBox txtPrice    = new TextBox();
        private readonly TextBox txtStock    = new TextBox();
        private readonly Button btnSave      = new Button();
        private readonly Button btnCancel    = new Button();
        private static readonly Color Brown  = Color.FromArgb(139, 90, 43);

        public ProductEditDialog(string? productId)
        {
            _productId = productId;
            BuildUI();
            LoadCategories();
            if (productId != null) LoadProduct(productId);
        }

        private void BuildUI()
        {
            Text = _productId == null ? "Add Product" : "Edit Product";
            BackColor = Color.White;
            ClientSize = new Size(420, 360);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            int lx = 24, fx = 24, fw = 372, y = 20, gy = 28;

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

            txtId.ReadOnly = _productId != null;
            if (_productId != null) txtId.BackColor = Color.FromArgb(240, 240, 240);
            AddRow("Product ID", txtId, ref y);
            AddRow("Product Name", txtName, ref y);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            AddRow("Category", cmbCategory, ref y);
            AddRow("Price (₱)", txtPrice, ref y);
            AddRow("Stock", txtStock, ref y);

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

            ClientSize = new Size(420, y + 60);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            try {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand("SELECT category_id, category_name FROM category ORDER BY category_name", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    cmbCategory.Items.Add(new CategoryItem(r["category_id"].ToString()!, r["category_name"].ToString()!));
            } catch { }
            if (cmbCategory.Items.Count > 0) cmbCategory.SelectedIndex = 0;
        }

        private void LoadProduct(string productId)
        {
            try {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(
                    "SELECT p.product_id, p.product_name, p.category_id, p.price, p.stock FROM product p WHERE p.product_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", productId);
                using var r = cmd.ExecuteReader();
                if (r.Read()) {
                    txtId.Text    = r["product_id"].ToString();
                    txtName.Text  = r["product_name"].ToString();
                    txtPrice.Text = Convert.ToDecimal(r["price"]).ToString("N2");
                    txtStock.Text = r["stock"].ToString();
                    string catId  = r["category_id"].ToString()!;
                    foreach (CategoryItem item in cmbCategory.Items)
                        if (item.Id == catId) { cmbCategory.SelectedItem = item; break; }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error loading product:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string id = txtId.Text.Trim(); string name = txtName.Text.Trim();
            string price = txtPrice.Text.Trim(); string stock = txtStock.Text.Trim();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name)) {
                MessageBox.Show("Product ID and Name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (!decimal.TryParse(price, out decimal priceVal) || priceVal < 0) {
                MessageBox.Show("Enter a valid price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (!int.TryParse(stock, out int stockVal) || stockVal < 0) {
                MessageBox.Show("Enter a valid stock quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (cmbCategory.SelectedItem is not CategoryItem cat) {
                MessageBox.Show("Select a category.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            try {
                using var conn = DatabaseConnection.GetConnection();
                if (_productId == null) {
                    using var cmd = new MySqlCommand(
                        "INSERT INTO product (product_id, product_name, category_id, price, stock) VALUES (@id,@n,@c,@p,@s)", conn);
                    cmd.Parameters.AddWithValue("@id", id); cmd.Parameters.AddWithValue("@n", name);
                    cmd.Parameters.AddWithValue("@c", cat.Id); cmd.Parameters.AddWithValue("@p", priceVal);
                    cmd.Parameters.AddWithValue("@s", stockVal); cmd.ExecuteNonQuery();
                    MessageBox.Show("Product added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else {
                    using var cmd = new MySqlCommand(
                        "UPDATE product SET product_name=@n, category_id=@c, price=@p, stock=@s WHERE product_id=@id", conn);
                    cmd.Parameters.AddWithValue("@n", name); cmd.Parameters.AddWithValue("@c", cat.Id);
                    cmd.Parameters.AddWithValue("@p", priceVal); cmd.Parameters.AddWithValue("@s", stockVal);
                    cmd.Parameters.AddWithValue("@id", _productId); cmd.ExecuteNonQuery();
                    MessageBox.Show("Product updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                DialogResult = DialogResult.OK; Close();
            } catch (MySqlException ex) when (ex.Number == 1062) {
                MessageBox.Show("A product with that ID already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } catch (Exception ex) {
                MessageBox.Show($"Error saving product:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class CategoryItem {
            public string Id { get; }
            private readonly string _name;
            public CategoryItem(string id, string name) { Id = id; _name = name; }
            public override string ToString() => _name;
        }
    }
}
