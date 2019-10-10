using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BalancingIonicEquations
{
    class Level<T>
    {
        public List<T> items = new List<T>();
        public int pos = 0;
    }

    class Stack<T>
    {
        public List<Level<T>> levels = new List<Level<T>>();
        public int levelNum = 0;

        public void AddLevel(IEnumerable<T> items)
        {
            Level<T> newLevel = new Level<T>();
            newLevel.items = items.ToList<T>();
            levels.Add(newLevel);
        }

        public void PrintLevels(bool showValues = true)
        {
            if (showValues)
            {
                Console.Write("(");
                for (int i = 0; i < levels.Count - 1; i++)
                    Console.Write($"{levels[i].items[levels[i].pos]}, ");
                Console.WriteLine($"{levels[levels.Count - 1].items[levels[levels.Count - 1].pos]})");
            }
            else
            {
                Console.Write("(");
                for (int i = 0; i < levels.Count - 1; i++)
                    Console.Write($"{levels[i].pos}, ");
                Console.WriteLine($"{levels[levels.Count - 1].pos})");
            }
        }

        public T GetValue(int index)
        {
            return levels[index].items[levels[index].pos];
        }

        public List<T> GetValues()
        {
            List<T> values = new List<T>();
            foreach (Level<T> level in levels)
                values.Add(level.items[level.pos]);
            return values;
        }

        public void Iterate(Action<Stack<T>> func)
        {
            while (levelNum >= 0)
            {
                while (levels[levelNum].pos < levels[levelNum].items.Count)
                {
                    if (levelNum == levels.Count - 1) break;
                    levelNum++;
                    levels[levelNum].pos = 0;
                }

                func(this);

                levels[levelNum].pos++;

                while (levels[levelNum].pos == levels[levelNum].items.Count)
                {
                    levelNum--;
                    if (levelNum < 0) break;
                    levels[levelNum].pos++;
                }
            }
        }
    }

    class Program
    {
        static int equationToSolve = 1;

        static int objective = Int32.MaxValue;

        static void CheckConstraints(Stack<int> stack)
        {
            List<int> x = stack.GetValues();

            switch (equationToSolve)
            {
                case 1:
                    // Equation #1:  MnO4(-) + Fe(2+) + H(+) => Mn(2+) + Fe(3+) + H2O

                    if (x[2] != x[5]*2) return;//Hydrogen
                    if (x[0] * 4 != x[5]) return;//Oxygen
                    if (x[1] * 2 != x[4] * 3) return;//Iron
                    if (-x[0] != x[3] * 2) return;//Manganese
                    break;

                case 2:
                    // Equation #2:  MnO4(-) + H2O2 + H(+) => Mn(2+) + O2 + H2O
                    if (x[0] != x[3]) return;
                    if (x[0] * 4 + x[1] * 2 != x[4] * 2 + x[5]) return;
                    if (x[1] * 2 + x[2] != x[5] * 2) return;
                    if (-x[0] + x[2] != x[3] * 2) return;
                    break;

                case 3:
                    // Equation #3:  P2I4 + P4 + H2O => PH4I + H3PO4
                    if (2 * x[0] + 4 * x[1] != x[3] + x[4]) return;//Iodine
                    if (4 * x[0] != x[3]) return;//Phosphorous
                    if (2 * x[2] != 4 * x[3] + 3 * x[4]) return;//Hydrogen
                    if (x[2] != x[4] * 4) return;//Oxygen

                    break;
            }

            if (objective > x.Sum())
            {
                stack.PrintLevels();
                objective = x.Sum();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Solving equation {equationToSolve}:\n");

            Stack<int> stack = new Stack<int>();

            //For Equations #1 & #2
            if (equationToSolve == 1 || equationToSolve == 2)
                for (int levels = 0; levels < 6; levels++)
                    stack.AddLevel(Enumerable.Range(1, 9));

            // For Equation #3
            if (equationToSolve == 3)
            {
                stack.AddLevel(Enumerable.Range(1, 10));
                stack.AddLevel(Enumerable.Range(1, 13));
                stack.AddLevel(Enumerable.Range(1, 128));
                stack.AddLevel(Enumerable.Range(1, 40));
                stack.AddLevel(Enumerable.Range(1, 32));
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();

            stack.Iterate(f => CheckConstraints(stack));

            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine($"\nTotal run time: {elapsedTime}");

            if (Debugger.IsAttached)
            {
                Console.WriteLine("\nPress any key to continue . . .");
                Console.ReadKey();
            }
        }
    }
}
