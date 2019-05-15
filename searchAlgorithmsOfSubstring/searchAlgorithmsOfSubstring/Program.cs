using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace searchAlgorithmsOfSubstring
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();

            IStringSearcher rabin_Karp = new RabinKarp();
            IStringSearcher boyer_Moor = new Boyer_Moore();

            string anna = string.Empty;

            using (StreamReader reader = new StreamReader("anna.txt"))
            {
                anna = reader.ReadToEnd();
            }

            timer.Start();
            IEnumerable<int> resRK = rabin_Karp.Find(anna, "Анна");
            timer.Stop();
            var timeRK = timer.ElapsedMilliseconds;

            timer.Reset();
            timer.Start();
            IEnumerable<int> resBM = boyer_Moor.Find(anna, "Анна");
            timer.Stop();

            var timeBM = timer.ElapsedMilliseconds;

            Console.WriteLine($"Бойер-Мур: время: {timeBM} | вхождения: {resBM.Count()}\n" +
                $"Рабин-Карп время: {timeRK} | вхождения: {resRK.Count()}");
        }
    }
}
