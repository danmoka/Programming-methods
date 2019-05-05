using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkipListLib;

namespace UnitTests
{
    [TestClass]
    public class SkipListUnitTest
    {
        [TestMethod]
        public void AddItems()
        {
            SkipList<int, int> skipList = new SkipList<int, int>(20, 0.5);
            for (int i = 0; i < 1000000; i++)
                skipList.Add(i, 1);
            Assert.AreEqual(skipList.Count, 1000000);
        }

        [TestMethod]
        public void RemoveItems()
        {
            SkipList<int, int> skipList = new SkipList<int, int>();
            for (int i = 0; i < 100; i++)
                skipList.Add(i, 1);
            for (int i = 20; i < 50; i++)
                skipList.Remove(i);
            bool result = false;
            for (int i = 20; i < 50; i++)
                result = result || skipList.ContainsKey(i);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsItems()
        {
            SkipList<int, int> skipList = new SkipList<int, int>();
            for (int i = 0; i < 100; i++)
                skipList.Add(i, 1);
            Assert.IsFalse(skipList.ContainsKey(200));
        }
    }
}
