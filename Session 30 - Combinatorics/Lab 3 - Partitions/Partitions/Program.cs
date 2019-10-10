using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using static System.Math;
using static System.Console;

namespace GeneratePartitionsRecursive
{
    class Program
    {
        static void Partition(int n, int s, int[] p, List<int[]> parts, bool square)
        {
            if (n == s)
            {
                for (int i = 1; i <= s; i++) p[i]++;
                if (square && !p.ToList<int>().All(x => Sqrt(x) % 1 == 0)) return;
                parts.Add(p);
            }
            else
            {
                for (int i = 1; i <= s; i++)
                {
                    int[] p2 = new int[p.Length];
                    for (int r = 1; r < p.Length; r++)
                        p2[r] = (r > s) ? p[r] : p[r] + 1;
                    if (i <= n - s)
                        Partition(n - s, i, p2, parts, square);
                }
            }
        }

        static void Main(string[] args)
        {
            int n = 6;
            int s = 2;
            bool square = false;

            /*int n = 48;
            int s = 10;
            bool square = true;*/

            bool display = true;

            List<int[]> parts = new List<int[]>();

            Partition(n, s, new int[s + 1], parts, square);

            WriteLine($"Partition {n} into size {s}, square = {square}");

            if (display)
                foreach (var p in parts)
                    WriteLine($"\t[{string.Join(",", p.Where(x => x > 0))}]");

            WriteLine($"Total partitions = {parts.Count}");

            if (Debugger.IsAttached)
            {
                Write("Press any key to continue . . .");
                ReadKey();
            }
        }
    }
}
