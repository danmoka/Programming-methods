using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchAlgorithmsOfSubstring
{
    public class RabinKarp:IStringSearcher
    {
        private int _textLength;
        private int _subStringLength;

        private string _text;
        private string _subString;

        private int _param = 31; // Для построения хэша

        private long[] _paramsDeg;
        private long[] _hashPrefixText;
        private long _hashSubString;

        public RabinKarp() { }

        private void SetParameters(string text, string subString)
        {
            _text = text;
            _subString = subString;
            _textLength = text.Length;
            _subStringLength = subString.Length;
            _paramsDeg = new long[_textLength];
            _paramsDeg[0] = 1;

            for (int i = 1; i < _textLength; i++)
                _paramsDeg[i] = (_paramsDeg[i - 1] * _param);

            _hashPrefixText = new long[_textLength];
            _hashPrefixText[0] = (text[0] - 'a' + 1) * _paramsDeg[0];

            for (int i = 1; i < _textLength; i++)
            {
                _hashPrefixText[i] = (text[i] - 'a' + 1) * _paramsDeg[i];
                _hashPrefixText[i] += _hashPrefixText[i - 1];
            }

            _hashSubString = 0;

            for (int i = 0; i < _subStringLength; i++)
                _hashSubString += (subString[i] - 'a' + 1) * _paramsDeg[i];
        }

        public IEnumerable<int> Find(string text, string subString)
        {
            SetParameters(text, subString);

            long cur_h = _hashPrefixText[0];

            for (int i = 1; i < _textLength; i++)
            {
                for (; i + _subStringLength - 1 < _textLength; i++)
                {
                    cur_h = _hashPrefixText[i + _subStringLength - 1];
                    cur_h -= _hashPrefixText[i - 1];

                    // приводим хэши к одной степени и сравниваем
                    if (cur_h == _hashSubString * _paramsDeg[i])
                        yield return i;
                }
            }
        }
    }
}
