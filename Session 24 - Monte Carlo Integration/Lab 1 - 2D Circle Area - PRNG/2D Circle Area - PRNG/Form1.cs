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


namespace Lab_1___2D_Circle_Area___Monte_Carlo
{
    public partial class Form1 : Form
    {
        private bool isZooming = false;
        private List<ScreenRect> zoomRects;
        private Bitmap priorImage;

        public Form1()
        {
            InitializeComponent();
            Tag = new SimpleScreen(pictureBox1);
            zoomRects = new List<ScreenRect>();
        }

        private void bnDraw_Click(object sender, EventArgs e)
        {
            zoomRects.Clear();
            Draw();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SimpleScreen.exitNow = true;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !isZooming)
            {
                isZooming = true;
                zoomRects.Add(new ScreenRect(e.Location));
                priorImage = (Bitmap)pictureBox1.Image.Clone();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isZooming)
            {
                pictureBox1.Image = priorImage;
                pictureBox1.Update();

                zoomRects.Last().Resize(e.Location);

                Bitmap newImage = (Bitmap)pictureBox1.Image.Clone();
                Graphics g = Graphics.FromImage(newImage);
                g.DrawRectangle(new Pen(Color.Red, 2), zoomRects.Last().GetRectangle());

                pictureBox1.Image = newImage;
                pictureBox1.Update();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isZooming)
            {
                zoomRects.Last().Resize(e.Location);
                Draw();
                isZooming = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                if (zoomRects.Count > 0)
                    zoomRects.RemoveAt(zoomRects.Count - 1);
                SimpleScreen ss = Tag as SimpleScreen;
                ss.Unzoom();
                Draw();
            }
        }

