using BinaryHeapLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*TODO:
 * Нужно разложить n объектов, каждый весом от нуля до одного килограмма, 
 * в наименьшее количество контейнеров, максимальная емкость каждого из которых не больше одного килограмма. 
 * Используйте алгоритм "первый лучший": объекты рассматриваются в исходном порядке 
 * и каждый объект помещается в частично заполненный контейнер, 
 * в котором после помещения данного объекта  останется наименьший свободный объем. 
 * Если такого контейнера нет, то объект помещается в новый(пустой) контейнер. Время исполнения данного алгоритма O(nlogn). 
 * На входе последовательность из n весов, на выходе - количество контейнеров.
 */

namespace BinaryHeapLab
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 10;
            Random rd = new Random();
            Queue<double> goodsWeight = new Queue<double>(); // веса товаров
            goodsWeight.Enqueue(0.6);
            goodsWeight.Enqueue(0.3);
            goodsWeight.Enqueue(0.4);
            goodsWeight.Enqueue(0.7);

            //for (int i = 0; i < n; i++)
            //    goodsWeight.Enqueue(Math.Round(rd.NextDouble(), 3));

            Console.Write("Weights on conveyor: ");

            foreach (var oneWeight in goodsWeight)
                Console.Write(" {0} |", oneWeight);

            BinaryHeap<double> boxes = new BinaryHeap<double>(); // коробки для упаковки

            boxes.Add(goodsWeight.Dequeue()); // + проверка на отсутствие товара

            while(goodsWeight.Count > 0)
            {
                var oneWeight = goodsWeight.Dequeue();
                var buffer = new List<double>(); // буфер для хранения извлеченных максимумов кучи

                while(boxes.Count > 0)
                {
                    var oneBox = boxes.Pop();

                    // если вес товара умещается в коробку, то...
                    if ( oneBox + oneWeight <= 1 )
                    {
                        // добавляем коробку с новым весом, вместо извлеченной
                        boxes.Add(oneBox + oneWeight);
                        break;
                    }
                    // иначе...
                    else
                    {
                        // запоминаем какой вес вытащили
                        buffer.Add(oneBox);
                    }
                }

                // если подходящая коробка не найдена, то...
                if (boxes.Count == 0)
                    // добавляем новую
                    boxes.Add(oneWeight);

                // добавляем все веса, которые запомнили
                foreach (var el in buffer)
                    boxes.Add(el);
            }

            Console.Write("\nPackaging in {0} boxes completed: ", boxes.Count);

            foreach (var box in boxes.Elements())
                Console.Write(" {0} |", box);

        }
    }
}
