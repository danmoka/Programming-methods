using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableLib
{
    interface IHashTable<TKey, TValue>
    {
        void Add(TKey key, TValue value);
        bool Contains(TKey key);
        bool Remove(TKey key);
    }
}
