using SkipListLib;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*TODO:
 * Напишите консольное приложение, которое  генерирует массив из случайных 10000 чисел; 
   -добавляет эти числа в Ваш список с пропусками;  удаляет  из списка числа которые находились в массиве на месте с 5000 до 7000; 
    выполняет поиск каждого элемента массива и замеряет общее время;
   - создайте SortedList на основе чисел массива. Также удалите из этого контейнера числа, 
    которые находились в массиве на месте с 5000 до 7000 и замерьте общее время работы с SortedList. 

 */
namespace SkipListLab
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 10000;
            int startIndexToRemove = 5000;
            int endIngexToRemove = 7000;
            int[] numbers = GetNumbers(n).ToArray();

            SkipList<int,int> skipList = new SkipList<int, int>();
            var skipTime = TestForSkipList(startIndexToRemove, endIngexToRemove, numbers, skipList);
            IDictionary<int, int> sortedList = new SortedList<int, int>();
            var sortedTime = TestForSortedList(startIndexToRemove, endIngexToRemove, numbers, sortedList);
            var diff = sortedTime / skipTime;

            Console.WriteLine("Skiplist: {0}mc faster then sortedlist: {1}mc by {2} times", skipTime, sortedTime, string.Format("{0:N2}", diff));

            //SimpleTest();
        }

        private static double TestForSkipList(int startIndexToRemove, int endIngexToRemove, int[] numbers, SkipList<int, int> skipList)
        {
            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < numbers.Length; i++)
                skipList.Add(numbers[i], 1);

            for (int i = startIndexToRemove; i < endIngexToRemove; i++)
                skipList.Remove(numbers[i]);

            for (int i = 0; i < numbers.Length; i++)
                skipList.ContainsKey(numbers[i]);

            timer.Stop();

            return timer.ElapsedMilliseconds;
        }

        private static double TestForSortedList(int startIndexToRemove, int endIngexToRemove, int[] numbers, IDictionary<int, int> sortedList)
        {
            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < numbers.Length; i++)
                sortedList.Add(numbers[i], 1);

            for (int i = startIndexToRemove; i < endIngexToRemove; i++)
                sortedList.Remove(numbers[i]);

            for (int i = 0; i < numbers.Length; i++)
                sortedList.ContainsKey(numbers[i]);

            timer.Stop();

            return timer.ElapsedMilliseconds;
        }

        private static IEnumerable<int> GetNumbers(int n)
        {
            HashSet<int> uniqNumbs = new HashSet<int>();
            Random rd = new Random();
            int[] array = new int[n];

            while (uniqNumbs.Count != n)
                uniqNumbs.Add(rd.Next());

            foreach (var el in uniqNumbs)
                yield return el;
        }

        private static void SimpleTest()
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
