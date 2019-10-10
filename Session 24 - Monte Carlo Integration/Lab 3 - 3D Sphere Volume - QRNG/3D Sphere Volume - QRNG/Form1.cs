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
using System.Diagnostics;



namespace Lab_3___3D_Sphere_Volume___Niederreiter_QRNG
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
            if (e.Button == MouseButtons.Left && !isZooming && bnDraw.Enabled)
            {
                isZooming = true;
                zoomRects.Add(new ScreenRect(e.Location));
                priorImage = (Bitmap)pictureBox1.Image.Clone();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isZooming && bnDraw.Enabled)
            {
                pictureBox1.Image = priorImage;
                pictureBox1.Update();

                zoomRects.Last().Resize(e.Location);

                Bitmap newImage = (Bitmap)pictureBox1.Image.Clone();
                Graphics g = Graphics.FromImage(newImage);
                g.DrawRectangle(new Pen(Color.Red, 3), zoomRects.Last().GetRectangle());

                pictureBox1.Image = newImage;
                pictureBox1.Update();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isZooming && bnDraw.Enabled)
            {
                zoomRects.Last().Resize(e.Location);
                Draw();
                isZooming = false;
            }
            if (e.Button == MouseButtons.Right && bnDraw.Enabled)
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
            if (bnDraw.Enabled)
            {
                bnDraw.Enabled = false;
                Application.DoEvents();
                SimpleScreen ss = Tag as SimpleScreen;
                ss.ClearScreen();

                int iterations = Convert.ToInt32(txtIterations.Text);
                double vol = 0;

                ss.Draw(zoomRects.LastOrDefault(), iterations, ref vol);

                ss.UpdateScreen(waitMode: 1);

                txtEstVol.Text = vol.ToString("N7");

                double actVol = Math.PI * 4.0 / 3.0;

                txtActualVol.Text = actVol.ToString("N7");

                double perError = Math.Abs((actVol - vol) / actVol);
                txtError.Text = perError.ToString("P7");

                bnDraw.Enabled = true;
                Application.DoEvents();
            }
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

        public WorldRect(double worldXmin, double worldYmin,
                        double worldXmax, double worldYmax,
                        PictureBox pictureBox)
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

    public class Point3D
    {
        public double X, Y, Z;

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class PointSet3D
    {
        private List<Point3D> points = new List<Point3D>();

        public Point3D this[int index]
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

        public void Add(double x, double y, double z)
        {
            points.Add(new Point3D(x, y, z));
        }

        public void Add(Point3D p3d)
        {
            points.Add(p3d);
        }
    }

    public class Facet
    {
        private PointSet3D vertices = new PointSet3D();

        public Facet(Point3D[] allVertices, int[] vertexNumbers)
        {
            for (int i = 0; i < vertexNumbers.Length; i++)
            {
                vertices.Add(allVertices[vertexNumbers[i]]);
            }
        }

        public PointSet3D Vertices
        {
            get
            {
                return vertices;
            }
            set
            {
                vertices = value;
            }
        }

        public int Count
        {
            get
            {
                return vertices.Count;
            }
        }
    }

    public class UnitVector
    {
        private double x, y, z;

        public UnitVector()
        {

        }

        public UnitVector(Point3D tail, Point3D tip)
        {
            x = tip.X - tail.X;
            y = tip.Y - tail.Y;
            z = tip.Z - tail.Z;
            Normalize();
        }

        private void Normalize()
        {
            double magnitude = (double)Math.Sqrt(x * x + y * y + z * z);
            x /= magnitude;
            y /= magnitude;
            z /= magnitude;
        }

        static public UnitVector CrossProduct(UnitVector A, UnitVector B)
        {
            UnitVector C = new UnitVector();

            C.x = A.y * B.z - A.z * B.y;
            C.y = A.z * B.x - A.x * B.z;
            C.z = A.x * B.y - A.y * B.x;

            C.Normalize();

            return C;
        }

        static public double DotProduct(UnitVector A, UnitVector B)
        {
            return A.x * B.x + A.y * B.y + A.z * B.z;
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

        private double obliqueAngle;
        private double obliqueCos;
        private double obliqueSin;
        private double aspectCorrection;

        static public bool exitNow = false;

        public SimpleScreen(PictureBox pbForm)
        {
            pb = pbForm;
            backgroundColor = Color.FromArgb(pb.BackColor.ToArgb());
            canvas = new Bitmap(pb.Width, pb.Height);
            g = Graphics.FromImage(canvas);
            worldRects = new List<WorldRect>();
            SetProjection();
        }

        public void ClearScreen()
        {
            g.Clear(pb.BackColor);
            pb.Image = canvas;
            Application.DoEvents();
        }

        public void UpdateScreen(int waitMode = 0, int delay = 0)
        {
            if (exitNow) Environment.Exit(0);
            pb.Image = canvas;
            if (waitMode > 0)
                Application.DoEvents();
            if (waitMode > 1)
                Thread.Sleep(delay);
        }

        private void SetWorldRectangle(double worldXmin, double worldYmin,
                                       double worldXmax, double worldYmax,
                                       ScreenRect zoom)
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

        private void SetProjection(double degrees = 45, double correction = 1)
        {
            obliqueAngle = degrees * Math.PI / 180;
            obliqueCos = Math.Cos(obliqueAngle);
            obliqueSin = Math.Sin(obliqueAngle);
            aspectCorrection = correction;
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

        private void CalcScreenPoints3D(PointSet3D ps3d)
        {
            screenPoints = new PointF[ps3d.Count];
            WorldRect w = worldRects.Last();
            for (int i = 0; i < ps3d.Count; i++)
            {
                // Convert WORLD coordinates to SCREEN coordinates
                double screenX = (ps3d[i].X - ps3d[i].Z * obliqueCos * aspectCorrection - w.xMin) * w.scaleX;
                double screenY = pb.Height - (ps3d[i].Y - ps3d[i].Z * obliqueSin - w.yMin) * w.scaleY;

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

        private void DrawLines(PointSet ps, Color clr, int width = 3,
                               bool fill = false, bool close = true,
                               int waitMode = 0, int delay = 0)
        {
            CalcScreenPoints(ps);
            if (fill)
            {
                g.FillPolygon(new SolidBrush(clr), screenPoints);
                UpdateScreen(waitMode, delay);
            }
            else
            {
                if (waitMode < 3)
                {
                    if (close)
                        g.DrawPolygon(new Pen(clr, width), screenPoints);
                    else
                        g.DrawLines(new Pen(clr, width), screenPoints);
                    UpdateScreen(waitMode, delay);
                }
                else
                {
                    int last = screenPoints.Length - 1;
                    Pen pen = new Pen(new SolidBrush(clr), width);
                    for (int i = 0; i < last; i++)
                    {
                        g.DrawLine(pen, screenPoints[i], screenPoints[i + 1]);
                        UpdateScreen(waitMode, delay);
                    }
                    if (close)
                    {
                        g.DrawLine(pen, screenPoints[last], screenPoints[0]);
                        UpdateScreen(waitMode, delay);
                    }
                }
            }
        }

        private void DrawLines(Facet[] facets, Color clr, int width = 3,
                               bool fill = false, bool close = true,
                               int waitMode = 0, int delay = 0)
        {
            for (int f = 0; f < facets.Length; f++)
            {
                CalcScreenPoints3D(facets[f].Vertices);
                if (fill)
                {
                    g.FillPolygon(new SolidBrush(clr), screenPoints);
                    UpdateScreen(waitMode, delay);
                }
                else
                {
                    if (waitMode < 3)
                    {
                        if (close)
                            g.DrawPolygon(new Pen(clr, width), screenPoints);
                        else
                            g.DrawLines(new Pen(clr, width), screenPoints);
                        UpdateScreen(waitMode, delay);
                    }
                    else
                    {
                        int last = screenPoints.Length - 1;
                        Pen pen = new Pen(new SolidBrush(clr), width);
                        for (int i = 0; i < last; i++)
                        {
                            g.DrawLine(pen, screenPoints[i], screenPoints[i + 1]);
                            UpdateScreen(waitMode, delay);
                        }
                        if (close)
                        {
                            g.DrawLine(pen, screenPoints[last], screenPoints[0]);
                            UpdateScreen(waitMode, delay);
                        }
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
            SetWorldRectangle(-1.25, -1.25, 1.25, 1.25, zoom);

            SetProjection(degrees: 29, correction: 0.225);

            // Define the radius of the sphere
            double radius = 1;

            // Define the number of intervals in hemisphere
            int intervals = 37;

            // Calculate the interval size
            double deltaPhi = Math.PI / intervals;          // Latitude
            double deltaTheta = 2 * Math.PI / intervals;    // Longitude

            // Define the camera vector
            Point3D origin = new Point3D(0.0, 0.0, 0.0);
            Point3D cameraLocation = new Point3D(0.3, 0.6, 1.2);
            UnitVector cameraVector = new UnitVector(cameraLocation, origin);

            // Start drawing sphere, stepping the phi angle through a half-circle in radians
            for (double phi = 0; phi < Math.PI; phi += deltaPhi)
            {
                double rA = radius * Math.Sin(phi);
                double rB = radius * Math.Sin(phi + deltaPhi);

                // Create a vertex array to hold the four points of this facet
                // Note:  We can currently only set the "Y" coordinate for each vertex
                Point3D[] vertex = new Point3D[4];
                vertex[0] = new Point3D(0, radius * Math.Cos(phi), 0);
                vertex[1] = new Point3D(0, radius * Math.Cos(phi + deltaPhi), 0);
                vertex[2] = new Point3D(0, radius * Math.Cos(phi + deltaPhi), 0);
                vertex[3] = new Point3D(0, radius * Math.Cos(phi), 0);

                // Step the theta angle around a full circle in radians
                // in order to set the "X" and "Z" coordinates for each vertex
                for (double theta = 0; theta < (Math.PI * 2); theta += deltaTheta)
                {
                    vertex[0].X = rA * Math.Sin(theta);
                    vertex[0].Z = rA * Math.Cos(theta);

                    vertex[1].X = rB * Math.Sin(theta);
                    vertex[1].Z = rB * Math.Cos(theta);

                    vertex[2].X = rB * Math.Sin(theta + deltaTheta);
                    vertex[2].Z = rB * Math.Cos(theta + deltaTheta);

                    vertex[3].X = rA * Math.Sin(theta + deltaTheta);
                    vertex[3].Z = rA * Math.Cos(theta + deltaTheta);

                    // Calculate facet vectors A and B
                    // Note:  The vertices are numbered in a counterclockwise direction
                    UnitVector vectorA = null;
                    UnitVector vectorB = null;
                    if (phi == 0)
                    {
                        // For the topmost slice only
                        vectorA = new UnitVector(vertex[1], vertex[2]);
                        vectorB = new UnitVector(vertex[1], vertex[0]);
                    }
                    else
                    {
                        // For every other slice after the topmost
                        vectorA = new UnitVector(vertex[0], vertex[1]);
                        vectorB = new UnitVector(vertex[0], vertex[3]);
                    }

                    // Calculate facet normal vector
                    UnitVector facetNormal = UnitVector.CrossProduct(vectorA, vectorB);

                    // Calculate dot product of camera vector and facet normal vector
                    double dotProduct = UnitVector.DotProduct(cameraVector, facetNormal);

                    // Only draw facets where the dotProduct < 0 since these are "facing" the camera
                    //if (dotProduct < 0)
                    {
                        Facet[] facets = new Facet[1];
                        facets[0] = new Facet(vertex, new int[] { 0, 1, 2, 3 });
                        DrawLines(facets, Color.Green, width: 1, waitMode: 0, delay: 10);
                    }
                }
            }

            Point3D[] v = new Point3D[2];
            Facet[] f = new Facet[1];

            int count = 0;

            int seed = 0;
            double[] r = new double[3];

            for (int i = 0; i < iterations; i++)
            {
                Niederreiter.niederreiter2(3, ref seed, r);

                double x = r[0] * -2.0 - 1.0;
                double y = r[1] * -2.0 - 1.0;
                double z = r[2] * -2.0 - 1.0;

                double distance = x * x + y * y + z * z;

                v[0] = new Point3D(x, y, z);
                v[1] = new Point3D(x + .01, y + .01, z + .01);
                f[0] = new Facet(v, new int[] { 0, 1 });

                if (distance <= 1.0)
                {
                    count++;
                    DrawLines(f, Color.Red, width: 1, waitMode: 0);
                }
                else
                {
                    DrawLines(f, Color.Blue, width: 1, waitMode: 0);
                }
            }

            area = (double)count / iterations * 8.0;
        }
    }
}
