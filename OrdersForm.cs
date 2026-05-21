using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public class OrdersForm : Form
    {
        private readonly DataGridView dgv   = new DataGridView();
        private readonly DateTimePicker dtpFrom = new DateTimePicker();
        private readonly DateTimePicker dtpTo   = new DateTimePicker();
        private readonly Button btnSearch   = new Button();
        private readonly Button btnDelete   = new Button();
        private readonly Button btnXClose   = new Button();
        private readonly Label  lblTitle    = new Label();
        private readonly Label  lblTotal    = new Label();
        // White card panel that holds the table + summary
        private readonly Panel pnlCard       = new Panel();

        private static readonly Color Brown   = Color.FromArgb(139, 90, 43);
        private static readonly Color PageBg  = Color.FromArgb(235, 237, 242);
        private static readonly Color CardBg  = Color.White;

        public OrdersForm() { BuildUI(); LoadOrders(); }

        private void BuildUI()
        {
            Text = "Orders"; BackColor = PageBg;
            ClientSize = new Size(960, 560); StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable; MinimumSize = new Size(800, 460);
            this.WindowState = FormWindowState.Maximized;

            lblTitle.Text = "Orders"; lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(74, 53, 37); lblTitle.AutoSize = true; lblTitle.BackColor = Color.Transparent; lblTitle.Location = new Point(24, 18);

            // X Close button — top right
            MkBtn(btnXClose, "Close", Color.FromArgb(140, 130, 120), new Point(ClientSize.Width - 120, 18), new Size(100, 34));
            btnXClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXClose.Click += (_, __) => Close();

            AddLbl("From:", new Point(24, 66));
            dtpFrom.Format = DateTimePickerFormat.Custom; dtpFrom.CustomFormat = "MMM dd, yyyy";
            dtpFrom.Value = new DateTime(2026, 1, 1);
            dtpFrom.Font = new Font("Segoe UI", 10F); dtpFrom.Location = new Point(24, 86); dtpFrom.Size = new Size(160, 26);

            AddLbl("To:", new Point(196, 66));
            dtpTo.Format = DateTimePickerFormat.Custom; dtpTo.CustomFormat = "MMM dd, yyyy";
            dtpTo.Value = DateTime.Now;
            dtpTo.Font = new Font("Segoe UI", 10F); dtpTo.Location = new Point(196, 86); dtpTo.Size = new Size(160, 26);

            MkBtn(btnSearch, "🔍 Search", Brown,                       new Point(0, 0), new Size(110, 34));
            MkBtn(btnDelete, "🗑 Delete", Color.FromArgb(180, 60, 60), new Point(0, 0), new Size(100, 34));

            btnSearch.Click += (_, __) => LoadOrders();
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

            ProductsForm.StyleDgv(dgv);

            pnlCard.Controls.Add(dgv);
            pnlCard.Controls.Add(lblTotal);

            Controls.AddRange(new Control[] { lblTitle, btnXClose, dtpFrom, dtpTo,
                btnSearch, btnDelete, pnlCard });

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
                    l.Location = new Point(l.Location.X == m ? m : m + 172, filterTop);

            dtpFrom.Location = new Point(m, controlsTop);
            dtpTo.Location   = new Point(m + 172, controlsTop);
            btnSearch.Location = new Point(m + 384, controlsTop);
            btnDelete.Location = new Point(m + 504, controlsTop);

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

        private void LoadOrders()
        {
            dgv.Rows.Clear(); dgv.Columns.Clear();
            dgv.Columns.Add("order_id",   "Order ID");
            dgv.Columns.Add("customer",   "Customer");
            dgv.Columns.Add("order_date", "Order Date");
            dgv.Columns.Add("amount",     "Amount (₱)");
            dgv.Columns.Add("payment",    "Payment Method");
            dgv.Columns["order_id"].FillWeight   = 70;
            dgv.Columns["customer"].FillWeight   = 200;
            dgv.Columns["order_date"].FillWeight = 120;
            dgv.Columns["amount"].FillWeight     = 110;
            dgv.Columns["payment"].FillWeight    = 130;

            decimal total = 0;
            try {
                using var conn = DatabaseConnection.GetConnection();
                const string sql = @"SELECT o.order_id,
                                            CONCAT(c.fname,' ',c.lname) AS customer,
                                            DATE_FORMAT(o.order_date,'%b %d, %Y') AS order_date,
                                            COALESCE(p.amount,0) AS amount,
                                            COALESCE(p.payment_method,'—') AS payment_method
                                     FROM orders o
                                     JOIN customer c ON o.customer_id=c.customer_id
                                     LEFT JOIN payment p ON o.order_id=p.order_id
                                     WHERE o.order_date BETWEEN @f AND @t
                                     ORDER BY o.order_date DESC, o.order_id";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@f", dtpFrom.Value.Date);
                cmd.Parameters.AddWithValue("@t", dtpTo.Value.Date);
                using var r = cmd.ExecuteReader();
                while (r.Read()) {
                    decimal amt = Convert.ToDecimal(r["amount"]);
                    total += amt;
                    dgv.Rows.Add(r["order_id"], r["customer"], r["order_date"],
                                 amt.ToString("N2"), r["payment_method"]);
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lblTotal.Text = $"   Total Orders: {dgv.Rows.Count}\n   Total Amount: ₱{total:N2}";
            UpdateLayout();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) {
                MessageBox.Show("Select an order to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string orderId = dgv.SelectedRows[0].Cells["order_id"].Value?.ToString() ?? "";
            if (MessageBox.Show($"Delete order {orderId}? This will also remove its items and payment.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try {
                using var conn = DatabaseConnection.GetConnection();
                foreach (string tbl in new[] { "payment", "orderitem", "orders" }) {
                    using var cmd = new MySqlCommand($"DELETE FROM {tbl} WHERE order_id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show($"Order {orderId} deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrders();
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
}


