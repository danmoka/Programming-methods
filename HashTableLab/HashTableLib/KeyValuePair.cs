using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableLib
{
    /// <summary>
    /// Данный класс реализует пару ключ - значение
    /// </summary>
    public class Pair<TKey, TValue> : IEquatable<Pair<TKey, TValue>>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; set; }
        public bool Deleted { get; private set; }

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Deleted = false;
        }

        /// <summary>
        /// Данный метод определяет равенство пар
        /// </summary>
        /// <param name="other"> Пара, с которой надо сравнить </param>
        /// <returns> True, если равны, иначе - false </returns>
        public bool Equals(Pair<TKey, TValue> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TKey>.Default.Equals(Key, other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pair<TKey, TValue>)obj);
        }

        public void SetDeletedValue(bool value)
        {
            Deleted = value;
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Key);
        }

        public static bool operator ==(Pair<TKey, TValue> left, Pair<TKey, TValue> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pair<TKey, TValue> left, Pair<TKey, TValue> right)
        {
            return !Equals(left, right);
        }

        internal bool IsDeleted()
        {
            return Deleted;
        }

        internal bool DeletePair()
        {
            if (Deleted) return false;
            Deleted = true;
            return true;
        }
    }
}
