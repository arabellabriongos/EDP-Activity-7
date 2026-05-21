using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    /// <summary>Lets the user pick which report to open.</summary>
    public class ReportChooserForm : Form
    {
        private static readonly Color Brown = Color.FromArgb(139, 90, 43);

        public ReportChooserForm()
        {
            Text = "Reports"; BackColor = Color.White;
            ClientSize = new Size(360, 280); StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false;

            var lbl = new Label {
                Text = "Select Report", Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 53, 37), AutoSize = true, Location = new Point(24, 20)
            };

            var sub = new Label {
                Text = "Choose a report to view and export to Excel.",
                Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(120, 100, 80),
                AutoSize = true, Location = new Point(24, 54)
            };

            MakeBtn("📊  Sales Report",     new Point(24, 90),  () => { new OrdersForm().ShowDialog(); });
            MakeBtn("📦  Inventory Report", new Point(24, 140), () => { new ProductsForm().ShowDialog(); });
            MakeBtn("👥  Customer Report",  new Point(24, 190), () => { new CustomersForm().ShowDialog(); });

            var btnClose = new Button {
                Text = "Close", BackColor = Color.FromArgb(140, 130, 120), ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(24, 234), Size = new Size(100, 32)
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (_, __) => Close();

            Controls.AddRange(new Control[] { lbl, sub, btnClose });
        }

        private void MakeBtn(string text, Point loc, Action onClick)
        {
            var b = new Button {
                Text = text, BackColor = Brown, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = loc, Size = new Size(312, 40)
            };
            b.FlatAppearance.BorderSize = 0;
            b.Click += (_, __) => onClick();
            Controls.Add(b);
        }
    }
}
