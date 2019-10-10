using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using static System.Math;
using static System.Console;


namespace Lab_4D_HyperSphere_Volume
{
    class Program
    {
        static int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31,
                                37, 41, 43, 47, 53, 59, 61, 67, 71 };

        static double Halton(int n, int p)
        {
            int b = primes[p];
            double h = 0;
            double f = 1;
            for (double i = n; i > 0; i = Floor(i / b))
            {
                f = f / b;
                h += f * (i % b);
            }
            return h;
        }

        static void Main(string[] args)
        {
            int iterations = 1000000;

            double count = 0;

            for (int i = 0; i < iterations; i++)
            {
                double x = Halton(i, 0);
                double y = Halton(i, 1);
                double z = Halton(i, 2);
                double w = Halton(i, 3);

                double distance = x * x + y * y + z * z+ w * w;

                if (distance <= 1.0)
                    count++;
            }

            double volume = count / iterations * 16;

            WriteLine($"{volume:F9}");

            Write("Press any key to continue . . .");
            ReadKey();
        }
    }
}
