namespace BrewAndBiteCafe
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Button btnCustomers;
        private System.Windows.Forms.Button btnOrders;

        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Panel pnlSalesCard;
        private System.Windows.Forms.Panel pnlOrdersCard;
        private System.Windows.Forms.Panel pnlCustomersCard;
        private System.Windows.Forms.Panel pnlProductsCard;
        private System.Windows.Forms.Label lblTotalSales;
        private System.Windows.Forms.Label lblTotalOrders;
        private System.Windows.Forms.Label lblTotalCustomers;
        private System.Windows.Forms.Label lblTotalProducts;
        private System.Windows.Forms.Label lblSalesValue;
        private System.Windows.Forms.Label lblOrdersValue;
        private System.Windows.Forms.Label lblCustomersValue;
        private System.Windows.Forms.Label lblProductsValue;
        private System.Windows.Forms.Panel panelTodayStats;
        private System.Windows.Forms.Panel panelBestSellers;
        private System.Windows.Forms.ListView lvBestSellers;
        private System.Windows.Forms.ColumnHeader colRank;
        private System.Windows.Forms.ColumnHeader colItem;
        private System.Windows.Forms.ColumnHeader colSold;
        private System.Windows.Forms.Label lblBestSellers;
        private System.Windows.Forms.Panel panelSalesTrend;
        private System.Windows.Forms.Panel panelTrendCanvas;
        private System.Windows.Forms.Label lblSalesTrend;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.btnCustomers = new System.Windows.Forms.Button();
            this.btnOrders = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelBestSellers = new System.Windows.Forms.Panel();
            this.lvBestSellers = new System.Windows.Forms.ListView();
            this.colRank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSold = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblBestSellers = new System.Windows.Forms.Label();
            this.panelSalesTrend = new System.Windows.Forms.Panel();
            this.panelTrendCanvas = new System.Windows.Forms.Panel();
            this.lblSalesTrend = new System.Windows.Forms.Label();
            this.panelTodayStats = new System.Windows.Forms.Panel();
            this.pnlProductsCard = new System.Windows.Forms.Panel();
            this.lblTotalProducts = new System.Windows.Forms.Label();
            this.lblProductsValue = new System.Windows.Forms.Label();
            this.pnlCustomersCard = new System.Windows.Forms.Panel();
            this.lblTotalCustomers = new System.Windows.Forms.Label();
            this.lblCustomersValue = new System.Windows.Forms.Label();
            this.pnlOrdersCard = new System.Windows.Forms.Panel();
            this.lblTotalOrders = new System.Windows.Forms.Label();
            this.lblOrdersValue = new System.Windows.Forms.Label();
            this.pnlSalesCard = new System.Windows.Forms.Panel();
            this.lblTotalSales = new System.Windows.Forms.Label();
            this.lblSalesValue = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.panelSidebar.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelBestSellers.SuspendLayout();
            this.panelSalesTrend.SuspendLayout();
            this.panelTodayStats.SuspendLayout();
            this.pnlProductsCard.SuspendLayout();
            this.pnlCustomersCard.SuspendLayout();
            this.pnlOrdersCard.SuspendLayout();
            this.pnlSalesCard.SuspendLayout();
            this.SuspendLayout();
            
            // panelSidebar
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(40)))), ((int)(((byte)(20)))));
            this.panelSidebar.Controls.Add(this.btnDashboard);
            this.panelSidebar.Controls.Add(this.btnProducts);
            this.panelSidebar.Controls.Add(this.btnCustomers);
            this.panelSidebar.Controls.Add(this.btnOrders);
            this.panelSidebar.Controls.Add(this.btnReports);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(220, 720);
            this.panelSidebar.TabIndex = 0;
            
            // btnDashboard
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(90)))), ((int)(((byte)(43)))));
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(10, 24);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(200, 45);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "📊 Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = false;

            // btnProducts
            this.btnProducts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(60)))), ((int)(((byte)(40)))));
            this.btnProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProducts.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnProducts.ForeColor = System.Drawing.Color.White;
            this.btnProducts.Location = new System.Drawing.Point(10, 80);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(200, 45);
            this.btnProducts.TabIndex = 1;
            this.btnProducts.Text = "☕ Products";
            this.btnProducts.UseVisualStyleBackColor = false;
 
            // btnCustomers 
            this.btnCustomers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(60)))), ((int)(((byte)(40)))));
            this.btnCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomers.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCustomers.ForeColor = System.Drawing.Color.White;
            this.btnCustomers.Location = new System.Drawing.Point(10, 136);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(200, 45);
            this.btnCustomers.TabIndex = 2;
            this.btnCustomers.Text = "👥 Customers";
            this.btnCustomers.UseVisualStyleBackColor = false;
            
            // btnOrders
            this.btnOrders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(60)))), ((int)(((byte)(40)))));
            this.btnOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrders.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnOrders.ForeColor = System.Drawing.Color.White;
            this.btnOrders.Location = new System.Drawing.Point(10, 192);
            this.btnOrders.Name = "btnOrders";
            this.btnOrders.Size = new System.Drawing.Size(200, 45);
            this.btnOrders.TabIndex = 3;
            this.btnOrders.Text = "🧾 Orders";
            this.btnOrders.UseVisualStyleBackColor = false;

            // btnReports
            this.btnReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(60)))), ((int)(((byte)(40)))));
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnReports.ForeColor = System.Drawing.Color.White;
            this.btnReports.Location = new System.Drawing.Point(10, 304);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(200, 45);
            this.btnReports.TabIndex = 5;
            this.btnReports.Text = "📈 Reports";
            this.btnReports.UseVisualStyleBackColor = false;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
          
            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.lblUser);
            this.panelTop.Controls.Add(this.btnLogout);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(220, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1060, 70);
            this.panelTop.TabIndex = 1;
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(60)))), ((int)(((byte)(40)))));
            this.lblTitle.Location = new System.Drawing.Point(24, 19);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(100, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Dashboard";
            
            // lblUser
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUser.ForeColor = System.Drawing.Color.Gray;
            this.lblUser.Location = new System.Drawing.Point(780, 26);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(100, 20);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "Logged in: Admin";
            
            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(940, 20);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 30);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            
            // panelContent
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(247, 247, 247);
            this.panelContent.Controls.Add(this.panelBestSellers);
            this.panelContent.Controls.Add(this.panelSalesTrend);
            this.panelContent.Controls.Add(this.panelTodayStats);
            this.panelContent.Controls.Add(this.lblWelcome);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(220, 70);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1060, 650);
            this.panelContent.TabIndex = 2;
            
            // panelBestSellers
            this.panelBestSellers.BackColor = System.Drawing.Color.White;
            this.panelBestSellers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBestSellers.Controls.Add(this.lvBestSellers);
            this.panelBestSellers.Controls.Add(this.lblBestSellers);
            this.panelBestSellers.Location = new System.Drawing.Point(708, 214);
            this.panelBestSellers.Name = "panelBestSellers";
            this.panelBestSellers.Size = new System.Drawing.Size(326, 408);
            this.panelBestSellers.TabIndex = 3;
            
            // lvBestSellers
            this.lvBestSellers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvBestSellers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRank,
            this.colItem,
            this.colSold});
            this.lvBestSellers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lvBestSellers.FullRowSelect = true;
            this.lvBestSellers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvBestSellers.HideSelection = false;
            this.lvBestSellers.Location = new System.Drawing.Point(16, 56);
            this.lvBestSellers.Name = "lvBestSellers";
            this.lvBestSellers.Size = new System.Drawing.Size(292, 332);
            this.lvBestSellers.TabIndex = 1;
            this.lvBestSellers.UseCompatibleStateImageBehavior = false;
            this.lvBestSellers.View = System.Windows.Forms.View.Details;
            
            // colRank
            this.colRank.Text = "#";
            this.colRank.Width = 34;
            
            // colItem
            this.colItem.Text = "Best Seller Today";
            this.colItem.Width = 183;
            
            // colSold
            this.colSold.Text = "Sold";
            this.colSold.Width = 68;
            
            // lblBestSellers
            this.lblBestSellers.AutoSize = true;
            this.lblBestSellers.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblBestSellers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(60)))), ((int)(((byte)(40)))));
            this.lblBestSellers.Location = new System.Drawing.Point(12, 16);
            this.lblBestSellers.Name = "lblBestSellers";
            this.lblBestSellers.Size = new System.Drawing.Size(209, 25);
            this.lblBestSellers.TabIndex = 0;
            this.lblBestSellers.Text = "Top Selling Items Today";
            
            // panelSalesTrend
            this.panelSalesTrend.BackColor = System.Drawing.Color.White;
            this.panelSalesTrend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSalesTrend.Controls.Add(this.panelTrendCanvas);
            this.panelSalesTrend.Controls.Add(this.lblSalesTrend);
            this.panelSalesTrend.Location = new System.Drawing.Point(24, 214);
            this.panelSalesTrend.Name = "panelSalesTrend";
            this.panelSalesTrend.Size = new System.Drawing.Size(662, 408);
            this.panelSalesTrend.TabIndex = 2;
            
            // panelTrendCanvas
            this.panelTrendCanvas.BackColor = System.Drawing.Color.White;
            this.panelTrendCanvas.Location = new System.Drawing.Point(16, 56);
            this.panelTrendCanvas.Name = "panelTrendCanvas";
            this.panelTrendCanvas.Size = new System.Drawing.Size(628, 332);
            this.panelTrendCanvas.TabIndex = 1;
            
            // lblSalesTrend
            this.lblSalesTrend.AutoSize = true;
            this.lblSalesTrend.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblSalesTrend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(60)))), ((int)(((byte)(40)))));
            this.lblSalesTrend.Location = new System.Drawing.Point(12, 16);
            this.lblSalesTrend.Name = "lblSalesTrend";
            this.lblSalesTrend.Size = new System.Drawing.Size(215, 25);
            this.lblSalesTrend.TabIndex = 0;
            this.lblSalesTrend.Text = "Today's Hourly Sales (₱)";
            
            // panelTodayStats
            this.panelTodayStats.BackColor = System.Drawing.Color.Transparent;
            this.panelTodayStats.Controls.Add(this.pnlProductsCard);
            this.panelTodayStats.Controls.Add(this.pnlCustomersCard);
            this.panelTodayStats.Controls.Add(this.pnlOrdersCard);
            this.panelTodayStats.Controls.Add(this.pnlSalesCard);
            this.panelTodayStats.Location = new System.Drawing.Point(24, 78);
            this.panelTodayStats.Name = "panelTodayStats";
            this.panelTodayStats.Size = new System.Drawing.Size(1010, 122);
            this.panelTodayStats.TabIndex = 1;
            
            // pnlProductsCard
            this.pnlProductsCard.BackColor = System.Drawing.Color.White;
            this.pnlProductsCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProductsCard.Controls.Add(this.lblTotalProducts);
            this.pnlProductsCard.Controls.Add(this.lblProductsValue);
            this.pnlProductsCard.Location = new System.Drawing.Point(768, 10);
            this.pnlProductsCard.Name = "pnlProductsCard";
            this.pnlProductsCard.Size = new System.Drawing.Size(230, 100);
            this.pnlProductsCard.TabIndex = 3;
            // 
            this.lblTotalProducts.AutoSize = true;
            this.lblTotalProducts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalProducts.ForeColor = System.Drawing.Color.Gray;
            this.lblTotalProducts.Location = new System.Drawing.Point(14, 14);
            this.lblTotalProducts.Name = "lblTotalProducts";
            this.lblTotalProducts.Size = new System.Drawing.Size(116, 19);
            this.lblTotalProducts.TabIndex = 0;
            this.lblTotalProducts.Text = "Products Sold Today";
            // 
            this.lblProductsValue.AutoSize = true;
            this.lblProductsValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblProductsValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(120)))), ((int)(((byte)(50)))));
            this.lblProductsValue.Location = new System.Drawing.Point(11, 43);
            this.lblProductsValue.Name = "lblProductsValue";
            this.lblProductsValue.Size = new System.Drawing.Size(47, 37);
            this.lblProductsValue.TabIndex = 1;
            this.lblProductsValue.Text = "38";
            
            // pnlCustomersCard
            this.pnlCustomersCard.BackColor = System.Drawing.Color.White;
            this.pnlCustomersCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCustomersCard.Controls.Add(this.lblTotalCustomers);
            this.pnlCustomersCard.Controls.Add(this.lblCustomersValue);
            this.pnlCustomersCard.Location = new System.Drawing.Point(514, 10);
            this.pnlCustomersCard.Name = "pnlCustomersCard";
            this.pnlCustomersCard.Size = new System.Drawing.Size(230, 100);
            this.pnlCustomersCard.TabIndex = 2;
            // 
            this.lblTotalCustomers.AutoSize = true;
            this.lblTotalCustomers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalCustomers.ForeColor = System.Drawing.Color.Gray;
            this.lblTotalCustomers.Location = new System.Drawing.Point(14, 14);
            this.lblTotalCustomers.Name = "lblTotalCustomers";
            this.lblTotalCustomers.Size = new System.Drawing.Size(124, 19);
            this.lblTotalCustomers.TabIndex = 0;
            this.lblTotalCustomers.Text = "Customers Today";
            // 
            this.lblCustomersValue.AutoSize = true;
            this.lblCustomersValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCustomersValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(160)))), ((int)(((byte)(60)))));
            this.lblCustomersValue.Location = new System.Drawing.Point(11, 43);
            this.lblCustomersValue.Name = "lblCustomersValue";
            this.lblCustomersValue.Size = new System.Drawing.Size(47, 37);
            this.lblCustomersValue.TabIndex = 1;
            this.lblCustomersValue.Text = "26";
            
            // pnlOrdersCard
            this.pnlOrdersCard.BackColor = System.Drawing.Color.White;
            this.pnlOrdersCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOrdersCard.Controls.Add(this.lblTotalOrders);
            this.pnlOrdersCard.Controls.Add(this.lblOrdersValue);
            this.pnlOrdersCard.Location = new System.Drawing.Point(260, 10);
            this.pnlOrdersCard.Name = "pnlOrdersCard";
            this.pnlOrdersCard.Size = new System.Drawing.Size(230, 100);
            this.pnlOrdersCard.TabIndex = 1;
            // 
            this.lblTotalOrders.AutoSize = true;
            this.lblTotalOrders.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalOrders.ForeColor = System.Drawing.Color.Gray;
            this.lblTotalOrders.Location = new System.Drawing.Point(14, 14);
            this.lblTotalOrders.Name = "lblTotalOrders";
            this.lblTotalOrders.Size = new System.Drawing.Size(95, 19);
            this.lblTotalOrders.TabIndex = 0;
            this.lblTotalOrders.Text = "Orders Today";
            // 
            this.lblOrdersValue.AutoSize = true;
            this.lblOrdersValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblOrdersValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblOrdersValue.Location = new System.Drawing.Point(11, 43);
            this.lblOrdersValue.Name = "lblOrdersValue";
            this.lblOrdersValue.Size = new System.Drawing.Size(47, 37);
            this.lblOrdersValue.TabIndex = 1;
            this.lblOrdersValue.Text = "32";
         
            // pnlSalesCard
            this.pnlSalesCard.BackColor = System.Drawing.Color.White;
            this.pnlSalesCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSalesCard.Controls.Add(this.lblTotalSales);
            this.pnlSalesCard.Controls.Add(this.lblSalesValue);
            this.pnlSalesCard.Location = new System.Drawing.Point(6, 10);
            this.pnlSalesCard.Name = "pnlSalesCard";
            this.pnlSalesCard.Size = new System.Drawing.Size(230, 100);
            this.pnlSalesCard.TabIndex = 0;
            // 
            this.lblTotalSales.AutoSize = true;
            this.lblTotalSales.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalSales.ForeColor = System.Drawing.Color.Gray;
            this.lblTotalSales.Location = new System.Drawing.Point(14, 14);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Size = new System.Drawing.Size(88, 19);
            this.lblTotalSales.TabIndex = 0;
            this.lblTotalSales.Text = "Sales Today";
            // 
            this.lblSalesValue.AutoSize = true;
            this.lblSalesValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblSalesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(90)))), ((int)(((byte)(43)))));
            this.lblSalesValue.Location = new System.Drawing.Point(11, 43);
            this.lblSalesValue.Name = "lblSalesValue";
            this.lblSalesValue.Size = new System.Drawing.Size(140, 37);
            this.lblSalesValue.TabIndex = 1;
            this.lblSalesValue.Text = "₱ 4,820.00";
            
            // lblWelcome
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(60)))), ((int)(((byte)(40)))));
            this.lblWelcome.Location = new System.Drawing.Point(20, 22);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(338, 30);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Dashboard Snapshot (Today)";
            
            // DashboardForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Text = "Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelSidebar);
            this.panelSidebar.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.panelBestSellers.ResumeLayout(false);
            this.panelBestSellers.PerformLayout();
            this.panelSalesTrend.ResumeLayout(false);
            this.panelSalesTrend.PerformLayout();
            this.panelTodayStats.ResumeLayout(false);
            this.pnlProductsCard.ResumeLayout(false);
            this.pnlProductsCard.PerformLayout();
            this.pnlCustomersCard.ResumeLayout(false);
            this.pnlCustomersCard.PerformLayout();
            this.pnlOrdersCard.ResumeLayout(false);
            this.pnlOrdersCard.PerformLayout();
            this.pnlSalesCard.ResumeLayout(false);
            this.pnlSalesCard.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}