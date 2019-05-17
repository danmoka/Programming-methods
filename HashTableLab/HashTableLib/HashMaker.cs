using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableLib
{
    /// <summary>
    /// Класс - родитель двух классов хэш - функций
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class HashMaker<T>
    {
        public int ModValue { get; set; }

        public HashMaker()
        {
            ModValue = 73;
        }

        public HashMaker(int modValue)
        {
            ModValue = modValue;
        }

        internal int GetHash(T key)
        {
            //0x7fffffff - маска для того, чтобы сделать число положительным
                    int hash = (key.GetHashCode() & 0x7fffffff) % ModValue;
            return hash;
        }
    }

    //internal class FirstHashMaker<T> : HashMaker<T>
    //{
    //    public FirstHashMaker(int mv) : base(mv)
    //    { }

    //    internal override int GetHash(T key)
    //    {
    //        // 0x7fffffff - маска для того, чтобы сделать число положительным
    //        int hash = (key.GetHashCode() & 0x7fffffff) % ModValue;
    //        return hash;
    //    }
    //}

    //internal class SecondHashMaker<T> : HashMaker<T>
    //{
    //    public SecondHashMaker(int mv) : base(mv)
    //    { }

    //    internal override int GetHash(T key)
    //    {
    //        int hash = 1 + (key.GetHashCode() & 0x7fffffff % (ModValue - 1));
    //        return hash;
    //    }
    //}
}
