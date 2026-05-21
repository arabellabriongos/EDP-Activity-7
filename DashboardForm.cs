using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public partial class DashboardForm : Form
    {
        private readonly PictureBox picLogo      = new PictureBox();
        private readonly PictureBox picAdmin     = new PictureBox();
        private readonly Label      lblAdminName = new Label();
        private readonly List<Button> _navButtons = new List<Button>();
        private readonly Button     btnSideLogout = new Button();

        private readonly Label  lblTodayDate  = new Label();
        private readonly Button btnNewOrder   = new Button();
        private readonly Label  lblBell       = new Label();
        private readonly PictureBox picTopAdmin   = new PictureBox();
        private readonly Label      lblTopName    = new Label();
        private readonly Label      lblTopRole    = new Label();

        private readonly Label lblSalesCaption     = new Label();
        private readonly Label lblOrdersCaption    = new Label();
        private readonly Label lblCustomersCaption = new Label();
        private readonly Label lblProductsCaption  = new Label();

        private readonly Panel  panelRevenueCard   = new Panel();
        private readonly Panel  panelRevenueCanvas = new Panel();
        private readonly Label  lblWeekBigTotal    = new Label();
        private readonly Button btnTfWeek  = new Button();
        private readonly Button btnTfMonth = new Button();
        private readonly Button btnTfYear  = new Button();
        private readonly ComboBox cmbRevPeriod = new ComboBox();
        private int    _revWeekOffset  = 0;
        private int    _revMonth       = DateTime.Now.Month;
        private int    _revYear        = DateTime.Now.Year;

        private readonly Panel panelTopSelling       = new Panel();
        private readonly Panel panelTopSellingCanvas = new Panel();

        private readonly Panel        panelRecentOrders = new Panel();
        private readonly DataGridView dgvRecentOrders   = new DataGridView();

        private readonly Panel panelScroll = new Panel();

        // legacy panels
        private readonly Panel panelTrackOrder      = new Panel();
        private readonly Panel panelNewOrder        = new Panel();
        private readonly Panel panelSalesWeek       = new Panel();
        private readonly Panel panelSalesWeekCanvas = new Panel();
        private readonly Panel panelChartLegend     = new Panel();
        private readonly FlowLayoutPanel panelTimeframe = new FlowLayoutPanel();
        private readonly Panel panelTopSellingOld   = new Panel();

        private readonly int[]    _weekWeights = { 125, 138, 146, 158, 149, 141, 152 };
        private readonly string[] _weekLabels  = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        private int     _todayOrders    = 0;
        private int     _todayCustomers = 0;
        private int     _todayProducts  = 0;
        private decimal _weekTotalPeso  = 0m;
        private decimal _todaySales     = 0m;
        private int     _selectedNavIndex = 0;
        private string  _loggedInName  = "Admin";
        private string  _loggedInRole  = "admin";
        private int     _loggedInId    = 0;
        private Form?   _currentModule = null;
        private (string name, int sold, decimal price)[] _topItems
            = Array.Empty<(string, int, decimal)>();
        private decimal[] _revenueByDay   = new decimal[7];
        private string[]  _revenueLabels  = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        private string    _revenueTimeframe = "Week";

        private static readonly Color Brown      = Color.FromArgb(139, 90, 43);
        private static readonly Color BgContent  = Color.FromArgb(245, 246, 250);
        private static readonly Color CardBg     = Color.White;
        private static readonly Color CardBeige  = Color.FromArgb(247, 244, 238);
        private static readonly Color SidebarBg  = Color.FromArgb(248, 249, 252);
        private static readonly Color SidebarActive = Color.FromArgb(139, 90, 43);
        private static readonly Color SidebarIdle   = Color.FromArgb(248, 249, 252);

        public DashboardForm() : this("Admin", "admin", 0) { }

        public DashboardForm(string fullName, string role, int userId = 0)
        {
            _loggedInName = fullName;
            _loggedInRole = role;
            _loggedInId   = userId;
            InitializeComponent();
            BuildDashboardLayout();
            LoadDashboardData();
            this.Resize += (_, __) => { ConfigureResponsiveLayout(); LayoutSidebar(); };
            panelSidebar.Resize += (_, __) => LayoutSidebar();
        }

        private void BuildDashboardLayout()
        {
            panelSidebar.BackColor = SidebarBg;
            panelTop.BackColor     = Color.White;
            panelContent.BackColor = Color.White;

            panelSidebar.Controls.Clear();
            panelContent.Controls.Clear();
            panelTop.Controls.Clear();
            _navButtons.Clear();

            picLogo.Size      = new Size(96, 96);
            picLogo.SizeMode  = PictureBoxSizeMode.Zoom;
            picLogo.BackColor = Color.FromArgb(200, 185, 165);
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "brew&bite.jpg");
            if (!File.Exists(logoPath))
                logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "coffee1.png");
            if (File.Exists(logoPath))
                try { picLogo.Image = Image.FromFile(logoPath); } catch { }
            panelSidebar.Controls.Add(picLogo);

            var navList = new List<string> { "Dashboard", "Products", "Orders", "Customers", "Reports" };
            if (_loggedInRole == "admin") navList.Add("Users");
            navList.Add("About");

            var navIcons = new Dictionary<string, string>
            {
                {"Dashboard","⊞"},{"Products","☕"},{"Orders","🧾"},
                {"Customers","👥"},{"Inventory","📦"},{"Reports","📊"},
                {"Users","👤"},{"About","ℹ"}
            };

            for (int i = 0; i < navList.Count; i++)
            {
                int idx  = i;
                string icon = navIcons.TryGetValue(navList[i], out var ic) ? ic + "  " : "   ";
                var b = new Button
                {
                    Text      = icon + navList[i],
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI", 10F),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding   = new Padding(14, 0, 0, 0),
                    Cursor    = Cursors.Hand
                };
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(235, 228, 218);
                b.Click += (_, __) => SelectNav(idx);
                if (navList[i] == "Dashboard")  b.Click += (_, __) => RestoreDashboardContent();
                if (navList[i] == "Products")   b.Click += (_, __) => LoadModuleIntoContent(() => new ProductsForm());
                if (navList[i] == "Orders")     b.Click += (_, __) => LoadModuleIntoContent(() => new OrdersForm());
                if (navList[i] == "Customers")  b.Click += (_, __) => LoadModuleIntoContent(() => new CustomersForm());
                if (navList[i] == "Users")      b.Click += (_, __) => LoadModuleIntoContent(() => new UserManagementForm());
                if (navList[i] == "Reports")    b.Click += (_, __) => LoadModuleIntoContent(() => new ReportGeneratorForm());
                if (navList[i] == "About")      b.Click += (_, __) => ShowWithBlur(() => new AboutForm());
                _navButtons.Add(b);
                panelSidebar.Controls.Add(b);
            }

            btnSideLogout.Text      = "🚪  Logout";
            btnSideLogout.FlatStyle = FlatStyle.Flat;
            btnSideLogout.Font      = new Font("Segoe UI", 10F);
            btnSideLogout.ForeColor = Color.FromArgb(180, 60, 60);
            btnSideLogout.BackColor = SidebarBg;
            btnSideLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnSideLogout.Padding   = new Padding(14, 0, 0, 0);
            btnSideLogout.Cursor    = Cursors.Hand;
            btnSideLogout.FlatAppearance.BorderSize = 0;
            btnSideLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 235, 235);
            btnSideLogout.Click += btnLogout_Click;
            panelSidebar.Controls.Add(btnSideLogout);

            panelSidebar.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using (var pen = new Pen(Color.FromArgb(220, 220, 228), 1))
                    g.DrawLine(pen, panelSidebar.Width - 1, 0, panelSidebar.Width - 1, panelSidebar.Height);
            };

            _selectedNavIndex = 0;
            ApplyNavStyles();

            lblTitle.Text      = "Dashboard";
            lblTitle.ForeColor = Color.FromArgb(74, 53, 37);
            lblTitle.Font      = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.AutoSize  = true;
            panelTop.Controls.Add(lblTitle);

            btnNewOrder.Text      = "New Order";
            btnNewOrder.BackColor = Brown;
            btnNewOrder.ForeColor = Color.White;
            btnNewOrder.FlatStyle = FlatStyle.Flat;
            btnNewOrder.FlatAppearance.BorderSize = 0;
            btnNewOrder.Font      = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnNewOrder.Size      = new Size(140, 36);
            btnNewOrder.UseVisualStyleBackColor = false;
            btnNewOrder.Click += (_, __) => { ShowWithBlur(() => new NewOrderForm()); LoadDashboardData(); };
            panelTop.Controls.Add(btnNewOrder);

            lblUser.Visible = false;
            panelTop.Controls.Add(lblUser);

            lblBell.Text      = "🔔";
            lblBell.Font      = new Font("Segoe UI", 16F);
            lblBell.AutoSize  = true;
            lblBell.Cursor    = Cursors.Hand;
            lblBell.BackColor = Color.Transparent;
            panelTop.Controls.Add(lblBell);

            picTopAdmin.Size      = new Size(38, 38);
            picTopAdmin.SizeMode  = PictureBoxSizeMode.Zoom;
            picTopAdmin.BackColor = Color.FromArgb(200, 185, 165);
            picTopAdmin.Cursor    = Cursors.Hand;
            string[] imgCands = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user.png"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "coffee1.png")
            };
            foreach (string c in imgCands)
                if (File.Exists(c)) { try { picTopAdmin.Image = Image.FromFile(c); break; } catch { } }
            void OpenUserProfile()
            {
                var dlg = new UserProfileForm(_loggedInId, _loggedInRole);
                ShowWithBlur(() => dlg);
                if (dlg.DialogResult == DialogResult.OK)
                {
                    _loggedInName     = dlg.UpdatedFullName;
                    lblTopName.Text   = _loggedInName;
                    lblAdminName.Text = _loggedInName;
                }
            }
            picTopAdmin.Click += (_, __) => OpenUserProfile();
            panelTop.Controls.Add(picTopAdmin);

            lblTopName.Text      = _loggedInName;
            lblTopName.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTopName.ForeColor = Color.FromArgb(55, 38, 22);
            lblTopName.AutoSize  = true;
            lblTopName.BackColor = Color.Transparent;
            lblTopName.Cursor    = Cursors.Hand;
            lblTopName.Click    += (_, __) => OpenUserProfile();
            panelTop.Controls.Add(lblTopName);

            lblTopRole.Text      = _loggedInRole == "admin" ? "Administrator" : "Staff";
            lblTopRole.Font      = new Font("Segoe UI", 8.5F);
            lblTopRole.ForeColor = Brown;
            lblTopRole.AutoSize  = true;
            lblTopRole.BackColor = Color.Transparent;
            panelTop.Controls.Add(lblTopRole);

            btnLogout.Visible = false;
            panelTop.Controls.Add(btnLogout);

            panelTop.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(230, 228, 225), 1);
                e.Graphics.DrawLine(pen, 0, panelTop.Height - 1, panelTop.Width, panelTop.Height - 1);
            };

            panelScroll.AutoScroll  = true;
            panelScroll.Dock        = DockStyle.Fill;
            panelScroll.BackColor   = Color.White;
            panelContent.Controls.Add(panelScroll);

            lblWelcome.Text      = "Today's Overview";
            lblWelcome.ForeColor = Color.FromArgb(74, 53, 37);
            lblWelcome.Font      = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblWelcome.AutoSize  = true;
            panelScroll.Controls.Add(lblWelcome);

            lblTodayDate.Font      = new Font("Segoe UI", 10F);
            lblTodayDate.ForeColor = Color.FromArgb(98, 82, 67);
            lblTodayDate.AutoSize  = true;
            panelScroll.Controls.Add(lblTodayDate);

            panelTodayStats.BackColor = Color.Transparent;
            panelScroll.Controls.Add(panelTodayStats);

            Color cardColor = CardBeige;
            foreach (Panel p in new[] { pnlSalesCard, pnlOrdersCard, pnlCustomersCard, pnlProductsCard })
            {
                p.BackColor   = CardBg;
                p.Tag         = cardColor;
                p.BorderStyle = BorderStyle.None;
                p.Paint      += PaintCardShadow;
            }

            lblTotalSales.Text     = "Sales Today";
            lblTotalOrders.Text    = "Orders Today";
            lblTotalCustomers.Text = "Customers Today";
            lblTotalProducts.Text  = "Products Sold";
            lblSalesValue.Text     = "₱ 0.00";
            lblOrdersValue.Text    = "0";
            lblCustomersValue.Text = "0";
            lblProductsValue.Text  = "0";

            foreach (Label l in new[] { lblTotalSales, lblTotalOrders, lblTotalCustomers, lblTotalProducts })
            {
                l.ForeColor  = Color.FromArgb(100, 85, 70);
                l.Font       = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                l.BackColor  = Color.Transparent;
                l.AutoSize   = false;
                l.TextAlign  = ContentAlignment.MiddleLeft;
            }
            foreach (Label l in new[] { lblSalesValue, lblOrdersValue, lblCustomersValue, lblProductsValue })
            {
                l.BackColor = Color.Transparent;
                l.ForeColor = Brown;
                l.Font      = new Font("Segoe UI", 22F, FontStyle.Bold);
                l.AutoSize  = false;
                l.TextAlign = ContentAlignment.MiddleLeft;
            }

            void StyleCaption(Label lbl, string text)
            {
                lbl.Text      = text;
                lbl.Font      = new Font("Segoe UI", 8.5F);
                lbl.ForeColor = Color.FromArgb(140, 120, 100);
                lbl.AutoSize  = false;
                lbl.TextAlign = ContentAlignment.MiddleLeft;
                lbl.BackColor = Color.Transparent;
            }
            StyleCaption(lblSalesCaption,     "today's total sales");
            StyleCaption(lblOrdersCaption,    "orders today");
            StyleCaption(lblCustomersCaption, "customers today");
            StyleCaption(lblProductsCaption,  "products sold today");

            pnlSalesCard.Controls.Clear();
            pnlOrdersCard.Controls.Clear();
            pnlCustomersCard.Controls.Clear();
            pnlProductsCard.Controls.Clear();
            pnlSalesCard.Controls.Add(lblTotalSales);     pnlSalesCard.Controls.Add(lblSalesValue);     pnlSalesCard.Controls.Add(lblSalesCaption);
            pnlOrdersCard.Controls.Add(lblTotalOrders);   pnlOrdersCard.Controls.Add(lblOrdersValue);   pnlOrdersCard.Controls.Add(lblOrdersCaption);
            pnlCustomersCard.Controls.Add(lblTotalCustomers); pnlCustomersCard.Controls.Add(lblCustomersValue); pnlCustomersCard.Controls.Add(lblCustomersCaption);
            pnlProductsCard.Controls.Add(lblTotalProducts);  pnlProductsCard.Controls.Add(lblProductsValue);  pnlProductsCard.Controls.Add(lblProductsCaption);
            panelTodayStats.Controls.Clear();
            panelTodayStats.Controls.Add(pnlSalesCard);
            panelTodayStats.Controls.Add(pnlOrdersCard);
            panelTodayStats.Controls.Add(pnlCustomersCard);
            panelTodayStats.Controls.Add(pnlProductsCard);

            panelRevenueCard.BackColor   = CardBg;
            panelRevenueCard.Tag         = cardColor;
            panelRevenueCard.BorderStyle = BorderStyle.None;
            panelRevenueCard.Paint      += PaintCardShadow;

            // "Sales" title
            var lblRevTitle = new Label
            {
                Text      = "Sales",
                Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 53, 37),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(18, 14)
            };
            panelRevenueCard.Controls.Add(lblRevTitle);

            // Big total
            lblWeekBigTotal.Font      = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblWeekBigTotal.ForeColor = Brown;
            lblWeekBigTotal.BackColor = Color.Transparent;
            lblWeekBigTotal.AutoSize  = false;
            lblWeekBigTotal.Size      = new Size(300, 38);
            lblWeekBigTotal.Location  = new Point(18, 38);
            lblWeekBigTotal.Text      = "₱ 0.00";
            panelRevenueCard.Controls.Add(lblWeekBigTotal);

            void StyleTfBtn(Button b, string label, bool active)
            {
                b.Text      = label;
                b.Tag       = label;
                b.Size      = new Size(64, 26);
                b.FlatStyle = FlatStyle.Flat;
                b.Font      = new Font("Segoe UI", 8.5F);
                b.FlatAppearance.BorderColor = Color.FromArgb(200, 195, 190);
                b.BackColor = active ? Brown : Color.FromArgb(235, 235, 238);
                b.ForeColor = active ? Color.White : Brown;
                b.UseVisualStyleBackColor = false;
                b.Cursor    = Cursors.Hand;
            }
            StyleTfBtn(btnTfWeek,  "Week",  true);
            StyleTfBtn(btnTfMonth, "Month", false);
            StyleTfBtn(btnTfYear,  "Year",  false);

            void UpdateTfButtons()
            {
                btnTfWeek.BackColor  = _revenueTimeframe == "Week"  ? Brown : Color.FromArgb(235, 235, 238);
                btnTfWeek.ForeColor  = _revenueTimeframe == "Week"  ? Color.White : Brown;
                btnTfMonth.BackColor = _revenueTimeframe == "Month" ? Brown : Color.FromArgb(235, 235, 238);
                btnTfMonth.ForeColor = _revenueTimeframe == "Month" ? Color.White : Brown;
                btnTfYear.BackColor  = _revenueTimeframe == "Year"  ? Brown : Color.FromArgb(235, 235, 238);
                btnTfYear.ForeColor  = _revenueTimeframe == "Year"  ? Color.White : Brown;
            }

            void PopulateWeekDropdown()
            {
                cmbRevPeriod.Items.Clear();
                // last 8 weeks
                for (int w = 0; w >= -7; w--)
                {
                    DateTime mon = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1 + w * 7);
                    DateTime sun = mon.AddDays(6);
                    string label = w == 0 ? $"This Week ({mon:MMM d}–{sun:MMM d})"
                                          : $"{mon:MMM d} – {sun:MMM d}";
                    cmbRevPeriod.Items.Add(new PeriodItem(label, w));
                }
                cmbRevPeriod.SelectedIndex = 0;
            }

            void PopulateMonthDropdown()
            {
                cmbRevPeriod.Items.Clear();
                string[] months = { "January","February","March","April","May","June",
                                    "July","August","September","October","November","December" };
                for (int m = 1; m <= 12; m++)
                    cmbRevPeriod.Items.Add(new PeriodItem(months[m - 1], m));
                cmbRevPeriod.SelectedIndex = DateTime.Now.Month - 1;
            }

            void PopulateYearDropdown()
            {
                cmbRevPeriod.Items.Clear();
                int curYear = DateTime.Now.Year;
                for (int y = curYear; y >= 2020; y--)
                    cmbRevPeriod.Items.Add(new PeriodItem(y.ToString(), y));
                cmbRevPeriod.SelectedIndex = 0;
            }

            btnTfWeek.Click += (_, __) =>
            {
                _revenueTimeframe = "Week";
                UpdateTfButtons();
                PopulateWeekDropdown();
                cmbRevPeriod.Visible = true;
                LoadDashboardData();
            };
            btnTfMonth.Click += (_, __) =>
            {
                _revenueTimeframe = "Month";
                UpdateTfButtons();
                PopulateMonthDropdown();
                cmbRevPeriod.Visible = true;
                LoadDashboardData();
            };
            btnTfYear.Click += (_, __) =>
            {
                _revenueTimeframe = "Year";
                UpdateTfButtons();
                PopulateYearDropdown();
                cmbRevPeriod.Visible = true;
                LoadDashboardData();
            };

            cmbRevPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRevPeriod.Font          = new Font("Segoe UI", 8.5F);
            cmbRevPeriod.BackColor     = Color.White;
            cmbRevPeriod.FlatStyle     = FlatStyle.Flat;
            cmbRevPeriod.SelectedIndexChanged += (_, __) =>
            {
                if (cmbRevPeriod.SelectedItem is PeriodItem pi)
                {
                    if (_revenueTimeframe == "Week")  _revWeekOffset = pi.Value;
                    if (_revenueTimeframe == "Month") _revMonth      = pi.Value;
                    if (_revenueTimeframe == "Year")  _revYear       = pi.Value;
                    LoadDashboardData();
                }
            };

            panelRevenueCard.Controls.Add(btnTfWeek);
            panelRevenueCard.Controls.Add(btnTfMonth);
            panelRevenueCard.Controls.Add(btnTfYear);
            panelRevenueCard.Controls.Add(cmbRevPeriod);

            PopulateWeekDropdown();
            cmbRevPeriod.Visible = true;

            panelRevenueCanvas.BackColor = Color.White;
            panelRevenueCanvas.Paint    += DrawRevenueLineChart;
            panelRevenueCard.Controls.Add(panelRevenueCanvas);
            panelScroll.Controls.Add(panelRevenueCard);

            panelTopSelling.BackColor   = CardBg;
            panelTopSelling.Tag         = cardColor;
            panelTopSelling.BorderStyle = BorderStyle.None;
            panelTopSelling.Paint      += PaintCardShadow;

            var lblTopTitle = new Label
            {
                Text      = "Top Selling This Month",
                Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 53, 37),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(18, 14)
            };
            panelTopSelling.Controls.Add(lblTopTitle);

            panelTopSellingCanvas.BackColor = Color.Transparent;
            panelTopSellingCanvas.Location  = new Point(0, 50);
            panelTopSellingCanvas.Paint    += PaintTopSelling;
            panelTopSelling.Controls.Add(panelTopSellingCanvas);
            panelScroll.Controls.Add(panelTopSelling);

            panelRecentOrders.BackColor   = CardBg;
            panelRecentOrders.Tag         = cardColor;
            panelRecentOrders.BorderStyle = BorderStyle.None;
            panelRecentOrders.Paint      += PaintCardShadow;

            var lblRecentTitle = new Label
            {
                Text      = "Recent Orders",
                Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 53, 37),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(18, 14)
            };
            panelRecentOrders.Controls.Add(lblRecentTitle);

            ProductsForm.StyleDgv(dgvRecentOrders);
            dgvRecentOrders.Location        = new Point(18, 50);
            dgvRecentOrders.ReadOnly        = true;
            dgvRecentOrders.AllowUserToAddRows = false;
            dgvRecentOrders.Columns.Add("order_id",   "Order ID");
            dgvRecentOrders.Columns.Add("customer",   "Customer");
            dgvRecentOrders.Columns.Add("date",       "Date");
            dgvRecentOrders.Columns.Add("items",      "Items");
            dgvRecentOrders.Columns.Add("total",      "Total");
            dgvRecentOrders.Columns.Add("payment",    "Payment");
            dgvRecentOrders.Columns["order_id"].FillWeight  = 70;
            dgvRecentOrders.Columns["customer"].FillWeight  = 140;
            dgvRecentOrders.Columns["date"].FillWeight      = 90;
            dgvRecentOrders.Columns["items"].FillWeight     = 50;
            dgvRecentOrders.Columns["total"].FillWeight     = 80;
            dgvRecentOrders.Columns["payment"].FillWeight   = 80;
            panelRecentOrders.Controls.Add(dgvRecentOrders);
            panelScroll.Controls.Add(panelRecentOrders);

            panelTrackOrder.Visible  = false;
            panelNewOrder.Visible    = false;
            panelSalesWeek.Visible   = false;
            panelSalesTrend.Visible  = false;
            panelBestSellers.Visible = false;

            LayoutSidebar();
            ConfigureResponsiveLayout();
            this.Load += (_, __) => LayoutSidebar();
        }

        private void LayoutSidebar()
        {
            int w = panelSidebar.Width;

            int logoSize = 96;
            picLogo.Size     = new Size(logoSize, logoSize);
            picLogo.Location = new Point((w - logoSize) / 2, 12);
            using (var gp = new GraphicsPath())
            {
                gp.AddEllipse(0, 0, logoSize - 1, logoSize - 1);
                picLogo.Region = new Region(gp);
            }

            int menuTop = picLogo.Bottom + 16;
            int btnH    = 44;
            int gap     = 3;
            int xPad    = 8;
            int btnW    = w - xPad * 2;
            int y       = menuTop;
            for (int i = 0; i < _navButtons.Count; i++)
            {
                _navButtons[i].SetBounds(xPad, y, btnW, btnH);
                y += btnH + gap;
            }

            // Logout at bottom
            btnSideLogout.SetBounds(xPad, panelSidebar.Height - btnH - 12, btnW, btnH);

            panelSidebar.Invalidate();
        }

        private void ConfigureResponsiveLayout()
        {
            int pad = 24;

            lblTitle.Location    = new Point(18, 18);
            int btnY = lblTitle.Top + (lblTitle.PreferredHeight - btnNewOrder.Height) / 2;
            btnNewOrder.Location = new Point(lblTitle.Right + 20, btnY);

            // Profile on right: pic -> name/role stack → bell
            int topH   = panelTop.Height;
            int rightX = panelTop.Width - 16;

            lblBell.Location = new Point(rightX - lblBell.PreferredWidth, (topH - lblBell.PreferredHeight) / 2);
            rightX = lblBell.Left - 14;

            // Name + role stacked
            int nameW = Math.Max(lblTopName.PreferredWidth, lblTopRole.PreferredWidth) + 4;
            lblTopName.Location = new Point(rightX - nameW, (topH / 2) - lblTopName.PreferredHeight - 1);
            lblTopRole.Location = new Point(rightX - nameW, (topH / 2) + 1);
            rightX = lblTopName.Left - 8;

            picTopAdmin.Size     = new Size(38, 38);
            picTopAdmin.Location = new Point(rightX - 38, (topH - 38) / 2);
            using (var gp = new GraphicsPath())
            {
                gp.AddEllipse(0, 0, 37, 37);
                picTopAdmin.Region = new Region(gp);
            }

            //Scroll panel
            panelScroll.SetBounds(0, 0, panelContent.Width, panelContent.Height);
            int scrollW = panelScroll.Width - 18; // reserve scrollbar space
            int innerW  = scrollW - pad * 2;

            // Header
            lblWelcome.Location   = new Point(pad, 8);
            lblTodayDate.Location = new Point(scrollW - lblTodayDate.PreferredWidth - pad, 16);

            int statsTop = lblWelcome.Bottom + 18;
            int cardGap  = 16;
            int cardW    = (innerW - cardGap * 3) / 4;
            int cardH    = 160;
            panelTodayStats.SetBounds(pad, statsTop, innerW, cardH);
            pnlSalesCard.SetBounds(0,                        0, cardW, cardH);
            pnlOrdersCard.SetBounds(cardW + cardGap,         0, cardW, cardH);
            pnlCustomersCard.SetBounds((cardW + cardGap) * 2, 0, cardW, cardH);
            pnlProductsCard.SetBounds((cardW + cardGap) * 3, 0, cardW, cardH);

            int lp = 18;
            foreach (var (lbl, val, cap) in new[] {
                (lblTotalSales,     lblSalesValue,     lblSalesCaption),
                (lblTotalOrders,    lblOrdersValue,    lblOrdersCaption),
                (lblTotalCustomers, lblCustomersValue, lblCustomersCaption),
                (lblTotalProducts,  lblProductsValue,  lblProductsCaption) })
            {
                lbl.SetBounds(lp, 16, cardW - lp * 2, 22);
                val.SetBounds(lp, 44, cardW - lp * 2, 52);
                cap.SetBounds(lp, 108, cardW - lp * 2, 26);
            }

            int row2Top = panelTodayStats.Bottom + 24;
            int rightW  = Math.Max(260, innerW / 3);
            int leftW   = innerW - rightW - cardGap;
            int row2H   = 420;

            panelRevenueCard.SetBounds(pad, row2Top, leftW, row2H);

            int btnRowY  = 86;
            int btnH2    = 28;
            btnTfWeek.SetBounds(18,          btnRowY, 64, btnH2);
            btnTfMonth.SetBounds(18 + 68,    btnRowY, 64, btnH2);
            btnTfYear.SetBounds(18 + 68 * 2, btnRowY, 64, btnH2);
            cmbRevPeriod.SetBounds(18 + 68 * 3 + 6, btnRowY, Math.Max(120, leftW - (18 + 68 * 3 + 6) - 18), btnH2);

            int canvasTop = btnRowY + btnH2 + 10;
            panelRevenueCanvas.SetBounds(0, canvasTop, leftW, row2H - canvasTop - 8);
            panelRevenueCanvas.Invalidate();

            panelTopSelling.SetBounds(pad + leftW + cardGap, row2Top, rightW, row2H);
            panelTopSellingCanvas.SetBounds(0, 54, rightW, row2H - 62);
            panelTopSellingCanvas.Invalidate();

            int row3Top    = row2Top + row2H + 24;
            int dgvH       = Math.Max(280, dgvRecentOrders.Rows.Count * 32 + 50);
            int cardTotalH = dgvH + 80;
            panelRecentOrders.SetBounds(pad, row3Top, innerW, cardTotalH);
            dgvRecentOrders.SetBounds(18, 54, innerW - 36, dgvH);

            panelScroll.AutoScrollMinSize = new Size(0, row3Top + cardTotalH + 40);
        }

        private void LoadDashboardData()
        {
            lblTodayDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");

            try
            {
                using var conn = DatabaseConnection.GetConnection();

                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM orders WHERE order_date = CURDATE()", conn))
                    _todayOrders = Convert.ToInt32(cmd.ExecuteScalar());

                using (var cmd = new MySqlCommand("SELECT COUNT(DISTINCT customer_id) FROM orders WHERE order_date = CURDATE()", conn))
                    _todayCustomers = Convert.ToInt32(cmd.ExecuteScalar());

                using (var cmd = new MySqlCommand(
                    @"SELECT COALESCE(SUM(oi.quantity),0) FROM orderitem oi
                      JOIN orders o ON oi.order_id=o.order_id WHERE o.order_date=CURDATE()", conn))
                    _todayProducts = Convert.ToInt32(cmd.ExecuteScalar());

                using (var cmd = new MySqlCommand(
                    @"SELECT COALESCE(SUM(p.amount),0) FROM payment p
                      JOIN orders o ON p.order_id=o.order_id
                      WHERE YEARWEEK(o.order_date,1)=YEARWEEK(CURDATE(),1)", conn))
                    _weekTotalPeso = Convert.ToDecimal(cmd.ExecuteScalar());

                using (var cmd = new MySqlCommand(
                    @"SELECT COALESCE(SUM(p.amount),0) FROM payment p
                      JOIN orders o ON p.order_id=o.order_id WHERE o.order_date=CURDATE()", conn))
                    _todaySales = Convert.ToDecimal(cmd.ExecuteScalar());

                if (_revenueTimeframe == "Month")
                {
                    // Group by week-of-month (4 weeks) for selected month
                    _revenueByDay  = new decimal[4];
                    _revenueLabels = new[] { "Wk 1", "Wk 2", "Wk 3", "Wk 4" };
                    string monthSql = @"
                        SELECT CEIL(DAY(o.order_date)/7.0) AS wk,
                               COALESCE(SUM(p.amount),0) AS rev
                        FROM payment p JOIN orders o ON p.order_id=o.order_id
                        WHERE MONTH(o.order_date)=@mo
                          AND YEAR(o.order_date)=YEAR(CURDATE())
                        GROUP BY wk";
                    using (var cmd = new MySqlCommand(monthSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mo", _revMonth);
                        using var r = cmd.ExecuteReader();
                        while (r.Read())
                        {
                            int wk = Convert.ToInt32(r["wk"]);
                            int idx = Math.Min(wk - 1, 3);
                            if (idx >= 0) _revenueByDay[idx] = Convert.ToDecimal(r["rev"]);
                        }
                    }
                    _weekTotalPeso = 0; foreach (var v in _revenueByDay) _weekTotalPeso += v;
                }
                else if (_revenueTimeframe == "Year")
                {
                    // Group by month (12 months) for selected year
                    _revenueByDay  = new decimal[12];
                    _revenueLabels = new[] { "Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec" };
                    string yearSql = @"
                        SELECT MONTH(o.order_date) AS mo,
                               COALESCE(SUM(p.amount),0) AS rev
                        FROM payment p JOIN orders o ON p.order_id=o.order_id
                        WHERE YEAR(o.order_date)=@yr
                        GROUP BY mo";
                    using (var cmd = new MySqlCommand(yearSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@yr", _revYear);
                        using var r = cmd.ExecuteReader();
                        while (r.Read())
                        {
                            int mo = Convert.ToInt32(r["mo"]);
                            if (mo >= 1 && mo <= 12) _revenueByDay[mo - 1] = Convert.ToDecimal(r["rev"]);
                        }
                    }
                    _weekTotalPeso = 0; foreach (var v in _revenueByDay) _weekTotalPeso += v;
                }
                else // Week
                {
                    _revenueByDay  = new decimal[7];
                    _revenueLabels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
                    string weekSql = @"
                        SELECT DAYOFWEEK(o.order_date) AS dow,
                               COALESCE(SUM(p.amount),0) AS rev
                        FROM payment p JOIN orders o ON p.order_id=o.order_id
                        WHERE o.order_date >= DATE_ADD(
                                DATE_SUB(CURDATE(), INTERVAL WEEKDAY(CURDATE()) DAY),
                                INTERVAL @woff WEEK)
                          AND o.order_date <  DATE_ADD(
                                DATE_SUB(CURDATE(), INTERVAL WEEKDAY(CURDATE()) DAY),
                                INTERVAL (@woff + 1) WEEK)
                        GROUP BY dow";
                    using (var cmd = new MySqlCommand(weekSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@woff", _revWeekOffset);
                        using var r = cmd.ExecuteReader();
                        while (r.Read())
                        {
                            int dow = Convert.ToInt32(r["dow"]);
                            int idx = (dow == 1) ? 6 : dow - 2; // Sun=1→6, Mon=2→0 … Sat=7→5
                            if (idx >= 0 && idx < 7) _revenueByDay[idx] = Convert.ToDecimal(r["rev"]);
                        }
                    }
                    _weekTotalPeso = 0; foreach (var v in _revenueByDay) _weekTotalPeso += v;
                }

                lblSalesValue.Text     = "₱ " + _todaySales.ToString("N2");
                lblOrdersValue.Text    = _todayOrders.ToString();
                lblCustomersValue.Text = _todayCustomers.ToString();
                lblProductsValue.Text  = _todayProducts.ToString();
                lblWeekBigTotal.Text   = "₱ " + _weekTotalPeso.ToString("N2");

                LoadTopSellingToday(conn);
                LoadRecentOrders(conn);
            }
            catch
            {
                lblSalesValue.Text     = "₱ 0.00";
                lblOrdersValue.Text    = "0";
                lblCustomersValue.Text = "0";
                lblProductsValue.Text  = "0";
                lblWeekBigTotal.Text   = "₱ 0.00";
            }

            panelRevenueCanvas.Invalidate();
            panelTopSellingCanvas.Invalidate();
            ConfigureResponsiveLayout();
        }

        private void LoadTopSellingToday(MySqlConnection conn)
        {
            var list = new List<(string name, int sold, decimal price)>();
            try
            {
                using var cmd = new MySqlCommand(
                    @"SELECT p.product_name, SUM(oi.quantity) AS qty, p.price
                      FROM orderitem oi
                      JOIN orders o  ON oi.order_id  = o.order_id
                      JOIN product p ON oi.product_id = p.product_id
                      WHERE MONTH(o.order_date)=MONTH(CURDATE()) AND YEAR(o.order_date)=YEAR(CURDATE())
                      GROUP BY p.product_id, p.product_name, p.price
                      ORDER BY qty DESC LIMIT 5", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    list.Add((r["product_name"].ToString()!, Convert.ToInt32(r["qty"]), Convert.ToDecimal(r["price"])));
            }
            catch { }

            if (list.Count == 0)
            {
                try
                {
                    using var cmd = new MySqlCommand(
                        @"SELECT p.product_name, SUM(oi.quantity) AS qty, p.price
                          FROM orderitem oi JOIN product p ON oi.product_id=p.product_id
                          GROUP BY p.product_id, p.product_name, p.price
                          ORDER BY qty DESC LIMIT 5", conn);
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                        list.Add((r["product_name"].ToString()!, Convert.ToInt32(r["qty"]), Convert.ToDecimal(r["price"])));
                }
                catch { }
            }
            _topItems = list.ToArray();
        }

        private void LoadRecentOrders(MySqlConnection conn)
        {
            dgvRecentOrders.Rows.Clear();
            try
            {
                using var cmd = new MySqlCommand(
                    @"SELECT o.order_id,
                             CONCAT(c.fname,' ',c.lname) AS customer,
                             o.order_date,
                             COUNT(oi.product_id) AS items,
                             COALESCE(SUM(py.amount),0) AS total,
                             COALESCE(MAX(py.payment_method),'—') AS payment
                      FROM orders o
                      JOIN customer c  ON o.customer_id = c.customer_id
                      LEFT JOIN orderitem oi ON o.order_id = oi.order_id
                      LEFT JOIN payment py   ON o.order_id = py.order_id
                      WHERE o.order_date >= DATE_SUB(CURDATE(), INTERVAL WEEKDAY(CURDATE()) DAY)
                        AND o.order_date <  DATE_ADD(DATE_SUB(CURDATE(), INTERVAL WEEKDAY(CURDATE()) DAY), INTERVAL 7 DAY)
                      GROUP BY o.order_id, c.fname, c.lname, o.order_date
                      ORDER BY o.order_date DESC, o.order_id DESC
                      LIMIT 20", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    dgvRecentOrders.Rows.Add(
                        r["order_id"].ToString(),
                        r["customer"].ToString(),
                        Convert.ToDateTime(r["order_date"]).ToString("MMM dd, yyyy"),
                        r["items"].ToString(),
                        "₱ " + Convert.ToDecimal(r["total"]).ToString("N2"),
                        r["payment"].ToString());
            }
            catch { }
        }

        private void DrawRevenueLineChart(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.White);

            int w = panelRevenueCanvas.Width;
            int h = panelRevenueCanvas.Height;
            if (w < 20 || h < 20) return;

            var data   = _revenueByDay;
            var labels = _revenueLabels;
            int n      = data.Length;
            if (n < 2) return;

            int lm = 52, rm = 16, tm = 10, bm = 28;
            var area = new Rectangle(lm, tm, w - lm - rm, h - tm - bm);

            decimal maxRev = 0;
            foreach (var v in data) if (v > maxRev) maxRev = v;
            maxRev = Math.Max(maxRev, 500m);
            decimal yMax = Math.Ceiling(maxRev / 500m) * 500m;

            using var gridPen = new Pen(Color.FromArgb(230, 225, 218), 1);
            using var axisFont = new Font("Segoe UI", 7.5F);
            int gridCount = 4;
            for (int gi = 0; gi <= gridCount; gi++)
            {
                int gy = area.Bottom - (int)(area.Height * gi / (float)gridCount);
                g.DrawLine(gridPen, area.Left, gy, area.Right, gy);
                decimal v = yMax * gi / gridCount;
                string lbl = v >= 1000 ? (v / 1000m).ToString("0.#") + "K" : v.ToString("0");
                var sz = g.MeasureString(lbl, axisFont);
                g.DrawString(lbl, axisFont, Brushes.Gray, area.Left - sz.Width - 2, gy - sz.Height / 2);
            }

            var pts = new PointF[n];
            for (int i = 0; i < n; i++)
            {
                float x = area.Left + i * (area.Width / (float)(n - 1));
                float y = area.Bottom - (float)(data[i] / yMax) * area.Height;
                pts[i] = new PointF(x, y);
            }

            var fillPts = new PointF[n + 2];
            fillPts[0] = new PointF(pts[0].X, area.Bottom);
            for (int i = 0; i < n; i++) fillPts[i + 1] = pts[i];
            fillPts[n + 1] = new PointF(pts[n - 1].X, area.Bottom);
            using var fillBrush = new LinearGradientBrush(
                new Point(0, area.Top), new Point(0, area.Bottom),
                Color.FromArgb(80, 139, 90, 43), Color.FromArgb(5, 139, 90, 43));
            g.FillPolygon(fillBrush, fillPts);

            using var linePen = new Pen(Brown, 2.5f);
            g.DrawLines(linePen, pts);

            using var dotBrush = new SolidBrush(Brown);
            for (int i = 0; i < n; i++)
            {
                g.FillEllipse(dotBrush, pts[i].X - 4, pts[i].Y - 4, 8, 8);
                g.FillEllipse(Brushes.White, pts[i].X - 2, pts[i].Y - 2, 4, 4);
                string dayLbl = i < labels.Length ? labels[i] : "";
                var daySize = g.MeasureString(dayLbl, axisFont);
                g.DrawString(dayLbl, axisFont, Brushes.DimGray,
                    pts[i].X - daySize.Width / 2, area.Bottom + 4);
            }
        }

        private void PaintTopSelling(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var items = _topItems.Length > 0 ? _topItems : new[]
            {
                ("Espresso",        5, 85m),
                ("Latte",           4, 95m),
                ("Cappuccino",      3, 90m),
                ("Classic Lemonade",2, 60m),
                ("Pesto Pasta",     1, 150m),
            };

            int maxSold = 1;
            foreach (var (_, s, _) in items) if (s > maxSold) maxSold = s;

            int w    = panelTopSellingCanvas.Width;
            int rowH = Math.Max(44, (panelTopSellingCanvas.Height - 8) / Math.Max(items.Length, 1));

            using var nameFont  = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var subFont   = new Font("Segoe UI", 8.5F);
            using var rankFont  = new Font("Segoe UI", 9F, FontStyle.Bold);

            Color[] rankColors = {
                Color.FromArgb(212, 175, 55),
                Color.FromArgb(160, 160, 160),
                Color.FromArgb(176, 101, 55),
                Brown, Brown
            };

            for (int i = 0; i < items.Length; i++)
            {
                int y = i * rowH + 4;
                var (name, sold, price) = items[i];

                if (i % 2 == 0)
                {
                    using var rowBg = new SolidBrush(Color.FromArgb(18, 139, 90, 43));
                    FillRoundedRect(g, rowBg, new Rectangle(4, y, w - 8, rowH - 4), 6);
                }

                // Rank badge
                int bs = 26, bx = 8, by = y + (rowH - 4 - bs) / 2;
                using var bb = new SolidBrush(rankColors[Math.Min(i, rankColors.Length - 1)]);
                g.FillEllipse(bb, bx, by, bs, bs);
                string rk = (i + 1).ToString();
                var rkSz = g.MeasureString(rk, rankFont);
                g.DrawString(rk, rankFont, Brushes.White, bx + (bs - rkSz.Width) / 2, by + (bs - rkSz.Height) / 2);

                // Name
                int tx = bx + bs + 8;
                g.DrawString(name, nameFont, new SolidBrush(Color.FromArgb(55, 38, 22)), tx, y + 5);

                // Price + sold
                string info = $"₱{price:N2}  •  {sold} sold";
                g.DrawString(info, subFont, new SolidBrush(Color.FromArgb(120, 95, 70)), tx, y + 22);

                // Mini bar
                int barX = tx, barY = y + rowH - 10;
                int barMaxW = w - tx - 10;
                int barFill = maxSold > 0 ? (int)(barMaxW * sold / (float)maxSold) : 0;
                using var barBg = new SolidBrush(Color.FromArgb(35, 139, 90, 43));
                FillRoundedRect(g, barBg, new Rectangle(barX, barY, barMaxW, 5), 2);
                using var barFg = new SolidBrush(rankColors[Math.Min(i, rankColors.Length - 1)]);
                if (barFill > 0) FillRoundedRect(g, barFg, new Rectangle(barX, barY, barFill, 5), 2);
            }
        }

        private void PaintCardShadow(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel p) return;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int radius = 12;
            var rect = new Rectangle(4, 4, p.Width - 10, p.Height - 10);
            for (int i = 3; i >= 1; i--)
                using (var sh = new SolidBrush(Color.FromArgb(8 * i, 0, 0, 0)))
                    FillRoundedRect(g, sh, new Rectangle(rect.X + i, rect.Y + i, rect.Width, rect.Height), radius);
            Color fill = p.Tag is Color tc ? tc : CardBg;
            using var bg = new SolidBrush(fill);
            FillRoundedRect(g, bg, rect, radius);
        }

        //  NAV / MODULE HELPERS
        private void SelectNav(int index) { _selectedNavIndex = index; ApplyNavStyles(); }

        private void ApplyNavStyles()
        {
            for (int i = 0; i < _navButtons.Count; i++)
            {
                bool active = i == _selectedNavIndex;
                _navButtons[i].BackColor = active ? SidebarActive : SidebarIdle;
                _navButtons[i].ForeColor = active ? Color.White : Color.FromArgb(80, 70, 60);
                _navButtons[i].Font      = new Font("Segoe UI", 10F, active ? FontStyle.Bold : FontStyle.Regular);
                _navButtons[i].FlatAppearance.MouseOverBackColor =
                    active ? Color.FromArgb(120, 78, 36) : Color.FromArgb(235, 228, 218);
            }
        }

        private void LoadModuleIntoContent(Func<Form> createModule, string pageTitle = "")
        {
            _currentModule?.Dispose();
            _currentModule = null;
            panelContent.Controls.Clear();
            panelContent.BackColor = Color.FromArgb(245, 246, 250);

            var module = createModule();
            _currentModule = module;
            lblTitle.Text = string.IsNullOrEmpty(pageTitle) ? module.Text : pageTitle;
            btnNewOrder.Visible = false;

            var hostPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            panelContent.Controls.Add(hostPanel);

            var controls = new List<Control>();
            foreach (Control c in module.Controls) controls.Add(c);
            module.Controls.Clear();
            foreach (var c in controls) c.Parent = hostPanel;

            foreach (Control c in hostPanel.Controls)
            {
                if (c is Button b && b.Text == "Close") b.Visible = false;
                if (c is Label lbl && lbl.Font.Size >= 16f && lbl.AutoSize) lbl.Visible = false;
            }

            void DoLayout()
            {
                var method = module.GetType().GetMethod("UpdateLayout",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (method != null) { module.ClientSize = hostPanel.ClientSize; method.Invoke(module, null); }
            }
            hostPanel.Resize += (_, __) => DoLayout();
            hostPanel.HandleCreated += (_, __) => DoLayout();
            if (hostPanel.IsHandleCreated) DoLayout();
        }

        private void RestoreDashboardContent()
        {
            _currentModule?.Dispose();
            _currentModule = null;
            panelContent.Controls.Clear();
            panelContent.BackColor = Color.White;
            lblTitle.Text = "Dashboard";
            btnNewOrder.Visible = true;

            panelContent.Controls.Add(panelScroll);
            ConfigureResponsiveLayout();
            LoadDashboardData();
        }

        private void ShowWithBlur(Func<Form> createForm)
        {
            var overlay = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor       = Color.Black,
                Opacity         = 0.45,
                ShowInTaskbar   = false,
                StartPosition   = FormStartPosition.Manual,
                Owner           = this
            };
            overlay.Location = this.PointToScreen(Point.Empty);
            overlay.Size     = this.ClientSize;
            overlay.Show(this);
            try { using var dlg = createForm(); dlg.ShowDialog(this); }
            finally { overlay.Close(); overlay.Dispose(); this.Refresh(); }
        }

        //  EVENT HANDLERS
        private void btnLogout_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void btnReports_Click(object? sender, EventArgs e)
            => LoadModuleIntoContent(() => new ReportGeneratorForm());

        private void DashboardForm_Load(object? sender, EventArgs e) => LayoutSidebar();

        //  HELPERS
        private static void FillRoundedRect(Graphics g, Brush brush, Rectangle rect, int radius)
        {
            if (rect.Width <= 0 || rect.Height <= 0) return;
            using var path = RoundedRect(rect, radius);
            g.FillPath(brush, path);
        }

        private static GraphicsPath RoundedRect(Rectangle b, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(b.X,          b.Y,           d, d, 180, 90);
            path.AddArc(b.Right - d,  b.Y,           d, d, 270, 90);
            path.AddArc(b.Right - d,  b.Bottom - d,  d, d,   0, 90);
            path.AddArc(b.X,          b.Bottom - d,  d, d,  90, 90);
            path.CloseFigure();
            return path;
        }

        private sealed class PeriodItem
        {
            public string Label { get; }
            public int    Value { get; }
            public PeriodItem(string label, int value) { Label = label; Value = value; }
            public override string ToString() => Label;
        }
    }
}
