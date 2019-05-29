using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchAlgorithmsOfSubstring
{
    /// <summary>
    /// Данный класс реализует алгоритм Рабина-Карпа поиска подстроки
    /// </summary>
    public class RabinKarp2:IStringSearcher
    {
        private int _abcLength = char.MaxValue + 1; // мощность алфавита
        private int _mod = int.MaxValue; // модуль, по которому будем считать хэш
        private int _subStringHash = 0; // хэш подстроки
        private int _textHash = 0; // хэш текста
        private int _h = 1; // для того, чтобы отрбрасывать первую цифру

        // O(n + m), в худшем случае O((n - m + 1) * m)
        public IEnumerable<int> Find(string haystack, string needle)
        { 
            // O(needleLength)
            for (int i = 1; i < needle.Length; i++)
            {
                _h = (_h * _abcLength) % _mod;
            }

            // O(needleLength)
            for (int i = 0; i < needle.Length; i++)
            {
                _subStringHash = (_abcLength * _subStringHash + needle[i] - 'a' + 1) % _mod; // считаем хэш от подстроки
                _textHash = (_abcLength * _textHash + haystack[i] - 'a' + 1) % _mod; // той же размерности считаем хэш текста
            }

            List<int> result = new List<int>();

            // O(haystackLength - needleLength)
            for(int i = 0; i <= haystack.Length - needle.Length; i++)
            {
                // если хэши равны, то...
                if (_subStringHash == _textHash)
                    // и если искомая строка совпадает с найденной в тексте, то...
                    if (needle == haystack.Substring(i, needle.Length))
                        // добавляем позицию, где нашли подстроку
                        result.Add(i);

                // если еще можем двигаться по тексту, то...
                if (i < haystack.Length - needle.Length)
                    // пересчитываем хэш текста: (123 - 1 * 100) * 10 + 4 = 234
                    _textHash = ((_textHash - _h * (haystack[i] - 'a' + 1)) * _abcLength + haystack[i + needle.Length] - 'a' + 1) % _mod;
            }

            return result;
        }
    }
}
