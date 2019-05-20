using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchAlgorithmsOfSubstring
{
    public class RabinKarp2:IStringSearcher
    {
        private int _textLength = 0;
        private int _subStringLength = 0;
        private long _p = 0;
        private long _t = 0;
        private long _d = 10;

        public IEnumerable<int> Find(string haystack, string needle)
        {
            _textLength = haystack.Length;
            _subStringLength = needle.Length;
            //_h = (long)Math.Pow(_d, _subStringLength - 1);

            for(int i = 0; i < _subStringLength; i++)
            {
                _p = (_d * _p + needle[i] - 'a' + 1);
                _t = (_d * _t + haystack[i] - 'a' + 1);
            }

            for(int i = 0; i < _textLength - _subStringLength; i++)
            {
                if (_p == _t)
                    if (needle == haystack.Substring(i, _subStringLength))
                        yield return i;

                if (i < _textLength - _subStringLength)
                    _t = (_t % 10) * 10 + haystack[i + _subStringLength] - 'a' + 1;
            }
        }
    }
}
