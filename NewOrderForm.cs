using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public class NewOrderForm : Form
    {
        // Controls
        private readonly Label       lblTitle           = new Label();
        private readonly Label       lblCustomerName    = new Label();
        private readonly TextBox     txtCustomerName    = new TextBox();
        private readonly Label       lblOrderDate       = new Label();
        private readonly DateTimePicker dtpDate         = new DateTimePicker();
        private readonly Label       lblProductSearch   = new Label();
        private readonly TextBox     txtProductSearch   = new TextBox();
        private readonly Label       lblQty             = new Label();
        private readonly NumericUpDown nudQty           = new NumericUpDown();
        private readonly Button      btnAddItem         = new Button();
        private readonly Label       lblPayment         = new Label();
        private readonly ComboBox    cmbPayment         = new ComboBox();

        // Order items section
        private readonly Label       lblItemsTitle      = new Label();
        private readonly DataGridView dgvItems          = new DataGridView();
        private readonly Button      btnRemoveItem      = new Button();

        private readonly Label       lblTotal           = new Label();
        private readonly Button      btnSave            = new Button();
        private readonly Button      btnCancel          = new Button();

        // Autocomplete
        private readonly ListBox     lstSuggestions     = new ListBox();

        private static readonly Color Brown  = Color.FromArgb(139, 90, 43);
        private static readonly Color BgWarm = Color.FromArgb(250, 248, 245);

        private readonly Dictionary<string, (string name, int qty, decimal price)> _cart = new();
        private readonly List<ProductItem> _allProducts = new();
        private ProductItem? _selectedProduct = null;

        // Layout constants
        private const int Lx     = 28;   // left margin
        private const int FieldW = 500;  // field width
        private const int FormW  = 580;  // form width

        public NewOrderForm()
        {
            BuildUI();
            LoadProducts();
        }

        private void BuildUI()
        {
            Text            = "New Order";
            BackColor       = BgWarm;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            StartPosition   = FormStartPosition.CenterParent;
            Width           = FormW;

            int y = 16;

            // Title
            lblTitle.Text      = "New Order";
            lblTitle.Font      = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(74, 53, 37);
            lblTitle.AutoSize  = true;
            lblTitle.Location  = new Point(Lx, y);
            Controls.Add(lblTitle);
            y += 44;

            // Divider
            AddDivider(y); y += 14;

            // Customer Name
            y = AddField(lblCustomerName, "Customer Name", y);
            txtCustomerName.Font            = new Font("Segoe UI", 11F);
            txtCustomerName.Location        = new Point(Lx, y);
            txtCustomerName.Size            = new Size(FieldW, 30);
            txtCustomerName.PlaceholderText = "Type customer full name…";
            Controls.Add(txtCustomerName); y += 38;

            // Order Date
            y = AddField(lblOrderDate, "Order Date", y);
            dtpDate.Font         = new Font("Segoe UI", 11F);
            dtpDate.Location     = new Point(Lx, y);
            dtpDate.Size         = new Size(FieldW, 30);
            dtpDate.Value        = DateTime.Now;
            dtpDate.Format       = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "MMM dd, yyyy";
            Controls.Add(dtpDate); y += 38;

            // Product Search
            y = AddField(lblProductSearch, "Product Search", y);
            txtProductSearch.Font            = new Font("Segoe UI", 11F);
            txtProductSearch.Location        = new Point(Lx, y);
            txtProductSearch.Size            = new Size(FieldW, 30);
            txtProductSearch.PlaceholderText = "Type to search products…";
            txtProductSearch.TextChanged    += TxtProductSearch_TextChanged;
            txtProductSearch.KeyDown        += TxtProductSearch_KeyDown;
            Controls.Add(txtProductSearch); y += 38;

            // Qty + Add button
            y = AddField(lblQty, "Quantity", y);
            nudQty.Font     = new Font("Segoe UI", 11F);
            nudQty.Location = new Point(Lx, y);
            nudQty.Size     = new Size(90, 30);
            nudQty.Minimum  = 1; nudQty.Maximum = 999; nudQty.Value = 1;
            Controls.Add(nudQty);

            MkBtn(btnAddItem, "Add to Order", Brown, new Point(Lx + 100, y - 1), new Size(FieldW - 100, 32));
            btnAddItem.Click += BtnAddItem_Click;
            Controls.Add(btnAddItem); y += 42;

            // Payment Method
            AddDivider(y); y += 14;
            y = AddField(lblPayment, "Payment Method", y);
            cmbPayment.Font          = new Font("Segoe UI", 11F);
            cmbPayment.Location      = new Point(Lx, y);
            cmbPayment.Size          = new Size(FieldW, 30);
            cmbPayment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPayment.Items.AddRange(new object[] { "Cash", "GCash" });
            cmbPayment.SelectedIndex = 0;
            Controls.Add(cmbPayment); y += 42;

            // Order Items section
            lblItemsTitle.Text      = "Order Items";
            lblItemsTitle.Font      = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblItemsTitle.ForeColor = Color.FromArgb(74, 53, 37);
            lblItemsTitle.AutoSize  = true;
            lblItemsTitle.Location  = new Point(Lx, y);
            lblItemsTitle.Visible   = false;
            Controls.Add(lblItemsTitle); y += 26;

            ProductsForm.StyleDgv(dgvItems);
            dgvItems.Location  = new Point(Lx, y);
            dgvItems.Size      = new Size(FieldW, 30); // starts minimal; grows with rows
            dgvItems.ScrollBars = ScrollBars.None;
            dgvItems.Columns.Add("pid",      "ID");
            dgvItems.Columns.Add("pname",    "Product");
            dgvItems.Columns.Add("qty",      "Qty");
            dgvItems.Columns.Add("price",    "Price");
            dgvItems.Columns.Add("subtotal", "Subtotal");
            dgvItems.Columns["pid"].FillWeight      = 50;
            dgvItems.Columns["pname"].FillWeight    = 160;
            dgvItems.Columns["qty"].FillWeight      = 40;
            dgvItems.Columns["price"].FillWeight    = 70;
            dgvItems.Columns["subtotal"].FillWeight = 70;
            dgvItems.Visible = false;
            Controls.Add(dgvItems); y += 30;

            MkBtn(btnRemoveItem, "Remove Selected", Color.FromArgb(180, 60, 60),
                  new Point(Lx, y), new Size(160, 30));
            btnRemoveItem.Click   += BtnRemoveItem_Click;
            btnRemoveItem.Visible  = false;
            Controls.Add(btnRemoveItem); y += 38;

            // Total
            lblTotal.Font      = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTotal.ForeColor = Brown;
            lblTotal.AutoSize  = true;
            lblTotal.Location  = new Point(Lx, y);
            lblTotal.Text      = "Total: ₱0.00";
            Controls.Add(lblTotal); y += 40;

            // Save / Cancel
            MkBtn(btnSave,   "Save Order", Brown,                          new Point(Lx, y),       new Size(170, 42));
            MkBtn(btnCancel, "Cancel",     Color.FromArgb(140, 130, 120),  new Point(Lx + 180, y), new Size(110, 42));
            btnSave.Font    = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCancel.Font  = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnSave.Click   += BtnSave_Click;
            btnCancel.Click += (_, __) => Close();
            Controls.Add(btnSave);
            Controls.Add(btnCancel); y += 56;

            // Autocomplete listbox — floats on top
            lstSuggestions.Font        = new Font("Segoe UI", 10.5F);
            lstSuggestions.Visible     = false;
            lstSuggestions.BorderStyle = BorderStyle.FixedSingle;
            lstSuggestions.BackColor   = Color.White;
            lstSuggestions.Click      += LstSuggestions_Click;
            lstSuggestions.KeyDown    += (s, e) => { if (e.KeyCode == Keys.Enter) SelectSuggestion(); };
            Controls.Add(lstSuggestions);
            lstSuggestions.BringToFront();

            // Set initial form height (no items yet)
            ClientSize = new Size(FormW, y);
            _baseHeight = y;
        }

        // Track the base height
        private int _baseHeight;

        private int AddField(Label lbl, string text, int y)
        {
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 9.5F);
            lbl.ForeColor = Color.FromArgb(80, 65, 50);
            lbl.AutoSize  = true;
            lbl.Location  = new Point(Lx, y);
            Controls.Add(lbl);
            return y + 22;
        }

        private void AddDivider(int y)
        {
            var sep = new Panel
            {
                BackColor = Color.FromArgb(220, 215, 210),
                Location  = new Point(Lx, y),
                Size      = new Size(FieldW, 1)
            };
            Controls.Add(sep);
        }

        private void ShowItemsSection()
        {
            if (lblItemsTitle.Visible) return;

            lblItemsTitle.Visible = true;
            dgvItems.Visible      = true;
            btnRemoveItem.Visible = true;
        }

        // Returns the pixel height needed to show all rows in the grid (header + rows, no scrollbar)
        private int GridContentHeight()
        {
            int h = dgvItems.ColumnHeadersHeight;
            foreach (DataGridViewRow row in dgvItems.Rows)
                if (!row.IsNewRow) h += row.Height;
            return h + 2; // +2 for border
        }

        private void LoadProducts()
        {
            _allProducts.Clear();
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd  = new MySqlCommand(
                    "SELECT product_id, product_name, price, stock FROM product WHERE stock>0 ORDER BY product_name", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    _allProducts.Add(new ProductItem(
                        r["product_id"].ToString()!,
                        r["product_name"].ToString()!,
                        Convert.ToDecimal(r["price"]),
                        Convert.ToInt32(r["stock"])));
            }
            catch { }
        }

        private void TxtProductSearch_TextChanged(object? sender, EventArgs e)
        {
            string query = txtProductSearch.Text.Trim();
            _selectedProduct = null;

            if (string.IsNullOrEmpty(query)) { lstSuggestions.Visible = false; return; }

            lstSuggestions.Items.Clear();
            foreach (var p in _allProducts)
                if (p.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    lstSuggestions.Items.Add(p);

            if (lstSuggestions.Items.Count > 0)
            {
                int count = Math.Min(lstSuggestions.Items.Count, 6);
                lstSuggestions.Height   = lstSuggestions.ItemHeight * count + 4;
                lstSuggestions.Location = new Point(txtProductSearch.Left,
                                                    txtProductSearch.Bottom);
                lstSuggestions.Width    = txtProductSearch.Width;
                lstSuggestions.Visible  = true;
                lstSuggestions.BringToFront();
            }
            else
            {
                lstSuggestions.Visible = false;
            }
        }

        private void TxtProductSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (!lstSuggestions.Visible) return;
            if (e.KeyCode == Keys.Down)
            {
                lstSuggestions.Focus();
                if (lstSuggestions.Items.Count > 0) lstSuggestions.SelectedIndex = 0;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                lstSuggestions.Visible = false;
            }
        }

        private void LstSuggestions_Click(object? sender, EventArgs e) => SelectSuggestion();

        private void SelectSuggestion()
        {
            if (lstSuggestions.SelectedItem is ProductItem pi)
            {
                _selectedProduct       = pi;
                txtProductSearch.Text  = pi.Name;
                lstSuggestions.Visible = false;
                nudQty.Focus();
            }
        }

        private void BtnAddItem_Click(object? sender, EventArgs e)
        {
            if (_selectedProduct == null)
            {
                string q = txtProductSearch.Text.Trim();
                foreach (var p in _allProducts)
                    if (string.Equals(p.Name, q, StringComparison.OrdinalIgnoreCase))
                    { _selectedProduct = p; break; }
            }

            if (_selectedProduct == null)
            {
                MessageBox.Show("Select a product from the suggestions.", "No Product",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProductItem pi  = _selectedProduct;
            int         qty = (int)nudQty.Value;

            if (qty > pi.Stock)
            {
                MessageBox.Show($"Only {pi.Stock} in stock.", "Insufficient Stock",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_cart.ContainsKey(pi.Id))
                _cart[pi.Id] = (pi.Name, _cart[pi.Id].qty + qty, pi.Price);
            else
                _cart[pi.Id] = (pi.Name, qty, pi.Price);

            // Show items section on first add
            ShowItemsSection();

            RefreshCart();
            txtProductSearch.Clear();
            _selectedProduct = null;
            nudQty.Value     = 1;
        }

        private void BtnRemoveItem_Click(object? sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 0) return;
            string pid = dgvItems.SelectedRows[0].Cells["pid"].Value?.ToString() ?? "";
            _cart.Remove(pid);
            RefreshCart();
        }

        private void RefreshCart()
        {
            dgvItems.Rows.Clear();
            decimal total = 0;
            foreach (var kv in _cart)
            {
                decimal sub = kv.Value.qty * kv.Value.price;
                total += sub;
                dgvItems.Rows.Add(kv.Key, kv.Value.name, kv.Value.qty,
                                  "₱" + kv.Value.price.ToString("N2"),
                                  "₱" + sub.ToString("N2"));
            }
            lblTotal.Text = $"Total: ₱{total:N2}";

            // Resize grid to fit rows exactly, then reflow controls below it
            if (dgvItems.Visible)
            {
                int newGridH = GridContentHeight();
                int delta    = newGridH - dgvItems.Height;
                dgvItems.Height = newGridH;

                // Shift everything below the grid
                int gridBottom = dgvItems.Bottom;
                foreach (Control c in Controls)
                    if (c != dgvItems && c != lblItemsTitle && c.Top > dgvItems.Top)
                        c.Top += delta;

                ClientSize = new Size(FormW, ClientSize.Height + delta);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string customerName = txtCustomerName.Text.Trim();
            if (string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Enter a customer name.", "Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (_cart.Count == 0)
            {
                MessageBox.Show("Add at least one product.", "Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            decimal total = 0;
            foreach (var kv in _cart) total += kv.Value.qty * kv.Value.price;

            try
            {
                using var conn = DatabaseConnection.GetConnection();

                // Find or create customer
                string customerId;
                using (var cmd = new MySqlCommand(
                    "SELECT customer_id FROM customer WHERE CONCAT(fname,' ',lname)=@n LIMIT 1", conn))
                {
                    cmd.Parameters.AddWithValue("@n", customerName);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        customerId = result.ToString()!;
                    }
                    else
                    {
                        using var maxCmd = new MySqlCommand(
                            "SELECT MAX(customer_id) FROM customer WHERE customer_id LIKE 'CST%'", conn);
                        var lastId = maxCmd.ExecuteScalar()?.ToString() ?? "CST000";
                        int num = 0;
                        if (lastId.Length >= 3) int.TryParse(lastId.Substring(3), out num);
                        customerId = "CST" + (num + 1).ToString("D3");

                        string[] parts = customerName.Split(' ', 2);
                        string fname = parts[0];
                        string lname = parts.Length > 1 ? parts[1] : "";

                        using var insCmd = new MySqlCommand(
                            "INSERT INTO customer (customer_id, fname, lname) VALUES (@id, @f, @l)", conn);
                        insCmd.Parameters.AddWithValue("@id", customerId);
                        insCmd.Parameters.AddWithValue("@f",  fname);
                        insCmd.Parameters.AddWithValue("@l",  lname);
                        insCmd.ExecuteNonQuery();
                    }
                }

                // Generate next order_id
                string orderId;
                using (var cmd = new MySqlCommand("SELECT MAX(order_id) FROM orders", conn))
                {
                    var last   = cmd.ExecuteScalar()?.ToString() ?? "O000";
                    int num    = 0;
                    string digits = last.TrimStart('O');
                    int.TryParse(digits, out num);
                    orderId = "O" + (num + 1).ToString("D3");
                }

                using (var cmd = new MySqlCommand(
                    "INSERT INTO orders (order_id,customer_id,order_date) VALUES (@oid,@cid,@dt)", conn))
                {
                    cmd.Parameters.AddWithValue("@oid", orderId);
                    cmd.Parameters.AddWithValue("@cid", customerId);
                    cmd.Parameters.AddWithValue("@dt",  dtpDate.Value.Date);
                    cmd.ExecuteNonQuery();
                }

                foreach (var kv in _cart)
                {
                    using var cmd = new MySqlCommand(
                        "INSERT INTO orderitem (order_id,product_id,quantity) VALUES (@oid,@pid,@qty)", conn);
                    cmd.Parameters.AddWithValue("@oid", orderId);
                    cmd.Parameters.AddWithValue("@pid", kv.Key);
                    cmd.Parameters.AddWithValue("@qty", kv.Value.qty);
                    cmd.ExecuteNonQuery();
                }

                string payId;
                using (var cmd = new MySqlCommand("SELECT MAX(payment_id) FROM payment", conn))
                {
                    var last   = cmd.ExecuteScalar()?.ToString() ?? "PM000";
                    int num    = 0;
                    string digits = last.TrimStart('P').TrimStart('M');
                    int.TryParse(digits, out num);
                    payId = "PM" + (num + 1).ToString("D3");
                }

                using (var cmd = new MySqlCommand(
                    "INSERT INTO payment (payment_id,order_id,amount,payment_method) VALUES (@pid,@oid,@amt,@pm)", conn))
                {
                    cmd.Parameters.AddWithValue("@pid", payId);
                    cmd.Parameters.AddWithValue("@oid", orderId);
                    cmd.Parameters.AddWithValue("@amt", total);
                    cmd.Parameters.AddWithValue("@pm",  cmbPayment.SelectedItem?.ToString() ?? "Cash");
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Order {orderId} saved!\nCustomer: {customerName}\nTotal: ₱{total:N2}",
                    "Order Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving order:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void MkBtn(Button b, string t, Color back, Point loc, Size sz)
        {
            b.Text      = t;
            b.BackColor = back;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font      = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            b.Location  = loc;
            b.Size      = sz;
            b.UseVisualStyleBackColor = false;
        }

        private class ProductItem
        {
            public string  Id    { get; }
            public string  Name  { get; }
            public decimal Price { get; }
            public int     Stock { get; }
            public ProductItem(string id, string name, decimal price, int stock)
            { Id = id; Name = name; Price = price; Stock = stock; }
            public override string ToString() => $"{Name}  (₱{Price:N2} | Stock: {Stock})";
        }
    }
}
