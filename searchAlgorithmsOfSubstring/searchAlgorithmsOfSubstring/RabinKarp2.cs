using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchAlgorithmsOfSubstring
{
    public class RabinKarp2:IStringSearcher
    {
        private int _abcLength = char.MaxValue + 1;
        private int _mod = int.MaxValue;
        private int _subStringHash = 0;
        private int _textHash = 0;
        private int _h = 1;

        public IEnumerable<int> Find(string haystack, string needle)
        { 
            for (int i = 1; i < needle.Length; i++)
            {
                _h = (_h * _abcLength) % _mod;
            }

            for (int i = 0; i < needle.Length; i++)
            {
                _subStringHash = (_abcLength * _subStringHash + needle[i] - 'a' + 1) % _mod;
                _textHash = (_abcLength * _textHash + haystack[i] - 'a' + 1) % _mod;
            }

            List<int> result = new List<int>();

            for(int i = 0; i <= haystack.Length - needle.Length; i++)
            {
                if (_subStringHash == _textHash)
                    if (needle == haystack.Substring(i, needle.Length))
                        result.Add(i);

                if (i < haystack.Length - needle.Length)
                    _textHash = ((_textHash - _h * (haystack[i] - 'a' + 1)) * _abcLength + haystack[i + needle.Length] - 'a' + 1) % _mod;
            }

            return result;
        }
    }
}
