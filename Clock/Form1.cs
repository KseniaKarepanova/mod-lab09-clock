using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ClientSize = new Size(500, 500);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            GraphicsState gs;


            int w = ClientSize.Width;
            int h = ClientSize.Height;
            g.TranslateTransform(w / 2, h / 2);

            Pen p = new Pen(Color.Chocolate, 5);
            g.DrawEllipse(p, -w / 2 + p.Width, -h / 2 + p.Width, w - 2 * p.Width, h - 2 * p.Width);

            Brush brush = new SolidBrush(Color.LemonChiffon);
            g.FillEllipse(brush, -w / 2 + p.Width, -h / 2 + p.Width, w - 2 * p.Width, h - 2 * p.Width);

            Pen p_1 = new Pen(Color.Chocolate, 2);
            Pen p_2 = new Pen(Color.Chocolate, 4);

            DrawClock(6, 20, g, p_1);
            DrawClock(30, 40, g, p_2);
            
            DrawClockFace(g, new Point(0, 0), w / 2);

            DateTime dt = DateTime.Now;
            Pen p_second = new Pen(Color.Red, 2);
            gs = g.Save();
            g.RotateTransform(6 * dt.Second);
            g.DrawLine(p_second, 0, h / 10, 0, (float)(-h / 2.5));
            g.Restore(gs);


            Pen p_minute = new Pen(Color.Black, 6);
            p_minute.StartCap = LineCap.RoundAnchor;
            p_minute.EndCap = LineCap.SquareAnchor;
            gs = g.Save();
            g.RotateTransform(6 * dt.Minute + dt.Second / 10);
            g.DrawLine(p_minute, 0, 0, 0, (float)(-h / 3));
            g.Restore(gs);

            Pen p_hour = new Pen(Color.Black, 10);
            p_minute.EndCap = LineCap.Round;
            gs = g.Save();
            g.RotateTransform(30 * (dt.Hour % 12) + dt.Minute / 2 + dt.Second / 120);
            g.DrawLine(p_hour, 0, 0, 0, (float)(-h / 4));
            g.Restore(gs);

        }

        void DrawClock(int angleBetweenSerifs, int serifLength, Graphics g, Pen p)
        {
            Point clockCenter = new Point(0, 0);
            int clockRadius = (int)Math.Min(ClientSize.Width / 2 - p.Width, ClientSize.Height / 2 - p.Width);

            for (int angle = 0; angle < 360; angle += angleBetweenSerifs)
            {
                double radians = angle * Math.PI / 180;
                int x = clockCenter.X + (int)(clockRadius * Math.Cos(radians));
                int y = clockCenter.Y + (int)(clockRadius * Math.Sin(radians));
                Point serifEnd = new Point(x, y);

                x = clockCenter.X + (int)((clockRadius - serifLength) * Math.Cos(radians));
                y = clockCenter.Y + (int)((clockRadius - serifLength) * Math.Sin(radians));
                Point serifStart = new Point(x, y);

                g.DrawLine(p, serifStart, serifEnd);
            }
        }

        static void DrawClockFace(Graphics graphics, Point center, int radius)
        {
            float fontSize = radius / 10f;
            Font font = new Font("Arial", fontSize, FontStyle.Bold);

            for (int hour = 1; hour <= 12; hour++)
            {
                float angle = hour * 30;
                float numberX = center.X + (float)(radius * 0.75 * Math.Cos((angle - 90) * Math.PI / 180));
                float numberY = center.Y + (float)(radius * 0.75 * Math.Sin((angle - 90) * Math.PI / 180));

                graphics.DrawString(hour.ToString(), font, Brushes.Black, numberX, numberY, new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}
