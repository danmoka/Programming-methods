using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*TODO:
 * Напишите  класс BinaryHeap<T>, который содержит открытые методы вставки элемента, 
 * удаления наибольшего(наименьшего) элемента, поиска наибольшего(наименьшего) элемента 
 * и необходимые закрытые методы( например, метод восстановления свойств пирамиды).
 */

namespace BinaryHeapLib
{   
    /// <summary>
    /// Данный класс реализует структуру - Бинарное дерево, где каждый предок не меньше своего сына (max - heap)
    /// </summary>
    public class BinaryHeap<T> where T : IComparable<T>
    {
        private List<T> _heap;
        public int Count
        {
            get
            {
                return _heap.Count();
            }
        }

        public BinaryHeap()
        {
            _heap = new List<T>();
        }

        private int Parent(int i)
        {
            // если вершина имеет индекс i, то её родитель имеет индекс [i/2]
            return (i - 1) / 2;
        }

        private int Left(int i)
        {
            // левый сын имеет индекс 2*i + 1
            return 2 * i + 1;
        }

        private int Right(int i)
        {
            // индекс правого сына
            return 2 * i + 2;
        }

        /// <summary>
        /// Метод добавления элемента в кучу
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            // Сложность метода не превышает высоты двоичной кучи (так как количество «подъемов» не больше высоты дерева), 
            //то есть равна O(log2 N).
            _heap.Add(value); // добавили значение в кучу
            int i = Count - 1;
            int parent = Parent(i); // индекс родителя добавленного элемента

            // пока родитель больше добавленного, добавленный "всплывает"
            while (i > 0 && _heap[parent].CompareTo(_heap[i]) < 0)
            {
                T temp = _heap[i];
                _heap[i] = _heap[parent];
                _heap[parent] = temp;

                i = parent; // добавленный теперь на родительском месте...
                parent = Parent(i); // и у него новый родитель
            }
        }

        /// <summary>
        /// Метод heapify восстанавливает основное свойство кучи для дерева с корнем в i-ой вершине при условии, 
        /// что оба поддерева ему удовлетворяют.
        /// </summary>
        /// <param name="i"></param>
        private void Heapify(int i)
        {
            /*Для этого необходимо «опускать» i-ую вершину (менять местами с наибольшим из потомков), 
             * пока основное свойство не будет восстановлено (процесс завершится, когда не найдется потомка, большего своего родителя). 
             * Cложность этого метода также равна O(log2 N).
             */

            while(true)
            {
                int leftChild = Left(i);
                int rightChild = Right(i);
                int largestChild = i;

                // Если левый ребенок, больше предка, то...
                if (leftChild < Count && _heap[leftChild].CompareTo(_heap[largestChild]) > 0)
                {
                    // наибольший - левый
                    largestChild = leftChild;
                }

                // Если правый ребенок больше предка, то...
                if (rightChild < Count && _heap[rightChild].CompareTo(_heap[largestChild]) > 0)
                {
                    // наибольший - правый
                    largestChild = rightChild;
                }

                // Если текущий - наибольший, то завершаем "опускание"
                if (largestChild == i)
                    break;

                T temp = _heap[i];
                _heap[i] = _heap[largestChild];
                _heap[largestChild] = temp;
                i = largestChild;
            }
        }

        /// <summary>
        /// Данный метод удаляет и возвращает максимальный элемент кучи
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T max = _heap[0];
            _heap[0] = _heap[Count - 1];
            _heap.RemoveAt(Count - 1);
            Heapify(0);

            return max;
        }

        public T Max()
        {
            return _heap[0];
        }

        public IEnumerable<T> Elements()
        {
            foreach (var el in _heap)
                yield return el;
        }
    }
}
