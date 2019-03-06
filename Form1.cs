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

namespace BresenhamTest
{
    public partial class Form1 : Form
    {
        private Brush b = new SolidBrush(Color.Red);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // atomic
            PlotEllipse(Width / 2, Height / 2, (int)numericUpDown1.Value, (int)numericUpDown2.Value, e);
            PlotLine(100, 100, 10, 10, e);
            PlotArc(200, 200, 48, 40, 180, 24, e);

            // shortcuts
            PlotRectangle(150, 50, 190, 90, e);
            PlotTriangle(200, 200, 250, 200, 225, 180, e);
            PlotRoundedRect(250, 250, 350, 350, 32, e);
        }

        private double degtorad(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        void PlotRoundedRect(int x1, int y1, int x2, int y2, int rounded, PaintEventArgs e)
        {
            PlotLine(x1 + rounded, y1, x2 - rounded, y1, e);
            PlotLine(x1 + rounded, y2, x2 - rounded, y2, e);
            PlotLine(x1, y1 + rounded, x1, y2 - rounded, e);
            PlotLine(x2, y1 + rounded, x2, y2 - rounded, e);

            PlotArc(x1 + rounded, y1 + rounded, rounded, 180, 90, 32, e);
            PlotArc(x2 - rounded, y1 + rounded, rounded, 270, 90, 32, e);
            PlotArc(x1 + rounded, y2 - rounded, rounded, 90, 90, 32, e);
            PlotArc(x2 - rounded, y2 - rounded, rounded, 0, 90, 32, e);
        }

        void PlotTriangle(int x1, int y1, int x2, int y2, int x3, int y3, PaintEventArgs e)
        {
            PlotLine(x1, y1, x2, y2, e);
            PlotLine(x1, y1, x3, y3, e);
            PlotLine(x2, y2, x3, y3, e);
        }

        void PlotRectangle(int x1, int y1, int x2, int y2, PaintEventArgs e)
        {
            PlotLine(x1, y1, x2, y1, e);
            PlotLine(x1, y1, x1, y2, e);
            PlotLine(x2, y1, x2, y2, e);
            PlotLine(x2, y2, x1, y2, e);
        }

        void PlotArc(int cx, int cy, int r, float startAngle, float arcAngle, int segments, PaintEventArgs e)
        {
            float theta = (6.283185f / segments) * (arcAngle / 360f);
            float tv = (float) Math.Tan(theta);
            float rv = (float) Math.Cos(theta);

            float x = r * (float) Math.Cos(degtorad(startAngle));
            float y = r * (float) Math.Sin(degtorad(startAngle));

            int prevX = (int) (x + cx);
            int prevY = (int) (y + cy);

            int nextX = (int)(x + cx);
            int nextY = (int)(y + cy);

            for (int i = 0; i < segments; i++)
            {
                nextX = (int)(x + cx);
                nextY = (int)(y + cy);

                PlotLine(prevX, prevY, nextX, nextY, e);

                float tx = -y;
                float ty = x;

                x += tx * tv;
                y += ty * tv;

                x *= rv;
                y *= rv;

                prevX = nextX;
                prevY = nextY;
            }

            PlotLine(prevX, prevY, nextX, nextY, e);
        }

        void PlotLine(int x1, int y1, int x2, int y2, PaintEventArgs e)
        {
            int dx, dy, sx, sy, e2, err;

            dx = Math.Abs(x2 - x1);
            dy = Math.Abs(y2 - y1);
            err = 0;

            sx = x1 < x2 ? 1 : -1;
            sy = y1 < y2 ? 1 : -1;

            for (;;)
            {
                putPixel(x1, y1, e);

                if (x1 == x2 && y1 == y2)
                {
                    break;
                }

                e2 = err;

                if (e2 > -dx)
                {
                    err -= dy;
                    x1 += sx;
                }

                if (e2 < dy)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        void PlotEllipse(int cx, int cy, int xrad, int yrad, PaintEventArgs e)
        {
            int x = 0, y = 0, xchange = 0, ychange = 0, ellipseError = 0, TwoASquare = 0, TwoBSquare = 0, StoppingX = 0, StoppingY = 0;

            TwoASquare = 2 * xrad * xrad;
            TwoBSquare = 2 * yrad * yrad;
            x = xrad;
            y = 0;

            xchange = yrad * yrad * (1 - 2 * xrad);
            ychange = xrad * xrad;

            ellipseError = 0;

            StoppingX = TwoBSquare * xrad;
            StoppingY = 0;

            while (StoppingX >= StoppingY)
            {
                Plot4EllipsePoints(x, y);
                y++;
                StoppingY += TwoASquare;
                ellipseError += ychange;
                ychange += TwoASquare;

                if (2 * ellipseError + xchange > 0)
                {
                    x--;
                    StoppingX -= TwoBSquare;
                    ellipseError += xchange;
                    xchange += TwoBSquare;
                }
            }

            x = 0;
            y = yrad;

            xchange = yrad * yrad;
            ychange = xrad * xrad * (1 - 2 * yrad);
            ellipseError = 0;
            StoppingX = 0;
            StoppingY = TwoASquare * yrad;

            while (StoppingX <= StoppingY)
            {
                Plot4EllipsePoints(x, y);
                x++;
                StoppingX += TwoBSquare;
                ellipseError += xchange;
                xchange += TwoBSquare;

                if (2 * ellipseError + ychange > 0)
                {
                    y--;
                    StoppingY -= TwoASquare;
                    ellipseError += ychange;
                    ychange += TwoASquare;
                }
            }


            void Plot4EllipsePoints(int xx, int yy)
            {
                putPixel(cx + x, cy + y, e);
                putPixel(cx - x, cy + y, e);
                putPixel(cx - x, cy - y, e);
                putPixel(cx + x, cy - y, e);
            }
        }



        void putPixel(int x, int y, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(b, x, y, 1, 1);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 10;
            Invalidate();
        }
    }
}
