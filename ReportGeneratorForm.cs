using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public partial class ReportGeneratorForm : Form
    {
        private static readonly Color Brown = Color.FromArgb(139, 90, 43);

        public ReportGeneratorForm()
        {
            InitializeComponent();
            StyleDataGridView(dgvReport);
            dtpFrom.Value = new DateTime(2026, 1, 1);
            dtpTo.Value   = DateTime.Now;
            dtpFrom.CustomFormat = "MMMM dd, yyyy";
            dtpTo.CustomFormat   = "MMMM dd, yyyy";
            dgvReport.Visible = false;
            lblTotalRecords.Visible = false;
            lblTotalSales.Visible = false;
            lblTotalRevenue.Visible = false;
            lblBestSeller.Visible = false;
            lblSummary.Visible = false;
            btnExport.Visible = false;
            // Update layout only after the form is fully shown
            this.Shown  += (_, __) => UpdateReportLayout();
            this.Resize += (_, __) => UpdateReportLayout();
        }

        private void UpdateLayout() => UpdateReportLayout();

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            dgvReport.Columns.Clear();
            dgvReport.Rows.Clear();
            lblTotalRecords.Text = "";
            lblTotalSales.Text   = "";
            lblTotalRevenue.Text = "";
            lblBestSeller.Text   = "";
            lblSummary.Text      = "";

            string type = cmbReportType.SelectedItem?.ToString() ?? "";
            switch (type)
            {
                case "Sales Report":     GenerateSalesReport();     break;
                case "Inventory Report": GenerateInventoryReport(); break;
                case "Order Report":     GenerateOrderReport();     break;
            }
            dgvReport.Visible = true;
            lblTotalRecords.Visible = false;
            lblTotalSales.Visible = false;
            lblTotalRevenue.Visible = false;
            lblBestSeller.Visible = false;
            lblSummary.Visible = true;
            btnExport.Visible = true;
            UpdateReportLayout();
        }

        // Sales Report
        private void GenerateSalesReport()
        {
            dgvReport.Columns.Add("order_id",      "Order ID");
            dgvReport.Columns.Add("order_date",    "Order Date");
            dgvReport.Columns.Add("amount",        "Amount (₱)");
            dgvReport.Columns.Add("payment_method","Payment Method");

            decimal totalSales = 0;
            int transactions = 0;
            int productsSold = 0;
            string bestSeller = "—";

            try {
                using var conn = DatabaseConnection.GetConnection();

                // Main orders query
                const string sqlOrders = @"SELECT o.order_id,
                                                  DATE_FORMAT(o.order_date,'%b %d, %Y') AS order_date,
                                                  COALESCE(p.amount,0) AS amount,
                                                  COALESCE(p.payment_method,'—') AS payment_method
                                           FROM orders o
                                           LEFT JOIN payment p ON o.order_id=p.order_id
                                           WHERE o.order_date BETWEEN @f AND @t
                                           ORDER BY o.order_date DESC, o.order_id";
                using (var cmd = new MySqlCommand(sqlOrders, conn)) {
                    cmd.Parameters.AddWithValue("@f", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@t", dtpTo.Value.Date);
                    using var r = cmd.ExecuteReader();
                    while (r.Read()) {
                        decimal amt = Convert.ToDecimal(r["amount"]);
                        totalSales += amt;
                        transactions++;
                        dgvReport.Rows.Add(r["order_id"], r["order_date"],
                                           amt.ToString("N2"), r["payment_method"]);
                    }
                }

                // Products sold + best seller query
                const string sqlTotalQty = @"SELECT COALESCE(SUM(oi.quantity),0) AS total_qty
                                             FROM orderitem oi
                                             JOIN orders o ON oi.order_id = o.order_id
                                             WHERE o.order_date BETWEEN @f AND @t";
                using (var cmd2 = new MySqlCommand(sqlTotalQty, conn)) {
                    cmd2.Parameters.AddWithValue("@f", dtpFrom.Value.Date);
                    cmd2.Parameters.AddWithValue("@t", dtpTo.Value.Date);
                    var result = cmd2.ExecuteScalar();
                    productsSold = result == DBNull.Value ? 0 : Convert.ToInt32(result);
                }

                // Best selling product
                const string sqlBest = @"SELECT pr.product_name, SUM(oi.quantity) AS qty
                                         FROM orderitem oi
                                         JOIN orders o  ON oi.order_id  = o.order_id
                                         JOIN product pr ON oi.product_id = pr.product_id
                                         WHERE o.order_date BETWEEN @f AND @t
                                         GROUP BY pr.product_id, pr.product_name
                                         ORDER BY qty DESC
                                         LIMIT 1";
                using (var cmd3 = new MySqlCommand(sqlBest, conn)) {
                    cmd3.Parameters.AddWithValue("@f", dtpFrom.Value.Date);
                    cmd3.Parameters.AddWithValue("@t", dtpTo.Value.Date);
                    using var r3 = cmd3.ExecuteReader();
                    if (r3.Read())
                        bestSeller = $"{r3["product_name"]} ({r3["qty"]} sold)";
                }

            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblTotalRecords.Text = $"   Total Sales: ₱{totalSales:N2}";
            lblTotalSales.Text   = $"   No. of Transactions: {transactions}";
            lblTotalRevenue.Text = $"   Products Sold: {productsSold}";
            lblBestSeller.Text   = $"   Best Selling Product: {bestSeller}";
            lblSummary.Text = $"Total Sales: ₱{totalSales:N2}\nNo. of Transactions: {transactions}\nProducts Sold: {productsSold}\nBest Selling Product: {bestSeller}";
        }

        // Inventory Report
        private void GenerateInventoryReport()
        {
            dgvReport.Columns.Add("product_id",   "Product ID");
            dgvReport.Columns.Add("product_name", "Product Name");
            dgvReport.Columns.Add("category",     "Category");
            dgvReport.Columns.Add("price",        "Price (₱)");
            dgvReport.Columns.Add("stock",        "Stock");
            dgvReport.Columns.Add("status",       "Status");

            try {
                using var conn = DatabaseConnection.GetConnection();
                const string sql = @"SELECT p.product_id, p.product_name, c.category_name, p.price, p.stock,
                                            CASE WHEN p.stock=0   THEN 'Out of Stock'
                                                 WHEN p.stock<=10 THEN 'Low Stock'
                                                 ELSE 'In Stock' END AS status
                                     FROM product p JOIN category c ON p.category_id=c.category_id
                                     ORDER BY c.category_name, p.product_name";
                using var cmd = new MySqlCommand(sql, conn);
                using var r = cmd.ExecuteReader();
                while (r.Read()) {
                    int rowIdx = dgvReport.Rows.Add(r["product_id"], r["product_name"],
                                                    r["category_name"],
                                                    Convert.ToDecimal(r["price"]).ToString("N2"),
                                                    r["stock"], r["status"]);
                    string st = r["status"].ToString()!;
                    if (st == "Low Stock")
                        dgvReport.Rows[rowIdx].DefaultCellStyle.ForeColor = Color.FromArgb(180,100,0);
                    else if (st == "Out of Stock")
                        dgvReport.Rows[rowIdx].DefaultCellStyle.ForeColor = Color.FromArgb(180,50,50);
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblTotalRecords.Text = $"   Total Products: {dgvReport.Rows.Count}";
            lblTotalSales.Text   = "";
            lblTotalRevenue.Text = "";
            lblBestSeller.Text   = "";
            lblSummary.Text = $"Total Products: {dgvReport.Rows.Count}";
        }

        // Order Report
        private void GenerateOrderReport()
        {
            dgvReport.Columns.Add("order_id",    "Order ID");
            dgvReport.Columns.Add("customer",    "Customer");
            dgvReport.Columns.Add("product",     "Product");
            dgvReport.Columns.Add("quantity",    "Quantity");
            dgvReport.Columns.Add("amount",      "Amount (₱)");
            dgvReport.Columns.Add("order_date",  "Order Date");

            int totalRows = 0;
            int totalQty  = 0;
            decimal totalAmt = 0;

            try {
                using var conn = DatabaseConnection.GetConnection();
                const string sql = @"SELECT o.order_id,
                                            CONCAT(c.fname,' ',c.lname) AS customer,
                                            pr.product_name AS product,
                                            oi.quantity,
                                            COALESCE(p.amount, 0) AS amount,
                                            DATE_FORMAT(o.order_date,'%b %d, %Y') AS order_date
                                     FROM orders o
                                     JOIN customer c    ON o.customer_id  = c.customer_id
                                     JOIN orderitem oi  ON o.order_id     = oi.order_id
                                     JOIN product pr    ON oi.product_id  = pr.product_id
                                     LEFT JOIN payment p ON o.order_id    = p.order_id
                                     WHERE o.order_date BETWEEN @f AND @t
                                     ORDER BY o.order_date DESC, o.order_id, pr.product_name";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@f", dtpFrom.Value.Date);
                cmd.Parameters.AddWithValue("@t", dtpTo.Value.Date);
                using var r = cmd.ExecuteReader();
                while (r.Read()) {
                    decimal amt = Convert.ToDecimal(r["amount"]);
                    int qty     = Convert.ToInt32(r["quantity"]);
                    totalRows++;
                    totalQty += qty;
                    dgvReport.Rows.Add(r["order_id"], r["customer"], r["product"],
                                       qty, amt.ToString("N2"), r["order_date"]);
                }

                const string sqlTotal = @"SELECT COALESCE(SUM(p.amount), 0)
                                          FROM orders o
                                          LEFT JOIN payment p ON o.order_id = p.order_id
                                          WHERE o.order_date BETWEEN @f AND @t";
                using var conn2 = DatabaseConnection.GetConnection();
                using (var cmdTotal = new MySqlCommand(sqlTotal, conn2)) {
                    cmdTotal.Parameters.AddWithValue("@f", dtpFrom.Value.Date);
                    cmdTotal.Parameters.AddWithValue("@t", dtpTo.Value.Date);
                    totalAmt = Convert.ToDecimal(cmdTotal.ExecuteScalar());
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblTotalRecords.Text = $"   Total Orders: {totalRows}";
            lblTotalSales.Text   = $"   Total Qty Sold: {totalQty}";
            lblTotalRevenue.Text = $"   Total Amount: ₱{totalAmt:N2}";
            lblBestSeller.Text   = "";
            lblSummary.Text = $"Total Orders: {totalRows}\nTotal Qty Sold: {totalQty}\nTotal Amount: ₱{totalAmt:N2}";
        }

        // Layout
        private void UpdateReportLayout()
        {
            if (panelMain == null || panelMain.Width == 0) return;

            // 1-inch margin at 96 DPI = 96 px each side
            int margin = 96;
            int width  = panelMain.Width - margin * 2;
            if (width <= 0) return;

            // Title row — shift up when embedded (title hidden)
            int titleTop    = lblTitle.Visible ? 18 : -40;
            int filterLblY  = lblTitle.Visible ? 66 : 14;
            int filterCtrlY = lblTitle.Visible ? 86 : 34;
            int btnY        = filterCtrlY;

            lblTitle.Location = new Point(margin, titleTop);

            // Filter row
            lblReportType.Location = new Point(margin, filterLblY);
            cmbReportType.SetBounds(margin, filterCtrlY, 220, cmbReportType.Height);

            lblDateFrom.Location = new Point(margin + 240, filterLblY);
            dtpFrom.SetBounds(margin + 240, filterCtrlY, 200, dtpFrom.Height);

            lblDateTo.Location = new Point(margin + 460, filterLblY);
            dtpTo.SetBounds(margin + 460, filterCtrlY, 200, dtpTo.Height);

            // Generate button — right beside dtpTo with a small gap
            btnGenerate.SetBounds(margin + 460 + 200 + 12, btnY, 140, 34);

            // Export and Close buttons — top-right, anchored to right edge
            btnClose.SetBounds(margin + width - 100, btnY, 100, 34);
            btnExport.SetBounds(margin + width - 100 - 150 - 8, btnY, 150, 34);

            // Card panel
            int cardTop = filterCtrlY + 44;
            pnlCard.SetBounds(margin, cardTop, width, panelMain.Height - cardTop - margin);

            // Table inside card
            int summaryH  = 20 + (lblSummary.Font.Height + 4) * Math.Max(1, lblSummary.Text.Split('\n').Length) + 16;
            int maxTableH = pnlCard.Height - summaryH;
            int tableH    = ProductsForm.DgvContentHeight(dgvReport, Math.Max(60, maxTableH));
            dgvReport.SetBounds(0, 0, pnlCard.Width, tableH);

            // Summary label — full width, vertical stacked
            int sumY = dgvReport.Bottom;
            lblSummary.SetBounds(0, sumY, pnlCard.Width, summaryH);

            // Hide old individual labels (zero-size stubs)
            lblTotalRecords.SetBounds(0, 0, 0, 0);
            lblTotalSales.SetBounds(0, 0, 0, 0);
            lblTotalRevenue.SetBounds(0, 0, 0, 0);
            lblBestSeller.SetBounds(0, 0, 0, 0);
        }

        // Export Excel
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0) {
                MessageBox.Show("Please generate a report first.", "No Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string type = cmbReportType.SelectedItem?.ToString() ?? "Report";
            using var sfd = new SaveFileDialog {
                Filter = "Excel|*.xlsx",
                FileName = type.Replace(" ", "") + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            switch (type) {
                case "Sales Report":
                    ExcelExporter.ExportSales(dgvReport, sfd.FileName, lblSummary.Text);
                    break;
                case "Inventory Report":
                    ExcelExporter.ExportInventory(dgvReport, sfd.FileName, lblSummary.Text);
                    break;
                case "Order Report":
                    ExcelExporter.ExportOrders(dgvReport, sfd.FileName, lblSummary.Text);
                    break;
            }
            MessageBox.Show("Exported successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void StyleDataGridView(DataGridView dgv) => ProductsForm.StyleDgv(dgv);
    }
}
