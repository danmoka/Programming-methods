using HashTableLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HashTableLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex regex = new Regex(@"\p{L}+");
            MatchCollection _matches = regex.Matches(new StreamReader("WarAndWorld.txt").ReadToEnd());

            string[] _words = _matches.Cast<Match>().Select(v => v.Value.ToString().ToLower()).ToArray(); //LINQ запрос, перевод из Matches в string[]

            Stopwatch timer = new Stopwatch();

            var hashTable = new OpenAddressHashTable<string, int>();
            timer.Start();
            TestingHashTable(hashTable, _words);
            timer.Stop();

            Console.WriteLine($"Total time for OAHT is {timer.ElapsedMilliseconds}");

            var dict = new Dictionary<string, int>();
            timer.Reset();
            timer.Start();
            TestingDict(dict, _words);
            timer.Stop();

            Console.WriteLine($"Total time for Dictionary is {timer.ElapsedMilliseconds}");

            //FirstHashMaker<int> first = new FirstHashMaker<int>();
            //Console.WriteLine(first.GetHash(2000000));

            //SecondHashMaker<int> second = new SecondHashMaker<int>();
            //Console.WriteLine(second.GetHash(20000000));
        }

        private static void TestingHashTable(OpenAddressHashTable<string, int> hashTable, string[] words)
        {
            foreach (var el in words)
            {
                if (hashTable.Contains(el))
                {
                    hashTable[el] = hashTable[el] + 1;
                }
                else
                {
                    hashTable.Add(el, 0);
                }
            }

            List<string> words7 = new List<string>();

            foreach (var pair in hashTable)
                if (pair.Key.Length == 7)
                    words7.Add(pair.Key);

            foreach (var el in words7)
                    hashTable.Remove(el);
        }

        private static void TestingDict(Dictionary<string, int> dictionary, string[] words)
        {
            foreach (var el in words)
            {
                if (dictionary.ContainsKey(el))
                {
                    dictionary[el] = dictionary[el] + 1;
                }
                else
                {
                    dictionary.Add(el, 0);
                }
            }

            List<string> words7 = new List<string>();

            foreach (var pair in dictionary)
                if (pair.Key.Length == 7)
                    words7.Add(pair.Key);

            foreach (var el in words7)
            {
                dictionary.Remove(el);
            }
        }
    }
}
