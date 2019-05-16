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
            int count = 10000;
            AVLTree<int, int> tree = new AVLTree<int, int>();
            Random random = new Random();
            int[] numbers = new int[count];
            SortedDictionary<int, int> keyValuePairs = new SortedDictionary<int, int>();
            Stopwatch t = new Stopwatch();

            for (int i = 0; i < count; i++)
            {
                numbers[i] = random.Next(count);
            }

            t.Start();

            for( int i = 0; i < count; i++)
                tree.Insert(i, numbers[i]);

            for (int i = 5000; i < 7000; i++)
                tree.Remove(i);

            for( int i = 0; i < count; i++)
                tree.Find(i);

            t.Stop();

            Console.WriteLine($"AVL time: {t.ElapsedMilliseconds}");

            t.Reset();
            t.Start();

            for(int i = 0; i < count; i++)
                keyValuePairs.Add(i, numbers[i]);

            for (int i = 5000; i < 7000; i++)
                keyValuePairs.Remove(i);

            for (int i = 0; i < count; i++)
                keyValuePairs.ContainsKey(i);

            t.Stop();
            Console.WriteLine($"SortedDictionary time: {t.ElapsedMilliseconds}");
        }
    }
}
