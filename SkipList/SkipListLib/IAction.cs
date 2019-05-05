using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipListLib
{
    public interface IAction<TKey, TValue>
    {
        bool ContainsKey(TKey key);
        void Remove(TKey key);
        void Add(TKey key, TValue value);
    }
}
