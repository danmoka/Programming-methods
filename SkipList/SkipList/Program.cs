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
            const int n = 10000;
            int testTimes = 20;
            const int startIndexToRemove = 5000;
            const int endIngexToRemove = 7000;
            int[] numbers = GetNumbers(n).ToArray();

            double avSkipTime = 0;
            double avSortedTime = 0;

            for (int i = 0; i < testTimes; i++)
            {
                IAction<int, int> skipList = new SkipList<int, int>();
                IAction<int, int> sortedList = new SortedListAdapter<int, int>();

                avSkipTime += TestForIAction(startIndexToRemove, endIngexToRemove, numbers, skipList);
                avSortedTime += TestForIAction(startIndexToRemove, endIngexToRemove, numbers, sortedList);
            }

            avSkipTime = avSkipTime / testTimes;
            avSortedTime = avSortedTime / testTimes;
            var diff = avSortedTime / avSkipTime;

            Console.WriteLine("Skiplist: {0}mc faster then sortedlist: {1}mc by {2} times", avSkipTime, avSortedTime, string.Format("{0:N2}", diff));

            //SimpleTest();
        }

        private static double TestForIAction(int startIndexToRemove, int endIngexToRemove, int[] numbers, IAction<int, int> list)
        {
            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < numbers.Length; i++)
                list.Add(numbers[i], 1);

            for (int i = startIndexToRemove; i < endIngexToRemove; i++)
                list.Remove(numbers[i]);

            for (int i = 0; i < numbers.Length; i++)
                list.ContainsKey(numbers[i]);

            timer.Stop();

            return timer.ElapsedMilliseconds;
        }

        private static int[] GetNumbers(int n)
        {
            int[] array = new int[n];
            var range = 100000;
            var unusedNumbers = Enumerable.Range(0, range).ToList();
            var rand = new Random();

            for (int i = 0; i < n; i++)
            {
                var pos = rand.Next(0, unusedNumbers.Count);
                array[i] = unusedNumbers[pos];
                unusedNumbers.RemoveAt(pos);
            }

            return array;
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
