using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static System.Math;
using static System.Console;

namespace Lab_Higher_Dimension_HyperSphere_Volume
{
    class Program
    {
        static double Halton(int n, int p)
        {
            double h = 0;
            double f = 1;
            for (int i = n; i > 0; i /= p)
            {
                f = f / p;
                h += f * (i % p);
            }
            return h;
        }

        static void Main(string[] args)
        {
            int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31,
                             37, 41, 43, 47, 53, 59, 61, 67, 71 };

            int iterations = 1000000;

            for (int dimension = 2; dimension < 13; dimension++)
            {
                double count = 0;

                for (int i = 0; i < iterations; i++)
                {
                    double distance = 0;

                    for (int d = 0; d < dimension; d++)
                    {
                        double v = Halton(i, primes[d]);

                        distance = distance + v * v;

                        if (distance > 1.0)
                            break;
                    }

                    if (distance <= 1.0)
                        count++;
                }

                double volume = count / iterations * Pow(2, dimension);

                WriteLine($"{dimension:D2}, {volume:F6}");
            }

            Console.WriteLine();
            if (Debugger.IsAttached)
            {
                Console.Write("Press any key to continue . . . ");
                Console.ReadKey();
            }
        }
    }
}
