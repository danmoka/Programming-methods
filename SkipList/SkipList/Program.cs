using SkipListLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipListLab
{
    class Program
    {
        static void Main(string[] args)
        {
            var lib = new SkipList<int, int>();
            lib.Add(9, 1);
            lib.Add(3, 1);
            lib.Add(5, 1);
            lib.Add(11, 1);
            lib.Add(0, 1);
            lib.Add(18, 1);
            lib.Add(25, 1);
            lib.Add(1, 1);
            lib.Print();
            Console.ReadKey();
        }
    }
}
