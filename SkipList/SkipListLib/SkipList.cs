using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool IsEmpty { get; set; } = false; // метка, что узел является пустым
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
    public class SkipList<TKey, TValue> where TKey : IComparable<TKey>
    {
        public int Count { get; private set; } // количество элементов в СкипЛисте
        private int _maxLevel; // максимально возможный уровень "башни"
        private int _curLevel; // текущей уровень заполненности
        private double _probability; // элемент в i-м слое появляется в i+1-м слое с некоторой фиксированной вероятностью _probability 
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
            Node<TKey,TValue> currentNode = node;

            // пока не дошли до хвоста и ключ не больше искомого...
            while (currentNode.Next != _tail && currentNode.Next.Key.CompareTo(key) <= 0)
                currentNode = currentNode.Next; // переходим к следующему на текущем уровне

            // если успустились до нижнего уровня, то...
            if (currentNode.Down == null)
            {
                if (currentNode.Next == _tail)
                    return null;

                return currentNode; // элемент найден
            }

            // если ключи совпали и узел не помечен как пустой, то...
            if ((currentNode.Key.CompareTo(key) == 0 && !currentNode.IsEmpty))
                return currentNode; // элемент найден

            return Find(currentNode.Down, key); // иначе спускаемся на уровень ниже и продолжаем поиск
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

                if (currentNode.Next.Key.CompareTo(key) == 0)
                    throw new ArgumentException("Key must be unique");

                previousItems[i] = currentNode;
                currentNode = currentNode.Down;
            }

            // подбрасываем монетку, чтобы понять на какую высоту поднимется новый узел 
            int height = 0; // высота башни для нового элемента

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

            // вставляем новый узел на все уровни от нижнего до height
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

        public bool Contains(TKey key)
        {
            var node = Find(_head[_curLevel], key);

            return node != null;
        }

        public void Remove(TKey key)
        {
            Remove(_head[_curLevel], key);
            Count--;
        }

        private void Remove(Node<TKey, TValue> node, TKey key)
        {
            var current = node;

            while (current.Next != _tail && current.Next.Key.CompareTo(key) < 0)
                current = current.Next;

            if (current.Down != null)
                Remove(current.Down, key);

            // перекидываем ссылки
            if (current.Next != _tail && current.Next.Key.CompareTo(key) == 0)
                current.Next = current.Next.Next;
        }

        public void Print()
        {
            for (int i = _curLevel; i >= 0; i--)
            {
                var current = _head[i].Next;

                while (current != _tail)
                {
                    Console.Write("{0} ",current.Key);
                    current = current.Next;
                }

                Console.WriteLine();
            }
        }
    }
}
