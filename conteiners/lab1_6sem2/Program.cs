using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
/*Напишите метод, который читает весь текст из файла WarAndWorld.txt, разбивает
его на слова. Далее вычисляет сколько уникальных слов содержится в тексте и вычисляет
первые 10 часто встречающихся слов. При этом используется контейнер SortedList.
Напишите еще три модификации этого же метода с использованием в качестве
контейнера для данных SortedDictionary, List, Dictionary. Для двух последних надо
вспомнить лямбда-выражения или LINQ-запросы для выполнения задания.
Напишите консольное приложение которое вызывает написанные Вами методы и
замеряет время работы.*/
namespace lab1_6sem2
{
    public class MyTuple
    {
        //Класс для хранения слова и кол-ва повторений этого слова в тексте
        public string Word { get; set; }
        public int Count { get; set; }

        public MyTuple(string word,int count)
        {
            Word = word;
            Count = count;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Stopwatch();

            #region Structures
            SortedList<string, int> pairsSList = new SortedList<string, int>();
            SortedDictionary<string, int> pairsSDict = new SortedDictionary<string, int>();
            List<MyTuple> pairsList = new List<MyTuple>();
            Dictionary<string, int> pairsDict = new Dictionary<string, int>();
            #endregion

            #region WordFinder
            Regex regex = new Regex(@"\p{L}+");
            MatchCollection _matches = regex.Matches(new StreamReader("WarAndWorld.txt").ReadToEnd());

            string[] _words = _matches.Cast<Match>().Select(v => v.Value.ToString().ToLower()).ToArray(); //LINQ запрос, перевод из Matches в string[]
            #endregion

            #region SortedList
            Console.WriteLine("-----SortedList-----");

            t.Start(); 
            CommonMethod(pairsSList, _words);
            t.Stop();

            Console.WriteLine("Время работы: {0}", t.ElapsedMilliseconds);
            #endregion

            #region SortedDictionary
            t.Reset();
            Console.WriteLine("-----SortedDictionary-----");

            t.Start();
            CommonMethod(pairsSDict, _words);
            t.Stop();

            Console.WriteLine("Время работы: {0}", t.ElapsedMilliseconds);
            #endregion

            #region List
            t.Reset();
            Console.WriteLine("-----List-----");

            t.Start();
            ListMethod(pairsList, _words);
            t.Stop();

            Console.WriteLine("Время работы: {0}", t.ElapsedMilliseconds);
            #endregion

            #region Dictionary
            t.Reset();
            Console.WriteLine("-----Dictionary-----");

            t.Start();
            CommonMethod(pairsDict, _words);
            t.Stop();

            Console.WriteLine("Время работы: {0}", t.ElapsedMilliseconds);
            #endregion
        }
        #region UsingMet
        private static void CommonMethod(IDictionary<string, int> pairs, string[] words)
        {
            foreach (var el in words)
            {
                if (!pairs.ContainsKey(el)) pairs.Add(el, 1);
                else pairs[el]++;
            }

            var spairs = pairs.OrderByDescending(v => v.Value);
            WriteAnAnswer(spairs);
        }

        private static void ListMethod(List<MyTuple> pairsList, string[] words)
        {
            var t = new Stopwatch();
            t.Start();
            foreach (var el in words)
            {
                var lin = pairsList.Find(v => v.Word == el);

                if(lin!=null) lin.Count++;
                else pairsList.Add(new MyTuple(el, 1));

                //if (!pairsList.Any(v => v.Word == el)) pairsList.Add(new MyTuple(el, 1));
                //else pairsList.Find(v => v.Word == el).Count++;
            }
            t.Stop();
            Console.WriteLine("Время работы вставки: {0}", t.ElapsedMilliseconds);

            t.Reset();
            t.Start();
            var spairs = pairsList.OrderByDescending(v => v.Count);
            t.Stop();
            Console.WriteLine("Время работы сортировки: {0}", t.ElapsedMilliseconds);

            #region WriteAnswer
            Console.WriteLine("Первые 10 уникальных: ");

            for (int i = 0; i < 10; i++) Console.WriteLine(spairs.ElementAt(i).Word);

            Console.WriteLine("Кол-во уникальных: {0}", spairs.Count());
            #endregion
        }

        private static void WriteAnAnswer(IOrderedEnumerable<KeyValuePair<string, int>> spairs)
        {
            Console.WriteLine("Первые 10 уникальных: ");

            for (int i = 0; i < 10; i++) Console.WriteLine(spairs.ElementAt(i).Key);

            Console.WriteLine("Кол-во уникальных: {0}", spairs.Count());
        }
        #endregion

        #region NotCommonMet
        //private static void DictMethod(Dictionary<string, int> pairsDict, MatchCollection _matches)
        //{
        //    foreach (var el in _matches)
        //    {
        //        string elstr = el.ToString().ToLower();
        //        if (!pairsDict.ContainsKey(elstr)) pairsDict.Add(elstr, 1);
        //        else pairsDict[elstr]++;
        //    }

        //    var spairs = pairsDict.OrderByDescending(v => v.Value);

        //    WriteAnAnswer(spairs);
        //}

        //private static void SortedDictMethod(SortedDictionary<string, int> pairs, MatchCollection _matches)
        //{
        //    foreach (var el in _matches)
        //    {
        //        string elstr = el.ToString().ToLower();
        //        if (!pairs.ContainsKey(elstr)) pairs.Add(elstr, 1);
        //        else pairs[elstr]++;
        //    }

        //    var spairs = pairs.OrderByDescending(v => v.Value);
        //    WriteAnAnswer(spairs);
        //}

        //private static void SortedListMethod(SortedList<string, int> pairs, MatchCollection _matches)
        //{
        //    foreach (var el in _matches)
        //    {
        //        string elstr = el.ToString().ToLower();
        //        if (!pairs.ContainsKey(elstr)) pairs.Add(elstr, 1);
        //        else pairs[elstr]++;
        //    }

        //    var spairs = pairs.OrderByDescending(v => v.Value);
        //    WriteAnAnswer(spairs);
        //}
        #endregion
    }
}
