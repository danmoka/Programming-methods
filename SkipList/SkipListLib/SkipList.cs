using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*TODO:
 Допишите   класс SkipList<TKey, TValue>, который содержит открытые методы: 
 удаления элемента, поиска элемента по ключу, вставки элемента и необходимые внутренние методы.
 */
namespace SkipListLib
{
    /// <summary>
    /// Данный класс представляет собой узел структуры SkipList
    /// </summary>
    /// <typeparam name="TKey"> Ключ </typeparam>
    /// <typeparam name="TValue"> Значение </typeparam>
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public bool IsEmpty { get; set; } = true; // метка, что узел является пустым
        public Node<TKey, TValue> Next = null, Up = null, Down = null; // ссылки на элементы: на этом уровне (следующий), выше и ниже

        public Node() { }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            IsEmpty = false;
        }

        /// <summary>
        /// Вывод пары ключ - значение
        /// </summary>
        /// <returns> Строка ключ - значение</returns>
        public string Print()
        {
            return  string.Join(" ",new string[2] { Key.ToString(), Value.ToString() });
        }
    }
    /// <summary>
    /// Класс, реализующий структуры данных СкипЛист
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SkipList<TKey, TValue>:IAction<TKey, TValue> where TKey : IComparable<TKey>
    {
        /*Список с пропусками (англ. Skip List) — вероятностная структура данных, основанная на
          нескольких параллельных отсортированных связных списках с эффективностью,
          сравнимой с двоичным деревом (порядка O(log n) среднее время для большинства
          операций).Вставка, поиск и удаление выполняются за логарифмическое случайное время.*/

        public int Count { get; private set; } // количество элементов в СкипЛисте
        private int _maxLevel; // максимально возможный уровень "башни"
                               // чем больше уровень, тем более список на этом уровне разрежен по сравнению с нижележащими.
                               //Эта разреженность наряду с вероятностной природой дает оценку сложности поиска O(log N), — такую же, 
                               //как для бинарных самобалансирующихся деревьев.
        private int _curLevel; // текущей уровень заполненности
        private double _probability; // элемент в i-м слое появляется в i+1-м слое с некоторой фиксированной вероятностью p = _probability. 
                                     // В среднем каждый элемент встречается в 1/(1-p) списках,
        private Random _rd;

        private Node<TKey, TValue>[] _head; // "башня - голова"
        private Node<TKey, TValue> _tail; // "башная - хвост"

        public SkipList(int maxLevel = 10, double p = 0.5)
        {
            _maxLevel = maxLevel;
            _probability = p;
            _curLevel = 0;
            _rd = new Random();

            _head = new Node<TKey, TValue>[_maxLevel];
            _tail = new Node<TKey, TValue>();

            _head[0] = new Node<TKey, TValue>
            {
                // голову на 0 уровне вынесем отдельно, чтобы _maxLevel раз не проверять условие if (i == 0)
                Next = _tail
            };

            for (int i = 1; i < _maxLevel; i++)
            {
                _head[i] = new Node<TKey, TValue>
                {
                    Next = _tail // связываем голову с хвостом
                };

                _head[i].Down = _head[i - 1];
                _head[i - 1].Up = _head[i];
            }
        }

        /// <summary>
        /// Метод поиска узла
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"> Ключ, по которому ищем искомый узел</param>
        /// <returns> Искомый узел </returns>
        private Node<TKey, TValue> Find(Node<TKey, TValue> node, TKey key)
        {
            /*  Ожидаемое число шагов
                в каждом связном списке 1/p, что можно увидеть просматривая путь поиска назад с
                целевого элемента пока не будет достигнут элемент, который появляется в следующем
                более высоком списке. Таким образом, общие ожидаемые затраты на поиск — (log1/p(n))/p = O(log(n))
                в случае константного p.
             */
            Node<TKey,TValue> currentNode = node;

            // пока не дошли до хвоста и ключ меньше искомого...
            while (currentNode.Next != _tail && currentNode.Next.Key.CompareTo(key) < 0)
                currentNode = currentNode.Next; // переходим к следующему на текущем уровне

            if (currentNode.Down == null || (currentNode.Key.CompareTo(key) == 0 && !currentNode.IsEmpty))
                return currentNode;

            return Find(currentNode.Down, key);
        }

        public string Find(TKey key)
        {
            var node = Find(_head[_curLevel], key);
            return node == null ? "Key not found" : node.Print();
        }

        /// <summary>
        /// Данный метод добавляет элемент в СкипЛист
        /// </summary>
        /// <param name="key"> По ключу </param>
        /// <param name="value"> С данным зачением </param>
        public void Add(TKey key, TValue value)
        {
            var previousItems = new Node<TKey, TValue>[_maxLevel]; // элементы, которые будут ссылаться на нашу новую "башню"
            var currentNode = _head[_curLevel]; // верхний узел "башни - головы"

            for (int i = _curLevel; i >= 0; i--)
            {
                while (currentNode.Next != _tail && currentNode.Next.Key.CompareTo(key) < 0)
                    currentNode = currentNode.Next;

                // если нашли такой ключ, то...
                if (currentNode.Next.Key.CompareTo(key) == 0 && !currentNode.Next.IsEmpty)
                    // бросаем исключение, что такой элемент уже есть
                    throw new ArgumentException("Key must be unique");

                previousItems[i] = currentNode;
                currentNode = currentNode.Down;
            }      
            int height = 0; // высота башни для нового элемента

            // подбрасываем монетку, чтобы понять на какую высоту поднимется новый узел 
            while (_rd.NextDouble() < _probability && height < _maxLevel - 1)
                height++;

            // дублируем элементы из "башни - головы" на разницу высот,т.е. если высота больше текущего  уровня в SkipList, добавим недостающие элементы
            if (height > _curLevel)
            {
                for (int i = _curLevel + 1; i <= height; i++)
                    previousItems[i] = _head[i];

                _curLevel = height;
            }

            Node<TKey, TValue> newItem = new Node<TKey, TValue>(key, value)
            {
                Next = previousItems[0].Next
            };

            previousItems[0].Next = newItem;

            // вставляем новый узел на все уровни от нижнего до height. Перекидываем ссылки ->  -> ^ |
            for (int i = 1 ; i <= height; i++)
            {
                newItem = new Node<TKey, TValue>(key, value)
                {
                    Next = previousItems[i].Next
                };

                previousItems[i].Next = newItem;

                newItem.Down = previousItems[i - 1].Next;
                previousItems[i - 1].Next.Up = newItem;
            }
            Count++;
        }

        /// <summary>
        /// Данный метод выполняет поиск по ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns> True, если элемент содержится в коллекции, false - иначе </returns>
        public bool ContainsKey(TKey key)
        {
            Node<TKey, TValue> node = Find(_head[_curLevel], key);
            return node.Key.CompareTo(key) == 0 ? true : false;
        }

        public void Remove(TKey key)
        {
            Remove(_head[_curLevel], key);
            Count--;
        }

        private void Remove(Node<TKey, TValue> node, TKey key)
        {
            Node<TKey, TValue> currentNode = node;

            while (currentNode.Next != _tail && currentNode.Next.Key.CompareTo(key) < 0)
                currentNode = currentNode.Next;

            if (currentNode.Down != null)
                Remove(currentNode.Down, key);

            // перекидываем ссылки
            if (currentNode.Next != _tail && currentNode.Next.Key.CompareTo(key) == 0)
                currentNode.Next = currentNode.Next.Next;
        }

        public void Print()
        {
            for (int i = _curLevel; i >= 0; i--)
            {
                var currentNode = _head[i].Next;

                while (currentNode != _tail)
                {
                    Console.Write("{0} ",currentNode.Key);
                    currentNode = currentNode.Next;
                }

                Console.WriteLine();
            }
        }
    }
}
