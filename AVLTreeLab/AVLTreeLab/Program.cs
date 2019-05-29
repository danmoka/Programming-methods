using AVLTreeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTreeLab
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1000000, startDel = 500000, endDel = 700000;
            AVLTree<int, int> tree = new AVLTree<int, int>();
            Random random = new Random();
            SortedDictionary<int, int> keyValuePairs = new SortedDictionary<int, int>(); // red-black tree
            Stopwatch t = new Stopwatch();
            HashSet<int> _hash = new HashSet<int>();

            while(_hash.Count != count)
            {
                _hash.Add(random.Next(count));
            }

            var values = _hash.ToArray();
            
            t.Start();

            for( int  i = 0; i < count; i++)
                keyValuePairs.Add(values[i], values[i]);

            for (int i = startDel; i < endDel; i++)
                keyValuePairs.Remove(values[i]);

            for (int i = 0; i < count; i++)
                keyValuePairs.ContainsKey(values[i]);

            t.Stop();
            Console.WriteLine($"SortedDictionary time: {t.ElapsedMilliseconds}, Count: {keyValuePairs.Count}");

            t.Reset();
            t.Start();

            for (int i = 0; i < count; i++)
                tree.Insert(values[i], values[i]);

            for (int i = startDel; i < endDel; i++)
                tree.Remove(values[i]);

            for (int i = 0; i < count; i++)
                tree.Find(values[i]);

            t.Stop();

            Console.WriteLine($"AVL time: {t.ElapsedMilliseconds}, Count: {tree.Count}");

            //foreach (var el in tree.Elements())
            //    Console.WriteLine(el.Value);
        }
    }
}
