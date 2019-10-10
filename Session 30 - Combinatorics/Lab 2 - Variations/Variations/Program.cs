using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Variations
{
    class Program
    {
        static void PrintItems(int[] items, int subsetLength)
        {
            for (int t = 0; t < subsetLength - 1; t++)
                Console.Write("{0}, ", items[t]);
            Console.WriteLine(items[subsetLength - 1]);
        }

        static void Swap(int[] items, int a, int b)
        {
            int t = items[a];
            items[a] = items[b];
            items[b] = t;
        }

        // Heap's Algorithm - Recursive
        static void Permutations(int[] items, int level)
        {
            if (level == 0)
            {
                PrintItems(items, items.Length);
            }
            else
            {
                for (int i = 0; i < level; i++)
                {
                    Permutations(items, level - 1);
                    Swap(items, level % 2 == 1 ? 0 : i, level - 1);
                }
            }
        }

        // Biersach's Algorithm - Recursive
        static void Variations(int[] items, int subsetLength, int level = 0)
        {
            if (level == (subsetLength - 1))
            {
                while (items[level] < items.Length)
                {
                    Permutations(items.Take(subsetLength).ToArray(), subsetLength);
                    items[level]++;
                }
            }
            else
            {
                while (items[level] < (items.Length - subsetLength + level + 1))
                {
                    items[level + 1] = items[level] + 1;
                    Variations(items, subsetLength, level + 1);
                    items[level]++;
                }
            }
        }

        static void Main(string[] args)
        {
            Variations(Enumerable.Range(0, 6).ToArray(), 4);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.Write("Press any key to continue . . . ");
                Console.ReadKey();
            }
        }
    }
}
