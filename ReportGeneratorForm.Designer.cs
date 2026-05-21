namespace BrewAndBiteCafe
{
    partial class ReportGeneratorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.ComboBox cmbReportType;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.Label lblTotalSales;
        private System.Windows.Forms.Label lblTotalRevenue;
        private System.Windows.Forms.Label lblBestSeller;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Panel pnlCard;
        
        private System.Windows.Forms.Panel panelOptions    = new System.Windows.Forms.Panel();
        private System.Windows.Forms.CheckBox chkIncludeCharts  = new System.Windows.Forms.CheckBox();
        private System.Windows.Forms.CheckBox chkIncludeSummary = new System.Windows.Forms.CheckBox();
        private System.Windows.Forms.Panel panelSummary    = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Label lblSummaryTitle = new System.Windows.Forms.Label();
        private System.Windows.Forms.Label lblSummaryBody  = new System.Windows.Forms.Label();
        private System.Windows.Forms.Panel panelChartHost  = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Panel panelChartCanvas = new System.Windows.Forms.Panel();
        
        private System.Windows.Forms.Label lblTotalAmount  = new System.Windows.Forms.Label();

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMain      = new System.Windows.Forms.Panel();
            this.lblTitle       = new System.Windows.Forms.Label();
            this.lblReportType  = new System.Windows.Forms.Label();
            this.cmbReportType  = new System.Windows.Forms.ComboBox();
            this.lblDateFrom    = new System.Windows.Forms.Label();
            this.dtpFrom        = new System.Windows.Forms.DateTimePicker();
            this.lblDateTo      = new System.Windows.Forms.Label();
            this.dtpTo          = new System.Windows.Forms.DateTimePicker();
            this.btnGenerate    = new System.Windows.Forms.Button();
            this.btnExport      = new System.Windows.Forms.Button();
            this.btnClose       = new System.Windows.Forms.Button();
            this.dgvReport      = new System.Windows.Forms.DataGridView();
            this.lblTotalRecords = new System.Windows.Forms.Label();
            this.lblTotalSales   = new System.Windows.Forms.Label();
            this.lblTotalRevenue = new System.Windows.Forms.Label();
            this.lblBestSeller   = new System.Windows.Forms.Label();
            this.lblSummary      = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();

            // panelMain
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location  = new System.Drawing.Point(0, 0);
            this.panelMain.Name      = "panelMain";
            this.panelMain.Size      = new System.Drawing.Size(1100, 700);
            this.panelMain.TabIndex  = 0;

            // lblTitle
            this.lblTitle.AutoSize  = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(74, 53, 37);
            this.lblTitle.Location  = new System.Drawing.Point(96, 18);
            this.lblTitle.Name      = "lblTitle";
            this.lblTitle.TabIndex  = 0;
            this.lblTitle.Text      = "Sales Report";

            // pnlCard
            this.pnlCard = new System.Windows.Forms.Panel();
            this.pnlCard.BackColor = System.Drawing.Color.White;
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Padding = new System.Windows.Forms.Padding(0);

            // lblReportType
            this.lblReportType.AutoSize  = true;
            this.lblReportType.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblReportType.ForeColor = System.Drawing.Color.FromArgb(100, 85, 70);
            this.lblReportType.Location  = new System.Drawing.Point(96, 66);
            this.lblReportType.Name      = "lblReportType";
            this.lblReportType.TabIndex  = 1;
            this.lblReportType.Text      = "Report Type:";

            // cmbReportType — only Sales Report
            this.cmbReportType.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportType.Font              = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Items.Add("Sales Report");
            this.cmbReportType.Items.Add("Inventory Report");
            this.cmbReportType.Items.Add("Order Report");
            this.cmbReportType.SelectedIndex = 0;
            this.cmbReportType.SelectedIndexChanged += new System.EventHandler((s, e) => {
                this.lblTitle.Text = this.cmbReportType.SelectedItem?.ToString() ?? "Report Generator";
            });
            this.cmbReportType.Location          = new System.Drawing.Point(96, 86);
            this.cmbReportType.Name              = "cmbReportType";
            this.cmbReportType.Size              = new System.Drawing.Size(220, 28);
            this.cmbReportType.TabIndex          = 2;

            // lblDateFrom
            this.lblDateFrom.AutoSize  = true;
            this.lblDateFrom.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDateFrom.ForeColor = System.Drawing.Color.FromArgb(100, 85, 70);
            this.lblDateFrom.Location  = new System.Drawing.Point(336, 66);
            this.lblDateFrom.Name      = "lblDateFrom";
            this.lblDateFrom.TabIndex  = 3;
            this.lblDateFrom.Text      = "From:";

            // dtpFrom
            this.dtpFrom.Font     = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFrom.Format   = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(336, 86);
            this.dtpFrom.Name     = "dtpFrom";
            this.dtpFrom.Size     = new System.Drawing.Size(200, 28);
            this.dtpFrom.TabIndex = 4;

            // lblDateTo
            this.lblDateTo.AutoSize  = true;
            this.lblDateTo.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDateTo.ForeColor = System.Drawing.Color.FromArgb(100, 85, 70);
            this.lblDateTo.Location  = new System.Drawing.Point(556, 66);
            this.lblDateTo.Name      = "lblDateTo";
            this.lblDateTo.TabIndex  = 5;
            this.lblDateTo.Text      = "To:";

            // dtpTo
            this.dtpTo.Font     = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpTo.Format   = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(556, 86);
            this.dtpTo.Name     = "dtpTo";
            this.dtpTo.Size     = new System.Drawing.Size(200, 28);
            this.dtpTo.TabIndex = 6;

            // btnGenerate
            this.btnGenerate.BackColor            = System.Drawing.Color.FromArgb(139, 90, 43);
            this.btnGenerate.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Font                 = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.ForeColor            = System.Drawing.Color.White;
            this.btnGenerate.Location             = new System.Drawing.Point(0, 0);
            this.btnGenerate.Name                 = "btnGenerate";
            this.btnGenerate.Size                 = new System.Drawing.Size(140, 34);
            this.btnGenerate.TabIndex             = 7;
            this.btnGenerate.Text                 = "🔍 Generate";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click               += new System.EventHandler(this.btnGenerate_Click);

            // dgvReport
            this.dgvReport.AllowUserToAddRows    = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.BackgroundColor       = System.Drawing.Color.White;
            this.dgvReport.BorderStyle           = System.Windows.Forms.BorderStyle.None;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvReport.Location              = new System.Drawing.Point(0, 0);
            this.dgvReport.Name                  = "dgvReport";
            this.dgvReport.ReadOnly              = true;
            this.dgvReport.Size                  = new System.Drawing.Size(1040, 420);
            this.dgvReport.TabIndex              = 8;

            // lblTotalRecords
            this.lblTotalRecords.AutoSize  = false;
            this.lblTotalRecords.Visible   = false;
            this.lblTotalRecords.Size      = new System.Drawing.Size(0, 0);

            // lblTotalSales — stub
            this.lblTotalSales.AutoSize  = false;
            this.lblTotalSales.Visible   = false;
            this.lblTotalSales.Size      = new System.Drawing.Size(0, 0);

            // lblTotalRevenue — stub
            this.lblTotalRevenue.AutoSize  = false;
            this.lblTotalRevenue.Visible   = false;
            this.lblTotalRevenue.Size      = new System.Drawing.Size(0, 0);

            // lblBestSeller — stub
            this.lblBestSeller.AutoSize  = false;
            this.lblBestSeller.Visible   = false;
            this.lblBestSeller.Size      = new System.Drawing.Size(0, 0);

            // lblSummary
            this.lblSummary.AutoSize   = false;
            this.lblSummary.BackColor  = System.Drawing.Color.FromArgb(247, 244, 238);
            this.lblSummary.Font       = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblSummary.ForeColor  = System.Drawing.Color.FromArgb(74, 53, 37);
            this.lblSummary.Location   = new System.Drawing.Point(0, 0);
            this.lblSummary.Name       = "lblSummary";
            this.lblSummary.Padding    = new System.Windows.Forms.Padding(16, 10, 0, 0);
            this.lblSummary.TabIndex   = 9;
            this.lblSummary.Text       = "";
            this.lblSummary.TextAlign  = System.Drawing.ContentAlignment.TopLeft;

            // btnExport (Print) — black
            this.btnExport.BackColor            = System.Drawing.Color.FromArgb(40, 40, 40);
            this.btnExport.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font                 = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor            = System.Drawing.Color.White;
            this.btnExport.Location             = new System.Drawing.Point(0, 0);
            this.btnExport.Name                 = "btnExport";
            this.btnExport.Size                 = new System.Drawing.Size(150, 34);
            this.btnExport.TabIndex             = 12;
            this.btnExport.Text = "📊 Export";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click               += new System.EventHandler(this.btnExport_Click);

            // btnClose
            this.btnClose.BackColor            = System.Drawing.Color.FromArgb(140, 130, 120);
            this.btnClose.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font                 = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor            = System.Drawing.Color.White;
            this.btnClose.Location             = new System.Drawing.Point(0, 0);
            this.btnClose.Name                 = "btnClose";
            this.btnClose.Size                 = new System.Drawing.Size(100, 34);
            this.btnClose.TabIndex             = 13;
            this.btnClose.Text                 = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click               += new System.EventHandler(this.btnClose_Click);

            // Wire up card panel
            this.pnlCard.Controls.Add(this.dgvReport);
            this.pnlCard.Controls.Add(this.lblTotalRecords);
            this.pnlCard.Controls.Add(this.lblTotalSales);
            this.pnlCard.Controls.Add(this.lblTotalRevenue);
            this.pnlCard.Controls.Add(this.lblBestSeller);
            this.pnlCard.Controls.Add(this.lblSummary);

            // Wire up panelMain
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Controls.Add(this.lblReportType);
            this.panelMain.Controls.Add(this.cmbReportType);
            this.panelMain.Controls.Add(this.lblDateFrom);
            this.panelMain.Controls.Add(this.dtpFrom);
            this.panelMain.Controls.Add(this.lblDateTo);
            this.panelMain.Controls.Add(this.dtpTo);
            this.panelMain.Controls.Add(this.btnGenerate);
            this.panelMain.Controls.Add(this.pnlCard);
            this.panelMain.Controls.Add(this.btnExport);

            // ReportGeneratorForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1100, 580);
            this.FormBorderStyle     = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox         = true;
            this.MinimumSize         = new System.Drawing.Size(900, 500);
            this.StartPosition       = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text                = "Report Generator";
            this.WindowState         = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.Add(this.panelMain);

            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
