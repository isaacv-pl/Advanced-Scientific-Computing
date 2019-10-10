using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Combinations
{
    class Program
    {
        static int total = 0;

        static void PrintElements<T>(T[] set)
        {
            for (int i = 0; i < set.Length - 1; i++)
                Console.Write("{0}, ", set[i]);
            Console.WriteLine(set[set.Length - 1]);
        }

        static void PrintElementsByIndex<T>(T[] set, int subsetLength, int[] itemIndexes)
        {
            for (int i = 0; i < subsetLength - 1; i++)
                Console.Write("{0}, ", set[itemIndexes[i]]);
            Console.WriteLine(set[itemIndexes[subsetLength - 1]]);
        }

        static void Swap<T>(T[] set, int a, int b)
        {
            T tmp = set[a];
            set[a] = set[b];
            set[b] = tmp;
        }

        static void Permutations<T>(T[] set, int level)
        {
            // Recursive Heap's Algorithm
            if (level == 0)
            {
                PrintElements(set);
                total++;
            }
            else
            {
                for (int i = 0; i < level; i++)
                {
                    Permutations(set, level - 1);
                    Swap(set, level % 2 == 1 ? 0 : i, level - 1);
                }
            }
        }

        static void Combinations<T>(T[] set, int subsetLength, 
            int[] itemIndexes = null, int level = 0)
        {
            // Recursive Biersach's Algorithm
            if (itemIndexes == null)
                itemIndexes = new int[set.Length];

            if (level == (subsetLength - 1))
            {
                while (itemIndexes[level] < itemIndexes.Length)
                {
                    PrintElementsByIndex(set, subsetLength, itemIndexes);
                    total++;
                    itemIndexes[level]++;
                }
            }
            else
            {
                while (itemIndexes[level] <
                    (itemIndexes.Length - subsetLength + level + 1))
                {
                    itemIndexes[level + 1] = itemIndexes[level] + 1;
                    Combinations(set, subsetLength, itemIndexes, level + 1);
                    itemIndexes[level]++;
                }
            }
        }


        static void Main(string[] args)
        {
            // Permutations
            total = 0;
            string[] names = new[] { "Dave", "Laura", "Brett", "Hope", "Bob" };
            Permutations(names, names.Length);
            Console.WriteLine("Total permutations = {0}\n", total);

            // Combinations
            total = 0;
            string[] letters = new[] { "a", "b", "c", "d", "e" };
            Combinations(letters, 3);
            Console.WriteLine("Total combinations = {0}\n", total);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.Write("Press any key to continue . . . ");
                Console.Read();
            }
        }
    }
}
