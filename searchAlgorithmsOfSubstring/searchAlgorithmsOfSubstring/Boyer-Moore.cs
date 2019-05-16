using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchAlgorithmsOfSubstring
{
    /// <summary>
    /// Данный класс реализует алгоритм Бойера-Мура поиска подстроки
    /// </summary>
    public class Boyer_Moore : IStringSearcher
    {
        /* Главная особенность алгоритма - это то, что сканирование производится слева направо, а сравнение справа налево
         * Общая оценка вычислительной сложности современного варианта алгоритма Бойера — Мура — O(n+m),
         * если не используется таблица стоп-символов, и  O(n+m+s), 
         * если используется таблица стоп-символов, где n — длина строки, 
         * в которой выполняется поиск, m — длина шаблона поиска,
         * s — алфавит, на котором проводится сравнение
         */
        private string _text; // исходный текст
        private string _pattern; // искомая подстрока
        private Dictionary<char, int> _stopSymbols; // стоп-символы. Для эвристики хорошего символа
        private int[] _goodSuffix; // хорошие суффиксы. Для эвристики хорошего суффикса

        public Boyer_Moore() { }

        public IEnumerable<int> Find(string text, string pattern)
        {
            IDictionary<char,int> lambda = ComputeLastOccurrenceFunction(pattern); // вычисляем таблицу стоп-символов
            int[] gamma = ComputeGoodSuffixFunction(pattern); // вычисляем хорошие суффиксы
            int stop = text.Length - pattern.Length + 1;
            List<int> res = new List<int>();

            int s = 0;

            while (s < stop)
            {
                int j = pattern.Length - 1;

                while ((j > -1) && (pattern[j] == text[s + j])) // сравниваем справа налево, пока совпадают
                    j--;

                // если полностью совпали
                if (j == -1)
                {
                    res.Add(s);
                    s += gamma[0];
                }
                // если совпал какой-то символ
                else
                {
                    // выбираем какая эвристика лучше на столько и сдвигаем
                    int lambdaShift = lambda.ContainsKey(text[s + j]) ? lambda[text[s + j]] : -1;
                    s += Math.Max(gamma[j + 1], j - lambdaShift);
                }
            }
            return res;
        }

        /// <summary>
        /// Данный метод считает эвристику стоп-символа
        /// </summary>
        /// <param name="needle"> Подстрока </param>
        /// <returns> Таблицу стоп-символов </returns>
        /// Данная эвристика для своей работы требует O(s) дополнительной памяти и
        /// O(s) дополнительного времени на этапе подготовки шаблона, где s - мощность алфавита.
        private IDictionary<char,int> ComputeLastOccurrenceFunction(string needle)
        {
            /*В таблице стоп-символов указывается последняя позиция в шаблоне t (исключая последнюю букву) каждого из символов алфавита. 
             Для всех символов, не вошедших в t пишем 0 */
            Dictionary<char,int> rightmostSymbolsPosition = new Dictionary<char, int>();

            for (int i = 0; i < needle.Length - 1; i++)
            {
                char symbol = needle[i];

                if (rightmostSymbolsPosition.ContainsKey(symbol))
                    rightmostSymbolsPosition[symbol] = i; // если символ повторился, то переписываем его позицию на более "правую"
                else
                    rightmostSymbolsPosition.Add(symbol, i);
            }

            return rightmostSymbolsPosition;
        }

        /// <summary>
        /// Данный метод вычисляет эвристику хорошего суффикса
        /// </summary>
        /// <param name="str"> Подстрока </param>
        /// <returns> Множество хороших суффиксов </returns>
        /// Данная эвристика для своей работы требует O(m) времени
        private int[] ComputeGoodSuffixFunction(string str)
        {
            /*Неформально, если при чтении шаблона справа налево совпал суффикс S, а символ b, 
             * стоящий перед S в шаблоне (т. е. шаблон имеет вид PbS), не совпал, 
             * то эвристика совпавшего суффикса сдвигает шаблон на наименьшее число позиций вправо так, чтобы строка S совпала с шаблоном,
             * а символ, предшествующий в шаблоне данному совпадению S, отличался бы от b (если такой символ вообще есть).*/

            int[] p = ComputePrefix(str);

            int[] pRev = ComputePrefix(ReverseString(str));

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
            var values = new int[str.Length];

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
