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

namespace InverseDistanceWeighting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private double GetBottomReferenceHeight(double x, double z)
        {
            return -80;
        }

        private double GetActualHeight(double x, double z)
        {
            return ((30 * Math.Sin(0.025 * x) * Math.Cos(0.025 * z)) +
                (50 * Math.Cos(0.025 * Math.Sqrt(x * x + z * z))));
        }

        private double GetEstimatedHeight(double x, double z, Point3D[] samples)
        {
            double power = Convert.ToDouble(txtPower.Text);

            double sumWeight = 0;
            double sumHeightWeight = 0;

            for (int n = 0; n < samples.Length; n++)
            {
                double distance = Math.Sqrt(Math.Pow(x - samples[n].X, 2)
                                            + Math.Pow(z - samples[n].Z, 2));

                if (distance == 0) return samples[n].Y;

                double weight = 1 / Math.Pow(distance, power);

                sumWeight += weight;
                sumHeightWeight += samples[n].Y * weight;
            }

            double height = sumHeightWeight / sumWeight;

            return height;
        }

        private double CalcRMSD(Point3D[,] actual, Point3D[,] estimated)
        {
            double rmsDeviation = 0;
            double sumErrors = 0;
            for (int gridZ = 0; gridZ < actual.GetLength(1); gridZ++)
            {
                for (int gridX = 0; gridX < actual.GetLength(0); gridX++)
                {
                    double act = actual[gridX, gridZ].Y;
                    double est = estimated[gridX, gridZ].Y;

                    sumErrors = sumErrors + Math.Pow(act - est, 2);
                }
            }
            rmsDeviation = Math.Sqrt(sumErrors / actual.Length);
            return rmsDeviation;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(canvas);

            SolidBrush backgroundBrush = new SolidBrush(pictureBox1.BackColor);

            // Define the Z axis 2.5D tilt angle (standard is 45 degrees, expressed as radians)
            double thetaZ = 29.0 * Math.PI / 180;

            // Calculate the Z axis tilt modifiers
            double thetaZsin = Math.Sin(thetaZ);
            double thetaZcos = Math.Cos(thetaZ);

            // Define the size of the ocean and the number of intervals to subdivide the ocean
            double oceanSize = 400;
            int intervals = 30;
            double delta = oceanSize / intervals;

            // Create an array of randomly sampled data points
            int totalSamples = 220;
            Point3D[] samples = new Point3D[totalSamples];
            Random rnd = new Random(2015);
            for (int i = 0; i < totalSamples; i++)
            {
                double sampleX = rnd.NextDouble() * oceanSize;
                double sampleZ = -rnd.NextDouble() * oceanSize;
                double sampleY = GetActualHeight(sampleX, sampleZ);
                samples[i] = new Point3D(sampleX, sampleY, sampleZ);
            }

            // Create 2D arrays for bottom reference, actual, and estimated ocean heights at each vertex
            Point3D[,] bottomReference = new Point3D[intervals + 1, intervals + 1];
            Point3D[,] actual = new Point3D[intervals + 1, intervals + 1];
            Point3D[,] estimated = new Point3D[intervals + 1, intervals + 1];
            for (int gridZ = 0; gridZ <= intervals; gridZ++)
            {
                for (int gridX = 0; gridX <= intervals; gridX++)
                {
                    double z = -gridZ * delta;
                    double x = gridX * delta;
                    bottomReference[gridX, gridZ] = new Point3D(x, GetBottomReferenceHeight(x, z), z);
                    actual[gridX, gridZ] = new Point3D(x, GetActualHeight(x, z), z);
                    estimated[gridX, gridZ] = new Point3D(x, GetEstimatedHeight(x, z, samples), z);
                }
            }

            // Draw the bottom reference grid (aka the artificial minimal height plane)
            for (int gridZ = 0; gridZ < intervals; gridZ++)
            {
                for (int gridX = 0; gridX < intervals; gridX++)
                {
                    Point3D[] vertex = new Point3D[4];
                    vertex[0] = bottomReference[gridX, gridZ];
                    vertex[1] = bottomReference[gridX + 1, gridZ];
                    vertex[2] = bottomReference[gridX + 1, gridZ + 1];
                    vertex[3] = bottomReference[gridX, gridZ + 1];
                    PointF[] screenPt = new PointF[4];
                    for (int v = 0; v < 4; v++)
                    {
                        screenPt[v].X = (float)(vertex[v].X - (vertex[v].Z * thetaZcos * 0.225));
                        screenPt[v].Y = (float)(400 - vertex[v].Y + (vertex[v].Z * thetaZsin));
                    }
                    g.DrawPolygon(Pens.Black, screenPt);
                }
            }

            if (button1.Text.Equals("Show Samples"))
            {
                button1.Text = "Show Estimated";
            }
            else
            {
                // Determine if user wants to see the actual or estimated heights
                // Assign the correct grid color for the mode, and then alternate button text
                Pen gridPen = Pens.Green;
                if (button1.Text.Equals("Show Actual"))
                {
                    gridPen = Pens.Blue;
                    button1.Text = "Show Estimated";
                }
                else
                {
                    button1.Text = "Show Actual";
                }

                // Draw the requested grid based upon the mode
                for (int gridZ = intervals - 1; gridZ >= 0; gridZ--)
                {
                    for (int gridX = 0; gridX < intervals; gridX++)
                    {
                        Point3D[] vertex = new Point3D[4];
                        vertex[0] = (gridPen == Pens.Green) ? estimated[gridX, gridZ] : actual[gridX, gridZ];
                        vertex[1] = (gridPen == Pens.Green) ? estimated[gridX + 1, gridZ] : actual[gridX + 1, gridZ];
                        vertex[2] = (gridPen == Pens.Green) ? estimated[gridX + 1, gridZ + 1] : actual[gridX + 1, gridZ + 1];
                        vertex[3] = (gridPen == Pens.Green) ? estimated[gridX, gridZ + 1] : actual[gridX, gridZ + 1];
                        PointF[] screenPt = new PointF[4];
                        for (int v = 0; v < 4; v++)
                        {
                            screenPt[v].X = (float)(vertex[v].X - (vertex[v].Z * thetaZcos * 0.225));
                            screenPt[v].Y = (float)(400 - vertex[v].Y + (vertex[v].Z * thetaZsin));
                        }
                        g.FillPolygon(backgroundBrush, screenPt);
                        g.DrawPolygon(gridPen, screenPt);
                    }
                }

                // Display the Root Mean Square Error for this Power of the Interpolating Function
                txtRMSD.Text = String.Format("{0:F4}", CalcRMSD(actual, estimated));
            }

            // Draw the array of randomly sampled data points
            for (int i = 0; i < totalSamples; i++)
            {
                float screenX = (float)Math.Round(samples[i].X - (samples[i].Z * thetaZcos * 0.225));
                float screenY = (float)Math.Round(400 - samples[i].Y + (samples[i].Z * thetaZsin));
                g.FillRectangle(Brushes.Red, screenX, screenY, 3, 3);
            }

            pictureBox1.Image = canvas;
        }

        private class Point3D
        {
            public double X, Y, Z;

            public Point3D(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
    }
}

