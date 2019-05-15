using System;
using BinaryHeapLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestBinHeap
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddItemsTest()
        {
            BinaryHeap<int> binaryHeap = new BinaryHeap<int>();

            foreach (int el in new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })
                binaryHeap.Add(el);

            Assert.AreEqual(10, binaryHeap.Count);
        }

        [TestMethod]
        public void PopItemsTest()
        {
            BinaryHeap<int> binaryHeap = new BinaryHeap<int>();
            int[] result = new int[5];

            foreach (int el in new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })
                binaryHeap.Add(el);

            for (int i = 0; i < 5; i++)
                result[i] = binaryHeap.Pop();

           CollectionAssert.AreEqual(new int[5] { 9, 8, 7, 6, 5 }, result);
        }

        [TestMethod]
        public void MaxItemTest()
        {
            BinaryHeap<int> binaryHeap = new BinaryHeap<int>();
            int[] result = new int[5];

            foreach (int el in new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })
                binaryHeap.Add(el);

            binaryHeap.Pop();
            binaryHeap.Pop();
            binaryHeap.Add(7);
            binaryHeap.Pop();

            Assert.AreEqual(7, binaryHeap.Max());
        }
    }
}
