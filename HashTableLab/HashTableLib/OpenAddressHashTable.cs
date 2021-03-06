﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableLib
{
    public class OpenAddressHashTable<TKey, TValue> : IEnumerable<Pair<TKey, TValue>>, IHashTable<TKey, TValue> where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        Pair<TKey, TValue>[] table;
        private int _capacity; // размер таблицы

        public int Capacity
        {
            get { return _capacity; }
            private set { _capacity = value; }
        }

        private HashMaker<TKey> _hashMaker1, _hashMaker2; // хэш-функции
        public int Count { get; private set; } // заполненность
        private const double FillFactor = 0.85; // определяет, когда нужно увеличивать размер таблицы
        private readonly int[] _sizes = new int[20] {
            373, 877, 2791, 4447,
            9241, 19927, 35317,
            50023, 80039, 250013,
            499973, 999979, 5000077,
            8999993, 15000017, 29999989,
            55999997, 111999997, 223999879, 1999999927 };
        private int _sizeIndex = 0; // определяет индекс элемента из массива размеров таблицы

        public OpenAddressHashTable(int size)
        {
            table = new Pair<TKey, TValue>[size];
            _capacity = size;
            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);
            Count = 0;
        }

        public OpenAddressHashTable()
        {
            int size = _sizes[_sizeIndex];
            _capacity = size;
            table = new Pair<TKey, TValue>[size];
            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);
            Count = 0;
        }

        // O(1), в худшем O(N) - когда долго ищем местечко куда вставить
        public void Add(TKey key, TValue value)
        {
            var h = _hashMaker1.GetHash(key);

            if (!TryToPut(h, key, value)) // ячейка занята
            {
                // двойное исследование
                int iterationNumber = 1;
                while (true)
                {
                    var place = (h + iterationNumber * (1 + _hashMaker2.GetHash(key))) % _capacity;

                    // пробуем вставить, есои успешно, то...
                    if (TryToPut(place, key, value))
                        // выходим из двойного исследования
                        break;

                    iterationNumber++;

                    if (iterationNumber >= _capacity)
                        throw new ApplicationException("HashTable full!!!");
                }
            }
            // Если заполненность таблицы больше 85%, то...
            if ((double)Count / _capacity >= FillFactor)
            {
                // надо расширить таблицу
                IncreaseTable();
            }
        }

        /// <summary>
        /// Метод, возращающий true, если по заданному хэшу можно вставить пару
        /// </summary>
        /// <param name="place"> Хэш </param>
        /// <param name="key"> Ключ </param>
        /// <param name="value"> Значение </param>
        /// <returns></returns>
        private bool TryToPut(int place, TKey key, TValue value)
        {
            // Если в таблице по заданному значению null или удаленная пара, то...
            if (table[place] == null || table[place].IsDeleted())
            {
                // вставить новую пару ключ значение
                table[place] = new Pair<TKey, TValue>(key, value);
                Count++;
                return true;
            }
            if (table[place].Key.Equals(key))
            {
                throw new ArgumentException();
            }
            // Если в таблице по данному хэшу есть пара отличная от данной (по ключу), то вернуть false
            return false;
        }

        /// <summary>
        /// Этот метод определяет поиск по ключу
        /// </summary>
        /// <param name="key"> Ключ </param>
        /// <returns> Пара: ключ - значение </returns>
        /// O(1), в худшем O(N)
        private Pair<TKey, TValue> Find(TKey key)
        {
            var h = _hashMaker1.GetHash(key);
            // если по ключу, значение null
            if (table[h] == null)
                return null;

            // если элемент не помечен как удаленный и ключи совпадают, то...
            if (!table[h].IsDeleted() && table[h].Key.Equals(key))
            {
                // возвращаем элемент
                return table[h];
            }

            // остается проверить вариант, когда пара в ходе двойного исследования лежит в другом месте
            int iterationNumber = 1;

            while (true)
            {
                // двойное исследование
                var place = (h + iterationNumber * (1 + _hashMaker2.GetHash(key))) % _capacity;

                if (table[place] == null)
                    return null;

                if (!table[place].IsDeleted() && table[place].Key.Equals(key))
                {
                    return table[place];
                }
                iterationNumber++;

                if (iterationNumber >= Count)
                    return null;
            }
        }

        /// <summary>
        /// Индексатор
        /// </summary>
        /// <param name="key"> Ключ </param>
        /// <returns> Значение </returns>
        public TValue this[TKey key]
        {
            get { return Find(key).Value; } // возвращает значение

            set
            {
                var h = _hashMaker1.GetHash(key);

                if (table[h] == null)
                    throw new KeyNotFoundException();

                // Если нашли по хэшу значение и ключи совпадают, и значение не является удаленным то...
                if (!table[h].IsDeleted() && table[h].Key.Equals(key))
                {
                    // возвращаем значение
                    table[h].Value = value;
                    return;
                }

                int iterationNumber = 1;

                // Возможно, случилась коллизия и наше значение прячется по другому хэшу, тогда...
                while (true)
                {
                    // зная, что у нас двойное исследование , найдем значение(пару), по которому нужно вставить value
                    var place = (h + iterationNumber * (1 + _hashMaker2.GetHash(key))) % _capacity;
                    if (table[place] == null)
                        throw new KeyNotFoundException();

                    // Если пара не помечена как удаленная и ключи совпали, то...
                    if (!table[place].IsDeleted() && table[place].Key.Equals(key))
                    {
                        // присваиваем новое значение
                        table[place].Value = value;
                        return;
                    }
                    iterationNumber++;
                    if (iterationNumber >= Count)
                        throw new KeyNotFoundException();
                }
            }
        }

        private void IncreaseTable()
        {
            _sizeIndex++;
            int size = _sizes[_sizeIndex];
            _capacity = size;
            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);
            Count = 0;

            Pair<TKey, TValue>[] buffer = table;
            table = new Pair<TKey, TValue>[size];

            foreach (var el in buffer)
            {
                if (el != null)
                    Add(el.Key, el.Value);
            }
        }

        // O(N)
        public bool Contains(TKey key)
        {
            int h = _hashMaker1.GetHash(key);

            if (table[h] == null)
                return false;

            if (!table[h].IsDeleted() && table[h].Key.Equals(key))
            {
                return true;
            }

            int iterationNumber = 1;
            while (true)
            {
                var place = (h + iterationNumber * (1 + _hashMaker2.GetHash(key))) % _capacity;

                if (table[place] == null)
                    return false;

                // Если неудалена и ключи совпадают
                if (!table[place].IsDeleted() && table[place].Key.Equals(key))
                {
                    return true;
                }

                iterationNumber++;

                if (iterationNumber >= Count)
                    return false;
            }
        }

        public bool Remove(TKey key)
        {
            var pair = Find(key);
            if (pair == null)
            {
                return false;
            }
            else
            {
                pair.SetDeletedValue(true);
                Count--;
                return true;
            }
        }

        public IEnumerator<Pair<TKey, TValue>> GetEnumerator()
        {
            foreach (var pair in table)
            {
                if (pair != null && !pair.IsDeleted())
                {
                    yield return pair;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
