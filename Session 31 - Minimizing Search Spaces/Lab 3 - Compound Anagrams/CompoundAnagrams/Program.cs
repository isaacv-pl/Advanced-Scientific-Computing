using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace FindAnagrams
{
    class Anagram
    {
        public List<string> words = new List<string>();
        public string letters;

        public Anagram(string phrase)
        {
            char[] seperators = new char[] { ' ', '_', '-', '\t', '.', '(', ')', ',', '\'', '%' };
            string[] words = phrase.ToLower().Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            phrase = string.Empty;
            foreach (var word in words)
            {
                this.words.Add(word);
                phrase += word;
            }

            this.letters += string.Concat(phrase.OrderBy(c => c));
        }

        public Anagram(List<string> words)
        {
            string phrase = string.Empty;
            foreach (var word in words)
            {
                this.words.Add(word);
                phrase += word;
            }
            this.letters += String.Concat(phrase.OrderBy(c => c));
        }
    }

    class Program
    {
        static Anagram input;
        static StreamWriter sw;

        static bool DisjointContains(string a, string b)
        {
            if (a.Length > b.Length)
            {
                string c = a;
                a = b;
                b = c;
            }
            int j = 0;
            for (int i = 0; i < a.Length; i++)
            {
                j = b.IndexOf(a[i], j);
                if (j == -1) return false;
                j++;
            }
            return true;
        }

        static void WriteMatches(List<Anagram> list)
        {
            var words = from anagram in list
                        where anagram.letters == input.letters
                        select anagram.words;

            foreach (var word in words)
            {
                word.ForEach(s => sw.Write($"{s} "));
                sw.WriteLine();

                word.ForEach(s => Console.Write($"{s} "));
                Console.WriteLine();
            }

            sw.WriteLine();
        }

        static void Main(string[] args)
        {
            //string phrase = "Dormitory";
            string phrase = "Software";
            //phrase = "Mother-in-law";

            Console.WriteLine($"Finding anagrams for {phrase} . . .");

            input = new Anagram(phrase);

            sw = new StreamWriter(
                Path.Combine(Environment.CurrentDirectory, "anagrams.txt"),
                false, Encoding.ASCII);
            sw.WriteLine($"Anagrams for {phrase}:");

            // Find all anagrams with perfect match
            List<Anagram> anagramsAll = new List<Anagram>();
            List<string> dict = File.ReadAllLines("english_dictionary.txt").ToList<string>();
            dict.ForEach(word => anagramsAll.Add(new Anagram(word)));

            WriteMatches(anagramsAll);

            // Find anagrams with partial (disjoint substring) match
            List<Anagram> anagramsPartial = (from anagram in anagramsAll
                                             where DisjointContains(anagram.letters, input.letters)
                                             select anagram).ToList<Anagram>();

            // Form composite list from the union of the set
            // of partial matches with itself (a first order self-join)
            List<Anagram> anagramsCompound = new List<Anagram>();
            foreach (Anagram a1 in anagramsPartial)
                foreach (Anagram a2 in anagramsPartial)
                    if (a1.words[0].Length + a2.words[0].Length == input.letters.Length)
                        anagramsCompound.Add(new Anagram(new List<string> { a1.words[0], a2.words[0] }));

            WriteMatches(anagramsCompound);

            sw.Flush();
            sw.Close();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("\nPress any key to continue . . .");
                Console.ReadKey();
            }
        }
    }
}
