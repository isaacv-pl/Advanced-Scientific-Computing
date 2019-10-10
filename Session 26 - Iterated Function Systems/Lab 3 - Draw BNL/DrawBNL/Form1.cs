﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace DrawBNL
{
    public partial class Form1 : Form
    {
        private bool isManuallyResizing = false;
        private Size priorSize;

        private bool isZooming = false;
        private List<ScreenRect> zoomRects;
        private Bitmap priorImage;

        public Form1()
        {
            InitializeComponent();
            Tag = new SimpleScreen(pictureBox1);
            pictureBox1.Dock = DockStyle.Fill;
            zoomRects = new List<ScreenRect>();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SimpleScreen ss = Tag as SimpleScreen;
            ss.exitNow = true;
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            isManuallyResizing = true;
            SimpleScreen ss = Tag as SimpleScreen;
            ss.stopDrawing = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if ((!isManuallyResizing) && (priorSize != Size)
                    && (WindowState != FormWindowState.Minimized))
            {
                priorSize = Size;
                SimpleScreen ss = Tag as SimpleScreen;
                ss.ResetScreen(zoomRects.LastOrDefault());
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            isManuallyResizing = false;
            if (priorSize != Size)
            {
                priorSize = Size;
                SimpleScreen ss = Tag as SimpleScreen;
                ss.ResetScreen(zoomRects.LastOrDefault());
            }
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
                g.DrawRectangle(new Pen(Color.White, 2), zoomRects.Last().GetRectangle());

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
            SimpleScreen ss = Tag as SimpleScreen;
            ss.ClearScreen();
            ss.Draw(zoomRects.LastOrDefault());
            ss.UpdateScreen();
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

            //int maxSide = Math.Max(xMax - xMin, yMax - yMin);
            //if (maxSide > (xMax - xMin))
            //    xMax = xMin + maxSide;
            //if (maxSide > (yMax - yMin))
            //    yMax = yMin + maxSide;

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

    public class IteratedFunctionSystem
    {
        private class Transform
        {
            public double x1, y1, x2, y2, x3, y3;
            public double[,] m;
            public Color c;
            public double p;
        }

        private List<Transform> transforms;
        private Random r;
        private double affineWidth;
        private double affineHeight;
        private double p;

        public int ColoringDepth { get; set; }

        private List<Color> prevClrs;

        public IteratedFunctionSystem()
        {
            transforms = new List<Transform>();
            r = new Random(2016);
            prevClrs = new List<Color>();
            ColoringDepth = 0;
        }

        private double Det(double[,] matrix)
        {
            double a = matrix[0, 0];
            double b = matrix[0, 1];
            double c = matrix[0, 2];
            double d = matrix[1, 0];
            double e = matrix[1, 1];
            double f = matrix[1, 2];
            double g = matrix[2, 0];
            double h = matrix[2, 1];
            double i = matrix[2, 2];

            double det = a * (e * i - f * h)
                         - (b * (d * i - f * g))
                         + c * (d * h - e * g);

            return det;
        }

        private double[,] Overlay(double[,] matrix, double[] values, int col)
        {
            double[,] newMatrix = new double[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    newMatrix[i, j] = matrix[i, j];
            for (int i = 0; i < 3; i++)
                newMatrix[i, col] = values[i];
            return newMatrix;
        }

        public void SetBaseFrame(double xMin, double yMin, double xMax, double yMax)
        {
            affineWidth = xMax - xMin;
            affineHeight = yMax - yMin;
        }

        public void AddMapping(double xLeft, double yLeft,
                       double xRight, double yRight,
                       double xTop, double yTop,
                       Color clr, double probability = 0)
        {
            Transform t = new Transform();
            t.x1 = xLeft; t.y1 = yLeft;
            t.x2 = xRight; t.y2 = yRight;
            t.x3 = xTop; t.y3 = yTop;
            t.c = clr;
            p += probability;
            t.p = p;
            transforms.Add(t);
        }

        public void GenerateTransforms()
        {
            double[,] coeff = new double[3, 3]
                      { {0, 0, 1},                // Bottom left corner
                        {affineWidth, 0, 1},      // Bottom right corner
                        {0, affineHeight, 1} };   // Top left corner

            double d = Det(coeff);

            foreach (Transform t in transforms)
            {
                double[] xVec = new double[3]
                    {t.x1, t.x2, t.x3};

                double[] yVec = new double[3]
                    {t.y1, t.y2, t.y3};

                double[,] matrix = new double[3, 3];
                matrix[0, 0] = Det(Overlay(coeff, xVec, 0)) / d;
                matrix[1, 0] = Det(Overlay(coeff, xVec, 1)) / d;
                matrix[2, 0] = Det(Overlay(coeff, xVec, 2)) / d;
                matrix[0, 1] = Det(Overlay(coeff, yVec, 0)) / d;
                matrix[1, 1] = Det(Overlay(coeff, yVec, 1)) / d;
                matrix[2, 1] = Det(Overlay(coeff, yVec, 2)) / d;
                matrix[0, 2] = 0;
                matrix[1, 2] = 0;
                matrix[2, 2] = 1;

                t.m = matrix;
            }
        }

        public void TransformPoint(ref double x, ref double y, ref Color clr)
        {
            Transform t;

            if (transforms[0].p == 0)
            {
                t = transforms[r.Next(transforms.Count)];
            }
            else
            {
                double p = r.NextDouble();
                t = transforms.FirstOrDefault(tn => p < tn.p)
                            ?? transforms.Last();
            }

            double xt = x * t.m[0, 0] + y * t.m[1, 0] + t.m[2, 0];
            double yt = x * t.m[0, 1] + y * t.m[1, 1] + t.m[2, 1];

            x = xt;
            y = yt;

            if (ColoringDepth == 0)
                clr = t.c;
            else
            {
                if (prevClrs.Count == ColoringDepth)
                {
                    clr = prevClrs.First();
                    prevClrs.RemoveAt(0);
                }

                if (prevClrs.Count < ColoringDepth)
                {
                    prevClrs.Add(t.c);
                }
            }
        }
    }

    public class SimpleScreen
    {
        #region Implementation Details

        private PictureBox pb;
        private Bitmap canvas;
        private Graphics g;

        private PointF[] screenPoints;

        private List<WorldRect> worldRects;

        public bool exitNow = false;
        public bool stopDrawing = false;

        public SimpleScreen(PictureBox pbForm)
        {
            pb = pbForm;
            canvas = new Bitmap(pb.Width, pb.Height);
            g = Graphics.FromImage(canvas);
            worldRects = new List<WorldRect>();
        }

        public void ResetScreen(ScreenRect zoom)
        {
            stopDrawing = false;
            canvas = new Bitmap(pb.Width, pb.Height);
            g = Graphics.FromImage(canvas);
            Draw(zoom);
            UpdateScreen();
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

        private void DrawBorder(Color clr, int width = 1)
        {
            g.DrawRectangle(new Pen(clr, width), 0, 0, canvas.Width - 1, canvas.Height - 1);
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

        public void Draw(ScreenRect zoom)
        {
            IteratedFunctionSystem ifs = new IteratedFunctionSystem();

            SetWorldRectangle(-1, -2, 18, 9, zoom);
            
            ifs.SetBaseFrame(0, 0, 17, 7);

            ifs.ColoringDepth = 0;

            // B
            ifs.AddMapping(0, 6, 2, 6, 0, 7, Color.LightBlue);     // 01
            ifs.AddMapping(2, 6, 4, 6, 2, 7, Color.Blue);          // 02
            ifs.AddMapping(1, 4, 1, 6, 0, 4, Color.Blue);          // 03
            ifs.AddMapping(4, 6, 4, 4, 5, 6, Color.Blue);          // 04
            ifs.AddMapping(0, 3, 2, 3, 0, 4, Color.Blue);          // 05
            ifs.AddMapping(4, 4, 2, 4, 4, 3, Color.Red);           // 06
            ifs.AddMapping(1, 1, 1, 3, 0, 1, Color.Pink);          // 07
            ifs.AddMapping(4, 3, 4, 1, 5, 3, Color.Blue);          // 08
            ifs.AddMapping(0, 0, 2, 0, 0, 1, Color.Blue);          // 09
            ifs.AddMapping(4, 1, 2, 1, 4, 0, Color.Sienna);        // 10

            // N
            ifs.AddMapping(6, 6, 7, 6, 6, 7, Color.Blue);          // 11
            ifs.AddMapping(10, 6, 11, 6, 10, 7, Color.PapayaWhip); // 12
            ifs.AddMapping(6, 5, 8, 5, 6, 6, Color.Chartreuse);    // 13
            ifs.AddMapping(11, 4, 11, 6, 10, 4, Color.Blue);       // 14
            ifs.AddMapping(7, 3, 7, 5, 6, 3, Color.Blue);          // 15
            ifs.AddMapping(8, 4, 9, 4, 8, 5, Color.Blue);          // 16
            ifs.AddMapping(7, 1, 7, 3, 6, 1, Color.Orange);        // 17
            ifs.AddMapping(9, 2, 9, 4, 8, 2, Color.Blue);          // 18
            ifs.AddMapping(10, 4, 10, 2, 11, 4, Color.Maroon);     // 19
            ifs.AddMapping(6, 0, 7, 0, 6, 1, Color.Blue);          // 20
            ifs.AddMapping(9, 1, 11, 1, 9, 2, Color.Blue);         // 21
            ifs.AddMapping(10, 0, 11, 0, 10, 1, Color.Yellow);     // 22

            // L
            ifs.AddMapping(13, 5, 13, 7, 12, 5, Color.Green);      // 23
            ifs.AddMapping(12, 4, 13, 4, 12, 5, Color.Blue);       // 24
            ifs.AddMapping(12, 4, 12, 2, 13, 4, Color.Blue);       // 25
            ifs.AddMapping(13, 1, 13, 2, 12, 1, Color.White);      // 26
            ifs.AddMapping(12, 0, 14, 0, 12, 1, Color.Blue);       // 27
            ifs.AddMapping(15, 0, 15, 1, 14, 0, Color.Blue);       // 28
            ifs.AddMapping(15, 0, 17, 0, 15, 1, Color.Purple);     // 29

            ifs.GenerateTransforms();

            double x = 0, y = 0;
            Color clr = Color.Black;

            int iterations = 1000000;

            for (int i = 0; i < iterations; i++)
            {
                ifs.TransformPoint(ref x, ref y, ref clr);
                DrawPixel(x, y, clr);
                if (stopDrawing) break;
            }
        }
    }
}
