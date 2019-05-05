using SkipListLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipListLab
{
    public class SortedListAdapter<TKey, TValue> : IAction<TKey, TValue>
    {
        private SortedList<TKey, TValue> _sortList;

        public SortedListAdapter()
        {
            _sortList = new SortedList<TKey, TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            _sortList.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
           return _sortList.ContainsKey(key);
        }

        public void Remove(TKey key)
        {
            _sortList.Remove(key);
        }
    }
}
