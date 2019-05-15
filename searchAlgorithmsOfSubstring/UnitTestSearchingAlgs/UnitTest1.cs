using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using searchAlgorithmsOfSubstring;

namespace UnitTestSearchingAlgs
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FindSomeWordsBetweenAlgsTest()
        {
            string sub = "qwerty"; 
            CollectionAssert.AreEqual(Tester.FindTest(new Boyer_Moore(), sub), Tester.FindTest(new RabinKarp(),sub));
        }

        [TestMethod]
        public void CheckFindedPositionBoyerMoorTest()
        {
            Assert.AreEqual(4, new Boyer_Moore().Find(Tester.s,"fikus").First());
        }

        [TestMethod]
        public void CheckFindedPositionRabinKarpTest()
        {
            Assert.AreEqual(4, new RabinKarp().Find(Tester.s, "fikus").First());
        }

        [TestMethod]
        public void NoWordInTextTest()
        {
            string sub = "Фикус";
            CollectionAssert.AreEqual(Tester.FindTest(new Boyer_Moore(), sub), Tester.FindTest(new RabinKarp(), sub));
        }

    }

    public static class Tester
    {
        public static string s = "asdafikussdvevfvqwertywfqwfvrqwertybtnynsdqwertyqqqwwwweertyqwertyq";
        public static List<int> FindTest(IStringSearcher searcher, string subString)
        {
            IEnumerable<int> values = searcher.Find(s, subString);
            var result = new List<int>();
            result.AddRange(values);

            return result;
        }
    }
}

