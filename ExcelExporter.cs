using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using XL = Microsoft.Office.Interop.Excel;

namespace BrewAndBiteCafe
{
    public static class ExcelExporter
    {
        private const string CompanyName    = "Brew & Bite Cafe";
        private const string CompanyTagline = "Sales & Inventory Management System";

        //Brown palette
        private static readonly XLColor BrownDark   = XLColor.FromArgb(74,  53,  37);
        private static readonly XLColor BrownMid    = XLColor.FromArgb(139, 90,  43);
        private static readonly XLColor BrownLight  = XLColor.FromArgb(55,  40,  25);

        //  PUBLIC EXPORT METHODS
        // dgv cols: order_id | order_date | amount | payment_method
        public static void ExportSales(DataGridView dgv, string path, string summaryText)
        {
            string[] cols = { "Order ID", "Order Date", "Amount (₱)", "Payment Method" };
            BuildSheet1(dgv, path, "Sales Report", cols, summaryText, stockColorCol: -1);
            var data = Aggregate(dgv, labelCol: 1, valueCol: 2);
            AddChart(path, "Daily Sales", "Order Date", "Amount (₱)", data, isBar: false);
        }

        // dgv cols: product_id | product_name | category | price | stock | status
        public static void ExportInventory(DataGridView dgv, string path, string summaryText)
        {
            string[] cols = { "Product ID", "Product Name", "Category", "Price (₱)", "Stock", "Status" };
            BuildSheet1(dgv, path, "Inventory Report", cols, summaryText, stockColorCol: 5);
            var data = Aggregate(dgv, labelCol: 2, valueCol: 4);
            AddChart(path, "Stock Level by Category", "Category", "Total Stock", data, isBar: true);
        }

        // dgv cols: order_id | customer | product | quantity | amount | order_date
        public static void ExportOrders(DataGridView dgv, string path, string summaryText)
        {
            string[] cols = { "Order ID", "Customer", "Product", "Quantity", "Amount (₱)", "Order Date" };
            BuildSheet1(dgv, path, "Order Report", cols, summaryText, stockColorCol: -1);
            var data = Aggregate(dgv, labelCol: 2, valueCol: 3);
            AddChart(path, "Quantity Sold by Product", "Product", "Total Qty Sold", data, isBar: false);
        }

        //  SHEET 1
        private static void BuildSheet1(DataGridView dgv, string path,
            string reportTitle, string[] cols, string summaryText, int stockColorCol)
        {
            using var wb = new XLWorkbook();
            var ws = wb.AddWorksheet(reportTitle);

            int dataStart = WriteHeader(ws, reportTitle, cols);
            int row = dataStart;

            foreach (DataGridViewRow dr in dgv.Rows)
            {
                for (int c = 0; c < cols.Length; c++)
                {
                    string raw = dr.Cells[c].Value?.ToString() ?? "";
                    // Write as number if parseable (removes apostrophe/text storage)
                    string clean = raw.Replace("₱", "").Replace(",", "").Trim();
                    if (double.TryParse(clean, out double num))
                    {
                        ws.Cell(row, c + 1).Value = num;
                        // Format amount columns with 2 decimal places
                        if (raw.Contains(".") || raw.Contains("₱"))
                            ws.Cell(row, c + 1).Style.NumberFormat.Format = "#,##0.00";
                    }
                    else
                    {
                        ws.Cell(row, c + 1).Value = raw;
                    }
                }

                if (stockColorCol >= 0)
                {
                    string st = dr.Cells[stockColorCol].Value?.ToString() ?? "";
                    if (st == "Low Stock")
                        ws.Row(row).Style.Font.FontColor = XLColor.FromArgb(180, 100, 0);
                    else if (st == "Out of Stock")
                        ws.Row(row).Style.Font.FontColor = XLColor.FromArgb(180, 50, 50);
                }
                row++;
            }

            StyleRows(ws, dataStart, row - 1, cols.Length);
            int sumEnd = WriteSummary(ws, row + 1, summaryText);
            WriteSignature(ws, sumEnd + 3);
            FinishSheet(ws);
            wb.SaveAs(path);
        }

        private static int WriteHeader(IXLWorksheet ws, string reportTitle, string[] cols)
        {
            int n = cols.Length; // total data columns

            // Row heights
            ws.Row(1).Height = 22;
            ws.Row(2).Height = 16;
            ws.Row(3).Height = 18;
            ws.Row(4).Height = 14;
            ws.Row(5).Height = 7;  
            ws.Row(6).Height = 6;   
            ws.Row(7).Height = 20;  

            // Col A = logo column, fixed width
            ws.Column(1).Width = 10;  // wide enough for logo without covering col B text

            // Logo
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "brew&bite.jpg");
            if (!File.Exists(logoPath))
                logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "coffee1.png");
            if (File.Exists(logoPath))
            {
                try
                {
                    var pic = ws.AddPicture(logoPath);
                    pic.MoveTo(ws.Cell(1, 1), new System.Drawing.Point(2, 2));
                    pic.Width  = 75;
                    pic.Height = 75;
                }
                catch { }
            }

