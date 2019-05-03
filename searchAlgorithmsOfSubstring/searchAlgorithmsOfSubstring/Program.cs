using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchAlgorithmsOfSubstring
{
    class Program
    {
        static void Main(string[] args)
        {
            RabinKarp karp = new RabinKarp();

            foreach (var el in karp.Find("qwertyuiop[asdfghjkl;zxcvbnm,wertyuiopsdfghjmk,zxabcabvbn m", "ab"))
            {
                Console.WriteLine(el);
            }
        }
    }
}
