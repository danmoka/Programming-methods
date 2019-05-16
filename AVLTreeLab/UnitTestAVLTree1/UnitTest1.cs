using System;
using AVLTreeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestAVLTree1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddTest()
        {
            // ненастоящий тест
            AVLTree<int, int> tree = new AVLTree<int, int>();

            for( int i = 0; i < 100000; i++)
                tree.Insert(i, 1);

            Assert.AreEqual(100000, tree.Count);
        }

        [TestMethod]
        public void AddTestAndFind()
        {
            AVLTree<int, int> tree = new AVLTree<int, int>();

            for (int i = 0; i < 100000; i++)
                tree.Insert(i, 1);

            bool isFinded = true;

            for (int i = 0; i < 100000; i++)
                isFinded = isFinded & tree.Find(i);

            Assert.IsTrue(isFinded);
        }

        [TestMethod]
        public void RemoveTest()
        {
            // ненастоящий тест
            AVLTree<int, int> tree = new AVLTree<int, int>();
            for (int i = 0; i < 100000; i++)
                tree.Insert(i, 1);

            for (int i = 100; i < 1000; i++)
                tree.Remove(i);

            Assert.AreEqual(99100, tree.Count);
        }

        [TestMethod]
        public void RemoveTestAndFind()
        {
            AVLTree<int, int> tree = new AVLTree<int, int>();
            for (int i = 0; i < 100000; i++)
                tree.Insert(i, 1);

            for (int i = 100; i < 1000; i++)
                tree.Remove(i);

            bool isFinded = false;

            for (int i = 100; i < 1000; i++)
                isFinded = isFinded || tree.Find(i);

            Assert.IsFalse(isFinded);
        }
    }
}