        private void Draw()
        {
            bnDraw.Enabled = false;
            SimpleScreen ss = Tag as SimpleScreen;
            ss.ClearScreen();

            int iterations = Convert.ToInt32(txtIterations.Text);
            double area = 0;

            ss.Draw(zoomRects.LastOrDefault(), iterations, ref area);

            ss.UpdateScreen();

            txtEstimatedArea.Text = area.ToString("N7");
            txtActualArea.Text = Math.PI.ToString("N7");

            double perError = Math.Abs((Math.PI - area) / Math.PI);
            txtError.Text = perError.ToString("P7");

            bnDraw.Enabled = true;
        }
    }

    public class ScreenRect
    {
        private Point pA, pB;

        private int xMin, yMin, xMax, yMax;

        public ScreenRect(Point p1)
        {
            pA = p1;
        }
        public void Resize(Point p2)
        {
            pB = p2;
        }

        public Rectangle GetRectangle()
        {
            xMin = Math.Min(pA.X, pB.X);
            yMin = Math.Min(pA.Y, pB.Y);
            xMax = Math.Max(pA.X, pB.X);
            yMax = Math.Max(pA.Y, pB.Y);

            int maxSide = Math.Max(xMax - xMin, yMax - yMin);
            if (maxSide > (xMax - xMin))
                xMax = xMin + maxSide;
            if (maxSide > (yMax - yMin))
                yMax = yMin + maxSide;

            return Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
        }

        public void GetPoints(out Point p1, out Point p2)
        {
            Rectangle r = GetRectangle();
            p1 = new Point(r.Left, r.Bottom);
            p2 = new Point(r.Right, r.Top);
        }
    }

    public class WorldRect
    {
        public double xMin, xMax, yMin, yMax;
        public PictureBox pb;

        public WorldRect(double worldXmin, double worldYmin, double worldXmax, double worldYmax, PictureBox pictureBox)
        {
            pb = pictureBox;
            xMin = worldXmin;
            yMin = worldYmin;
            xMax = worldXmax;
            yMax = worldYmax;
        }

        public WorldRect Scale(ScreenRect zoom)
        {
            Point p1, p2;
            zoom.GetPoints(out p1, out p2);

            double xMin2 = xMin + (double)p1.X / pb.Width * width;
            double xMax2 = xMin + (double)p2.X / pb.Width * width;

            double yMin2 = yMax - (double)p1.Y / pb.Height * height;
            double yMax2 = yMax - (double)p2.Y / pb.Height * height;

            return new WorldRect(xMin2, yMin2, xMax2, yMax2, pb);
        }

        public double width => xMax - xMin;
        public double height => yMax - yMin;

        public double scaleX => (pb.Width - 1) / width;
        public double scaleY => (pb.Height - 1) / height;

        public bool Contains(double x, double y)
        {
            if (x < xMin) return false;
            if (x > xMax) return false;
            if (y < yMin) return false;
            if (y > yMax) return false;
            return true;
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
        private Color backgroundColor;

        private PointF[] screenPoints;

        private List<WorldRect> worldRects;

        static public bool exitNow = false;

        public SimpleScreen(PictureBox pbForm)
        {
            pb = pbForm;
            backgroundColor = Color.FromArgb(pb.BackColor.ToArgb());
            canvas = new Bitmap(pb.Width, pb.Height);
            g = Graphics.FromImage(canvas);
            worldRects = new List<WorldRect>();
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

        private void SetWorldRectangle(double worldXmin, double worldYmin, double worldXmax, double worldYmax, ScreenRect zoom)
        {
            if (zoom == null)
                worldRects.Add(new WorldRect(worldXmin, worldYmin, worldXmax, worldYmax, pb));
            else
                worldRects.Add(worldRects.Last().Scale(zoom));
        }

        public void Unzoom()
        {
            if (worldRects.Count > 1)
            {
                worldRects.RemoveRange(worldRects.Count - 2, 2);
            }
        }

        private void DrawBorder(Color clr, int width = 1)
        {
            g.DrawRectangle(new Pen(clr, width), 0, 0, canvas.Width - 1, canvas.Height - 1);
        }

        private void CalcScreenPoints(PointSet ps)
        {
            screenPoints = new PointF[ps.Count];
            WorldRect w = worldRects.Last();
            for (int i = 0; i < ps.Count; i++)
            {
                // Convert WORLD coordinates to SCREEN coordinates
                double screenX = (ps[i].X - w.xMin) * w.scaleX;
                double screenY = pb.Height - (ps[i].Y - w.yMin) * w.scaleY;

                // Add this SCREEN coordinate to array of points
                screenPoints[i] = new PointF((float)screenX, (float)screenY);
            }
        }

        private void DrawAxes(Color clr, int width = 2)
        {
            WorldRect w = worldRects.Last();

            // Draw X axis
            double screenY0 = pb.Height + w.yMin * w.scaleY;
            g.DrawLine(new Pen(clr, width), 0, (float)screenY0, pb.Width, (float)screenY0);

            // Draw Y axis
            double screenX0 = -w.xMin * w.scaleX;
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
                                   double width, double height, int borderWidth = 3, bool fill = false)
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

        private void DrawPixel(double x, double y, Color clr, int delay = 0)
        {
            WorldRect w = worldRects.Last();
            if (w.Contains(x, y))
            {
                double screenX = (x - w.xMin) * w.scaleX;
                double screenY = (pb.Height - 1) - (y - w.yMin) * w.scaleY;

                if ((screenX >= 0) && (screenX < canvas.Width)
                    && (screenY >= 0) && (screenY < canvas.Height))
                    canvas.SetPixel((int)screenX, (int)screenY, clr);
            }
        }

        private Color GetPixel(double x, double y)
        {
            WorldRect w = worldRects.Last();
            if (w.Contains(x, y))
            {
                double screenX = (x - w.xMin) * w.scaleX;
                double screenY = (pb.Height - 1) - (y - w.yMin) * w.scaleY;

                if ((screenX >= 0) && (screenX < canvas.Width)
                    && (screenY >= 0) && (screenY < canvas.Height))
                    return canvas.GetPixel((int)screenX, (int)screenY);
            }
            return Color.Transparent;
        }
        #endregion

        public void Draw(ScreenRect zoom, double iterations, ref double area)
        {
            SetWorldRectangle(-1.05, -1.05, 1.05, 1.05, zoom);

            DrawAxes(Color.Black);

            PointSet psCircle = new PointSet();

            int intervals = 97;

            double deltaTheta = 2.0 * Math.PI / intervals;

            for (int i = 0; i < intervals; i++)
            {
                double theta = deltaTheta * i;
                double x = Math.Cos(theta);
                double y = Math.Sin(theta);
                psCircle.Add(x, y);
            }

            DrawLines(psCircle, Color.Green);

            Random r = new Random(2016);

            int count = 0;

            for (int i = 0; i < iterations; i++)
            {
                double x = r.NextDouble() * 2.0 - 1.0;
                double y = r.NextDouble() * 2.0 - 1.0;

                double distance = x * x + y * y;

                if (distance <= 1.0)
                {
                    count++;
                    DrawPixel(x, y, Color.Red);
                }
                else
                {
                    DrawPixel(x, y, Color.Blue);
                }
            }

            area = (double)count / iterations * 4.0;
        }
    }

}