            var c1 = ws.Cell(1, 2);
            c1.Value = CompanyName;
            c1.Style.Font.Bold      = true;
            c1.Style.Font.FontSize  = 20;
            c1.Style.Font.FontColor = BrownDark;
            c1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            if (n >= 2) ws.Range(1, 2, 1, n).Merge();

            var c2 = ws.Cell(2, 2);
            c2.Value = CompanyTagline;
            c2.Style.Font.Italic    = true;
            c2.Style.Font.FontSize  = 10;
            c2.Style.Font.FontColor = BrownMid;
            c2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            if (n >= 2) ws.Range(2, 2, 2, n).Merge();

            var c3 = ws.Cell(3, 2);
            c3.Value = reportTitle;
            c3.Style.Font.Bold      = true;
            c3.Style.Font.FontSize  = 13;
            c3.Style.Font.FontColor = BrownLight;
            c3.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            if (n >= 2) ws.Range(3, 2, 3, n).Merge();

            var c4 = ws.Cell(4, 2);
            c4.Value = $"Date Generated: {DateTime.Now:MMMM dd, yyyy}";
            c4.Style.Font.FontSize  = 9;
            c4.Style.Font.FontColor = XLColor.FromArgb(85, 85, 85);
            c4.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            if (n >= 2) ws.Range(4, 2, 4, n).Merge();

            ws.Range(5, 1, 5, n).Style.Fill.BackgroundColor = BrownMid;

