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
    public class RabinKarp:IStringSearcher
    {
        /* Для текста длины n и шаблона длины m его среднее и лучшее время исполнения равно O(n) при правильном выборе хеш-функции, 
         * но в худшем случае он имеет эффективность O(nm).
         * Лучшее время будет в случае, когда допустимы ложные срабатывания при поиске, то есть,
         * когда некоторые из найденных вхождений шаблона на самом деле могут не соответствовать шаблону. 
         * Алгоритм Рабина — Карпа работает за гарантированное время O(n) и при подходящем выборе рандомизированной хеш-функции.
         * Этот алгоритм хорошо работает во многих практических случаях, но совершенно неэффективен, например,
         * на поиске строки из 10 тысяч символов «a», за которыми следует «b», в строке из 10 миллионов символов «a».
         * В этом случае он показывает своё худшее время исполнения Θ(mn).*/

        private int _textLength; // длина исходного текста
        private int _subStringLength; // длина искомого текста

        private string _text;
        private string _subString;

        private int _param = 31; // для построения хэша

        private long[] _paramsDeg; // для степеней параметра 
        private long[] _hashPrefixText; // для хэша текста
        private long _hashSubString; // для хэша подстроки

        public RabinKarp() { }

        /// <summary>
        /// Метод устанавливает параметры необходимые для поиска подстроки
        /// </summary>
        /// <param name="text"> Текст, в котром осуществляется поиск </param>
        /// <param name="subString"> Подстрока, которую нужно найти </param>
        private void SetParameters(string text, string subString)
        {
            _text = text;
            _subString = subString;
            _textLength = text.Length;
            _subStringLength = subString.Length;
            _paramsDeg = new long[_textLength];
            _paramsDeg[0] = 1;

            for (int i = 1; i < _textLength; i++)
                _paramsDeg[i] = (_paramsDeg[i - 1] * _param); // считаем все степени параметра

            _hashPrefixText = new long[_textLength];
            _hashPrefixText[0] = (text[0] - 'a' + 1) * _paramsDeg[0];

            for (int i = 1; i < _textLength; i++)
            {
                // считаем все хэши для текста
                _hashPrefixText[i] = (text[i] - 'a' + 1) * _paramsDeg[i];
                _hashPrefixText[i] += _hashPrefixText[i - 1];
            }

            _hashSubString = 0;

            for (int i = 0; i < _subStringLength; i++)
                _hashSubString += (subString[i] - 'a' + 1) * _paramsDeg[i]; // считаем все хэши для подстроки
        }

        /// <summary>
        /// Данный метод ищет подстроку в строке
        /// </summary>
        /// <param name="text"> Строка </param>
        /// <param name="subString"> Подстрока </param>
        /// <returns> Позиции, в которых найдено совпадение </returns>
        public IEnumerable<int> Find(string text, string subString)
        {
            SetParameters(text, subString);
            List<int> res = new List<int>();
            long cur_h = _hashPrefixText[0];

            for (int i = 1; i < _textLength; i++)
            {
                for (; i + _subStringLength - 1 < _textLength; i++)
                {
                    // считаем хэш от подстроки в тексте
                    cur_h = _hashPrefixText[i + _subStringLength - 1];
                    cur_h -= _hashPrefixText[i - 1];

                    // приводим хэши к одной степени и сравниваем
                    if (cur_h == _hashSubString * _paramsDeg[i])
                        res.Add(i);
                }
            }
            return res;
        }
    }
}
