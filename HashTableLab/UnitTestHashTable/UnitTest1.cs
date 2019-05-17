using System;
using HashTableLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestHashTable
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddTest()
        {
            OpenAddressHashTable<int, int> hashTable = new OpenAddressHashTable<int, int>();

            for (int i = 0; i < 10000; i++)
                hashTable.Add(i, i);
            Assert.AreEqual(10000, hashTable.Count);
        }

        [TestMethod]
        public void RemoveTest()
        {
            OpenAddressHashTable<int, int> hashTable = new OpenAddressHashTable<int, int>();

            for (int i = 0; i < 10000; i++)
                hashTable.Add(i, i);

            for (int i = 100; i < 1000; i++)
                hashTable.Remove(i);

            Assert.AreEqual(9100, hashTable.Count);
        }

        [TestMethod]
        public void FindTest()
        {
            OpenAddressHashTable<int, int> hashTable = new OpenAddressHashTable<int, int>();

            for (int i = 0; i < 10000; i++)
                hashTable.Add(i, i);

            for (int i = 100; i < 1000; i++)
                hashTable.Remove(i);

            bool result = false;

            for (int i = 100; i < 1000; i++)
                result = result || hashTable.Contains(i);

            bool resultTrue = true;

            for (int i = 0; i < 100; i++)
                resultTrue = resultTrue & hashTable.Contains(i);
            Assert.IsFalse(result);
            Assert.IsTrue(resultTrue);
        }
    }
}
