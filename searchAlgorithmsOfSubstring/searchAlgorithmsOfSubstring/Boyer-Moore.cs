using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchAlgorithmsOfSubstring
{
    public class Boyer_Moore : IStringSearcher
    {
        private string _haystack;
        private string _needle;
        private Dictionary<char, int> _stopSymbols;
        private int[] _goodSuffix;

        public Boyer_Moore() { }

        public IEnumerable<int> Find(string haystack, string needle)
        {
            IDictionary<char,int> lambda = ComputeLastOccurrenceFunction(needle);
            int[] gamma = ComputeGoodSuffixFunction(needle);
            int stop = haystack.Length - needle.Length + 1;

            int s = 0;

            while (s < stop)
            {
                int j = needle.Length - 1;

                while ((j > -1) && (needle[j] == haystack[s + j]))
                    j--;

                if (j == -1)
                {
                    yield return s;
                    s += gamma[0];
                }
                else
                {
                    int lambdaShift = lambda.ContainsKey(haystack[s + j]) ? lambda[haystack[s + j]] : -1;
                    s += Math.Max(gamma[j + 1], j - lambdaShift);
                }
            }
        }

        private IDictionary<char,int> ComputeLastOccurrenceFunction(string needle)
        {
            Dictionary<char,int> rightmostSymbolsPosition = new Dictionary<char, int>();

            for (int i = 0; i < needle.Length - 1; i++)
            {
                char symbol = needle[i];

                if (rightmostSymbolsPosition.ContainsKey(symbol))
                    rightmostSymbolsPosition[symbol] = i;
                else
                    rightmostSymbolsPosition.Add(symbol, i);
            }

            return rightmostSymbolsPosition;
        }

        private int[] ComputeGoodSuffixFunction(string str)
        {
            var p = ComputePrefix(str);

            var pRev = ComputePrefix(ReverseString(str));

            int[] valuesForShift = new int[str.Length + 1];

            for (int j = 0; j < valuesForShift.Length; j++)
                valuesForShift[j] = str.Length - p[str.Length - 1];

            for (int l = 1; l < valuesForShift.Length; l++)
            {
                int j = str.Length - pRev[l - 1];

                if (valuesForShift[j] > l - pRev[l - 1])
                    valuesForShift[j] = l - pRev[l - 1];
            }
            return valuesForShift;
        }

        private string ReverseString(string str)
        {
            char[] symbols = str.ToCharArray();
            Array.Reverse(symbols);

            return (new string(symbols));
        }

        private static int[] ComputePrefix(string str)
        {
            var values = new int[str.Length]; //значения

            values[0] = 0; //для префикса из одного символа функция равна нулю

            int k = 0;

            for (int q = 1; q < str.Length; q++)
            {
                while ((k > 0) && (str[k] != str[q]))
                    k = values[k - 1];

                if (str[k] == str[q])
                    k++;

                values[q] = k;
            }
            return values;
        }
    }
}
