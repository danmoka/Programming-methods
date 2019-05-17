using System;
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

        private HashMaker<TKey> _hashMaker1, _hashMaker2;
        public int Count { get; private set; } // заполненность
        private const double FillFactor = 0.85;
        private readonly int[] numbers = new int[20] {
            373, 877, 2791, 4447,
            9241, 19927, 35317,
            50023, 80039, 250013,
            499973, 999979, 5000077,
            8999993, 15000017, 29999989,
            55999997, 111999997, 223999879, 1999999927 };
        private int _sizeIndex = 0;

        //public OpenAddressHashTable() : this(9973)
        //{ }

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
            int size = numbers[_sizeIndex];
            _capacity = size;
            table = new Pair<TKey, TValue>[size];
            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);
            Count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            var h = _hashMaker1.GetHash(key);

            if (!TryToPut(h, key, value)) // ячейка занята
            {
                int iterationNumber = 1;
                while (true)
                {
                    var place = (h + iterationNumber * (1 + _hashMaker2.GetHash(key))) % _capacity;

                    if (TryToPut(place, key, value))
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
        private Pair<TKey, TValue> Find(TKey key)
        {
            var h = _hashMaker1.GetHash(key);
            if (table[h] == null)
                return null;

            if (!table[h].IsDeleted() && table[h].Key.Equals(key))
            {
                return table[h];
            }

            int iterationNumber = 1;

            while (true)
            {
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
            get { return Find(key).Value; } // Метод Find возвращает значение

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
            int size = numbers[_sizeIndex];
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
