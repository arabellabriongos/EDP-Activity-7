using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace BrewAndBiteCafe
{
    public partial class AboutForm : Form
    {
        private Image? _logoImage;

        public AboutForm()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            BuildLayout();
        }

        private void BuildLayout()
        {
            this.Controls.Clear();
            this.ClientSize = new Size(560, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About";

            string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "coffee1.png");
            if (File.Exists(imgPath))
                _logoImage = Image.FromFile(imgPath);

            int cx = 24; // content x
            int cw = 512; // content width

            // App identity card: circle + name side by side
            Panel idCard = MakeCard(cx, 24, cw, 100);
            this.Controls.Add(idCard);

            // Circle logo
            Panel picCircle = new Panel
            {
                BackColor = Color.Transparent,
                Bounds = new Rectangle(16, 14, 68, 68)
            };
            picCircle.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var bgBrush = new SolidBrush(Color.FromArgb(80, 55, 35));
                g.FillEllipse(bgBrush, 0, 0, 67, 67);
                if (_logoImage != null)
                {
                    using var gp = new GraphicsPath();
                    gp.AddEllipse(2, 2, 63, 63);
                    g.SetClip(gp);
                    g.DrawImage(_logoImage, 2, 2, 63, 63);
                    g.ResetClip();
                }
            };
            idCard.Controls.Add(picCircle);

            int tx = 100;
            idCard.Controls.Add(MakeLabel("Brew && Bite Cafe",
                new Font("Segoe UI", 17F, FontStyle.Bold),
                Color.FromArgb(74, 53, 37), new Point(tx, 16)));

            idCard.Controls.Add(MakeLabel("Sales && Inventory Management System",
                new Font("Segoe UI", 9F, FontStyle.Italic),
                Color.FromArgb(139, 90, 43), new Point(tx, 52)));

            //Description card
            this.Controls.Add(MakeSectionLabel("ABOUT THE SYSTEM", cx, 140));

            Panel descCard = MakeCard(cx, 158, cw, 90);
            descCard.Controls.Add(new Label
            {
                Text = "Brew && Bite Cafe is a sales and inventory management system " +
                       "designed to help a cafe business manage their daily sales, track " +
                       "inventory, and generate reports efficiently.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(80, 65, 50),
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(cw - 40, 72),
                Location = new Point(18, 12),
                TextAlign = ContentAlignment.TopLeft
            });
            this.Controls.Add(descCard);

            // Developer info card
            this.Controls.Add(MakeSectionLabel("DEVELOPER INFO", cx, 266));

            Panel devCard = MakeCard(cx, 284, cw, 220);
            var devRows = new[]
            {
                ("Developer",  "Arabella B. Briongos"),
                ("Course",     "BS Information Technology (BSIT)"),
                ("School",     "Bicol University"),
                ("Year",       "2026"),
            };

            int rowY = 14;
            for (int i = 0; i < devRows.Length; i++)
            {
                var (label, value) = devRows[i];
                devCard.Controls.Add(MakeLabel(label,
                    new Font("Segoe UI", 8.5F),
                    Color.FromArgb(140, 120, 100), new Point(20, rowY)));
                devCard.Controls.Add(MakeLabel(value,
                    new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    Color.FromArgb(55, 40, 25), new Point(20, rowY + 17)));

                if (i < devRows.Length - 1)
                {
                    devCard.Controls.Add(new Panel
                    {
                        BackColor = Color.FromArgb(225, 218, 205),
                        Bounds = new Rectangle(16, rowY + 44, cw - 52, 1)
                    });
                }
                rowY += 52;
            }
            this.Controls.Add(devCard);

            // Footer
            this.Controls.Add(new Label
            {
                Text = "© 2026 Brew && Bite Cafe. All rights reserved.",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(180, 165, 150),
                BackColor = Color.White,
                AutoSize = true,
                Location = new Point(cx, 524)
            });

            Button btnClose = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(139, 90, 43),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 34),
                Location = new Point(cx + cw - 110, 518)
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (_, __) => this.Close();
            this.Controls.Add(btnClose);
        }

        // Helpers
        private static Panel MakeCard(int x, int y, int w, int h)
        {
            var p = new Panel
            {
                BackColor = Color.White,
                Bounds = new Rectangle(x, y, w, h),
                Tag = Color.FromArgb(247, 244, 238)
            };
            p.Paint += PaintCard;
            return p;
        }

        private static Label MakeLabel(string text, Font font, Color fore, Point loc)
        {
            return new Label
            {
                Text = text,
                Font = font,
                ForeColor = fore,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = loc
            };
        }

        private static Label MakeSectionLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(160, 140, 120),
                BackColor = Color.White,
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private static void PaintCard(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel p) return;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int r = 12;
            var rect = new Rectangle(4, 4, p.Width - 10, p.Height - 10);
            for (int i = 3; i >= 1; i--)
            {
                using var sh = new SolidBrush(Color.FromArgb(10 * i, 0, 0, 0));
                FillRounded(g, sh, new Rectangle(rect.X + i, rect.Y + i + 1, rect.Width, rect.Height), r);
            }
            Color fill = p.Tag is Color tc ? tc : Color.FromArgb(247, 244, 238);
            using var bg = new SolidBrush(fill);
            FillRounded(g, bg, rect, r);
        }

        private static void FillRounded(Graphics g, Brush brush, Rectangle rect, int radius)
        {
            if (rect.Width <= 0 || rect.Height <= 0) return;
            int d = radius * 2;
            using var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            g.FillPath(brush, path);
        }
    }
}
