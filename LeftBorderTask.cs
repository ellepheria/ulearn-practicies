using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete
{
    public class LeftBorderTask
    {
        /// <returns>
        /// Возвращает индекс левой границы.
        /// То есть индекс максимальной фразы, которая не начинается с prefix и меньшая prefix.
        /// Если такой нет, то возвращает -1
        /// </returns>
        /// <remarks>
        /// Функция рекурсивная
        /// и работает за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
        /// </remarks>
        public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            if (left == right - 1) return (int)left;
            uint temp = (uint)(left + right) / 2;
            var m = (int)temp;
            if (string.Compare(prefix, phrases[m], StringComparison.OrdinalIgnoreCase) < 0 ||
                phrases[m].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return GetLeftBorderIndex(phrases, prefix, left, m);
            return GetLeftBorderIndex(phrases, prefix, m, right);
        }
    }
}
