using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace LogisticMap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Tag = new SimpleScreen(pictureBox1);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SimpleScreen.exitNow = true;
        }

        private void bnDraw_Click(object sender, EventArgs e)
        {
            bnDraw.Enabled = false;
            SimpleScreen ss = Tag as SimpleScreen;
            ss.ClearScreen();
            ss.Draw();
            ss.UpdateScreen();
            bnDraw.Enabled = true;
        }
    }

    public class PointSet
    {
        private List<PointF> points = new List<PointF>();

        public PointF this[int index]
        {
            get
            {
                return points[index];
            }
            set
            {
                points[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return points.Count;
            }
        }

        public void Add(double x, double y)
        {
            points.Add(new PointF((float)x, (float)y));
        }
    }

    public class SimpleScreen
    {
        #region Implementation Details

        private PictureBox pb;
        private Bitmap canvas;
        private Graphics g;

        private PointF[] screenPoints;

        private double scaleX, scaleY;

        private double worldXmin, worldYmax;
        private double worldXmax, worldYmin;
        private double worldWidth, worldHeight;

        static public bool exitNow = false;

        public SimpleScreen(PictureBox pbForm)
        {
            pb = pbForm;
            canvas = new Bitmap(pb.Width, pb.Height);
            g = Graphics.FromImage(canvas);
        }

        public void ClearScreen()
        {
            g.Clear(pb.BackColor);
            pb.Image = canvas;
            Application.DoEvents();
        }

        public void UpdateScreen(int delay = 0)
        {
            pb.Image = canvas;
            if (delay > 0)
            {
                Application.DoEvents();
                if (exitNow) Environment.Exit(0);
                Thread.Sleep(delay);
            }
        }

        private void CalcScreenPoints(PointSet ps)
        {
            screenPoints = new PointF[ps.Count];
            for (int i = 0; i < ps.Count; i++)
            {
                // Convert WORLD coordinates to SCREEN coordinates
                double screenX = (ps[i].X - worldXmin) * scaleX;
                double screenY = pb.Height - (ps[i].Y - worldYmin) * scaleY;

                // Add this SCREEN coordinate to array of points
                screenPoints[i] = new PointF((float)screenX, (float)screenY);
            }
        }

        private void DrawBorder(Color clr, int width = 1)
        {
            g.DrawRectangle(new Pen(clr, width), 0, 0, canvas.Width - 1, canvas.Height - 1);
        }

        private void SetWorldRect(double xMin, double yMin, double xMax, double yMax)
        {
            worldXmin = xMin;
            worldYmax = yMax;
            worldXmax = xMax;
            worldYmin = yMin;

            worldWidth = worldXmax - worldXmin;
            worldHeight = worldYmax - worldYmin;

            scaleX = (pb.Width - 1) / worldWidth;
            scaleY = (pb.Height - 1) / worldHeight;
        }

        private void DrawAxes(Color clr, int width = 2)
        {
            // Draw X axis
            double screenY0 = pb.Height + worldYmin * scaleY;
            g.DrawLine(new Pen(clr, width), 0, (float)screenY0, pb.Width, (float)screenY0);

            // Draw Y axis
            double screenX0 = -worldXmin * scaleX;
            g.DrawLine(new Pen(clr, width), (float)screenX0, 0, (float)screenX0, pb.Height);
        }

        private void DrawLines(PointSet ps, Color clr, int width = 3, int delay = 0,
                               bool fill = false, bool close = true)
        {
            CalcScreenPoints(ps);
            if (fill)
            {
                g.FillPolygon(new SolidBrush(clr), screenPoints);
                UpdateScreen(delay);
            }
            else
            {
                if (delay == 0)
                {
                    if (close)
                        g.DrawPolygon(new Pen(clr, width), screenPoints);
                    else
                        g.DrawLines(new Pen(clr, width), screenPoints);
                }
                else
                {
                    int last = screenPoints.Length - 1;
                    Pen pen = new Pen(new SolidBrush(clr), width);
                    for (int i = 0; i < last; i++)
                    {
                        g.DrawLine(pen, screenPoints[i], screenPoints[i + 1]);
                        UpdateScreen(delay);
                    }
                    if (close)
                    {
                        g.DrawLine(pen, screenPoints[last], screenPoints[0]);
                        UpdateScreen(delay);
                    }
                }
            }
        }

        private void DrawRectangle(Color clr, double xMin, double yMin,
                                   double width, double height,
                                   int borderWidth = 3, bool fill = false)
        {
            PointSet ps = new PointSet();
            ps.Add(xMin, yMin);
            ps.Add(xMin + width, yMin);
            ps.Add(xMin + width, yMin + height);
            ps.Add(xMin, yMin + height);
            DrawLines(ps, clr, borderWidth, fill: fill);
        }

        private void DrawCircle(double centerX, double centerY,
                                double radius, Color clr, int width)
        {
            PointSet psCircle = new PointSet();

            int intervals = 97;

            double deltaTheta = 2.0 * Math.PI / intervals;

            for (int i = 0; i < intervals; i++)
            {
                double theta = deltaTheta * i;
                double x = centerX + radius * Math.Cos(theta);
                double y = centerY + radius * Math.Sin(theta);
                psCircle.Add(x, y);
            }

            DrawLines(psCircle, clr, width);
        }

        private void DrawPixel(double x, double y, Color clr)
        {
            double screenX = (x - worldXmin) * scaleX;
            double screenY = (pb.Height - 1) - (y - worldYmin) * scaleY;

            if ((screenX >= 0) && (screenX < canvas.Width)
                && (screenY >= 0) && (screenY < canvas.Height))
            {
                canvas.SetPixel((int)screenX, (int)screenY, clr);
            }
        }

        #endregion

        public void Draw()
        {
            double xMin = 2.5, yMin = 0.1;
            double xMax = 4.1, yMax = 1.0;

            SetWorldRect(xMin, yMin, xMax, yMax);

            Random r = new Random(2016);

            int iterations = 500;

            double dx = (xMax - xMin) / pb.Width;

            for (double x = xMin; x < xMax; x += dx)
            {
                double y = r.NextDouble();

                // TODO #1:
                //    Write a for() loop to go from 0 to iterations
                //       Inside the loop, iterate on the logistic map equation
                for(int i=0; i < iterations; i++)
                {
                    y = x * y * (1 - y);
                }
                for(int i=0;i<iterations;i++)
                {
                    y = x * y * (1 - y);
                    DrawPixel(x, y, Color.Blue);
                }
                // TODO #2:
                //    Write another for() loop to go from 0 to iterations
                //       Inside the loop:
                //          Iterate on the logistic map equation
                //          Call DrawPixel() to plot the current (x,y) values
            }
        }
    }
}