            for (int c = 0; c < cols.Length; c++)
            {
                var cell = ws.Cell(7, c + 1);
                cell.Value = cols[c];
                cell.Style.Font.Bold            = true;
                cell.Style.Font.FontSize        = 10;
                cell.Style.Font.FontColor       = XLColor.Black;
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);
                cell.Style.Border.BottomBorder      = XLBorderStyleValues.Medium;
                cell.Style.Border.BottomBorderColor = XLColor.FromArgb(120, 120, 120);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                cell.Style.Alignment.Vertical   = XLAlignmentVerticalValues.Center;
            }

            return 8;
        }

        private static void StyleRows(IXLWorksheet ws, int r1, int r2, int cols)
        {
            for (int r = r1; r <= r2; r++)
                for (int c = 1; c <= cols; c++)
                {
                    var cell = ws.Cell(r, c);
                    cell.Style.Border.BottomBorder      = XLBorderStyleValues.Thin;
                    cell.Style.Border.BottomBorderColor = XLColor.FromArgb(210, 210, 210);
                    if (r % 2 == 0)
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(248, 248, 248);
                }
        }

        private static int WriteSummary(IXLWorksheet ws, int startRow, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return startRow;
            int r = startRow;
            foreach (string raw in text.Split('\n'))
            {
                string line = raw.Trim();
                if (line.Length == 0) continue;
                var cell = ws.Cell(r, 1);
                cell.Value = line;
                cell.Style.Font.Bold      = true;
                cell.Style.Font.FontSize  = 10;
                cell.Style.Font.FontColor = BrownLight;
                r++;
            }
            return r;
        }

        private static void WriteSignature(IXLWorksheet ws, int row)
        {
            var darkGray = XLColor.FromArgb(85, 85, 85);

            ws.Cell(row,     1).Value = "Prepared by:";
            ws.Cell(row,     1).Style.Font.Bold = true;
            ws.Cell(row + 1, 1).Value = "Name:  ________________________________";
            ws.Cell(row + 3, 1).Value = "________________________________";
            ws.Cell(row + 4, 1).Value = "Signature over Printed Name";
            ws.Cell(row + 4, 1).Style.Font.Italic    = true;
            ws.Cell(row + 4, 1).Style.Font.FontColor = darkGray;
            ws.Cell(row + 5, 1).Value = "Position / Title:  ________________________________";
            ws.Cell(row + 5, 1).Style.Font.FontColor = darkGray;
            ws.Cell(row + 6, 1).Value = "Date Signed:  _______________";
            ws.Cell(row + 6, 1).Style.Font.FontColor = darkGray;
        }

        private static void FinishSheet(IXLWorksheet ws)
        {
            ws.Columns().AdjustToContents();
            ws.Column(1).Width = 10; // keep logo col — lock after AdjustToContents
            ws.PageSetup.PaperSize       = XLPaperSize.A4Paper;
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        }

        private static Dictionary<string, double> Aggregate(
            DataGridView dgv, int labelCol, int valueCol)
        {
            var d = new Dictionary<string, double>();
            foreach (DataGridViewRow dr in dgv.Rows)
            {
                string lbl = dr.Cells[labelCol].Value?.ToString() ?? "—";
                string raw = (dr.Cells[valueCol].Value?.ToString() ?? "0")
                             .Replace("₱", "").Replace(",", "").Trim();
                if (!double.TryParse(raw, out double v)) v = 0;
                if (d.ContainsKey(lbl)) d[lbl] += v; else d[lbl] = v;
            }
            return d;
        }

        private static void AddChart(string filePath, string chartTitle,
            string catLabel, string valLabel,
            Dictionary<string, double> data, bool isBar)
        {
            if (data.Count == 0) return;

            XL.Application? app = null;
            XL.Workbook?    wb  = null;

            try
            {
                app = new XL.Application();
                app.Visible       = false;
                app.DisplayAlerts = false;

                wb = app.Workbooks.Open(filePath,
                    Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value);

                XL.Worksheet ws2 = (XL.Worksheet)wb.Sheets.Add(
                    Missing.Value, wb.Sheets[wb.Sheets.Count],
                    Missing.Value, Missing.Value);
                ws2.Name = "Chart";

                XL.Range hdr1 = ws2.Range[ws2.Cells[1, 1], ws2.Cells[1, 6]];
                hdr1.Merge(Missing.Value);
                ((XL.Range)ws2.Cells[1, 1]).Value2 = CompanyName;
                ((XL.Range)ws2.Cells[1, 1]).Font.Bold  = true;
                ((XL.Range)ws2.Cells[1, 1]).Font.Size  = 14;
                ((XL.Range)ws2.Cells[1, 1]).Font.Color =
                    System.Drawing.ColorTranslator.ToOle(
                        System.Drawing.Color.FromArgb(74, 53, 37));
                ws2.Rows[1].RowHeight = 22;

                XL.Range hdr2 = ws2.Range[ws2.Cells[2, 1], ws2.Cells[2, 6]];
                hdr2.Merge(Missing.Value);
                ((XL.Range)ws2.Cells[2, 1]).Value2 = chartTitle;
                ((XL.Range)ws2.Cells[2, 1]).Font.Bold  = true;
                ((XL.Range)ws2.Cells[2, 1]).Font.Size  = 12;
                ((XL.Range)ws2.Cells[2, 1]).Font.Color =
                    System.Drawing.ColorTranslator.ToOle(
                        System.Drawing.Color.FromArgb(139, 90, 43));
                ws2.Rows[2].RowHeight = 18;

                ((XL.Range)ws2.Cells[3, 1]).Value2 =
                    $"Generated: {DateTime.Now:MMMM dd, yyyy}";
                ((XL.Range)ws2.Cells[3, 1]).Font.Size  = 9;
                ((XL.Range)ws2.Cells[3, 1]).Font.Color =
                    System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gray);
                ws2.Rows[3].RowHeight = 14;

                XL.Range th1 = (XL.Range)ws2.Cells[5, 1];
                XL.Range th2 = (XL.Range)ws2.Cells[5, 2];
                th1.Value2 = catLabel;  th1.Font.Bold = true;
                th2.Value2 = valLabel;  th2.Font.Bold = true;
                th1.Interior.Color = System.Drawing.ColorTranslator.ToOle(
                    System.Drawing.Color.FromArgb(220, 220, 220));
                th2.Interior.Color = th1.Interior.Color;

                int r = 6;
                foreach (var kv in data)
                {
                    ((XL.Range)ws2.Cells[r, 1]).Value2 = kv.Key;
                    ((XL.Range)ws2.Cells[r, 2]).Value2 = kv.Value;
                    r++;
                }
                int lastRow = r - 1;

                ((XL.Range)ws2.Columns[1]).AutoFit();
                ((XL.Range)ws2.Columns[2]).AutoFit();

                double chartTop  = (lastRow + 2) * 15.0;
                double chartLeft = 10;
                double chartW    = 480;
                double chartH    = 280;

                XL.Range dataRange = ws2.Range[ws2.Cells[5, 1], ws2.Cells[lastRow, 2]];
                XL.ChartObjects cos = (XL.ChartObjects)ws2.ChartObjects(Missing.Value);
                XL.ChartObject  co  = cos.Add(chartLeft, chartTop, chartW, chartH);
                XL.Chart        ch  = co.Chart;

                ch.ChartWizard(
                    Source:         dataRange,
                    Gallery:        isBar
                                    ? XL.XlChartType.xlBarClustered
                                    : XL.XlChartType.xlColumnClustered,
                    Format:         Missing.Value,
                    PlotBy:         XL.XlRowCol.xlColumns,
                    CategoryLabels: 1,
                    SeriesLabels:   0,
                    HasLegend:      false,
                    Title:          chartTitle,
                    CategoryTitle:  catLabel,
                    ValueTitle:     valLabel,
                    ExtraTitle:     Missing.Value);

                ((XL.Worksheet)wb.Sheets[1]).Activate();

                ws2.PageSetup.PrintArea = "$A$1:$I$" + (lastRow + 25);
                ws2.PageSetup.Zoom      = false;
                ws2.PageSetup.FitToPagesWide = 1;
                ws2.PageSetup.FitToPagesTall = 0;
                ws2.PageSetup.Orientation =
                    XL.XlPageOrientation.xlPortrait;

                wb.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Chart creation failed:\n{ex.Message}",
                    "Chart Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                try { if (wb  != null) { wb.Close(false);  Marshal.ReleaseComObject(wb);  } } catch { }
                try { if (app != null) { app.Quit();       Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
