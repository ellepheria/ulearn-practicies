using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        /// <returns>
        /// Возвращает первую фразу словаря, начинающуюся с prefix.
        /// </returns>
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];

            return null;
        }

        /// <returns>
        /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
        /// элементов словаря, начинающихся с prefix.
        /// </returns>
        /// <remarks>Эта функция работает за O(log(n) + count)</remarks>
        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var countByPrefix = GetCountByPrefix(phrases, prefix);
            var length = Math.Min(count, countByPrefix);

            var result = new string[length];
            var startIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;

            for (var i = startIndex; i < startIndex + length; i++)
            {
                result[i - startIndex] = phrases[i];
            }

            return result;
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            // тут стоит использовать написанные ранее классы LeftBorderTask и RightBorderTask
            var leftIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            var rightIndex = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);

            return rightIndex - leftIndex - 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            IReadOnlyList<string> phrases = new List<string>(new[] { "aaa", "aab", "aac", "aad", "baa", "bad" });
            var prefix = "d";
            var count = 1;
            var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
            CollectionAssert.IsEmpty(actualTopWords);
        }

        [Test]
        public void TopByPrefix_IsEmpty_WhenNoCount()
        {
            IReadOnlyList<string> phrases = new List<string>(new[] { "aaa", "aab", "aac", "aad", "baa", "bad" });
            var prefix = "aa";
            var count = 0;
            var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
            CollectionAssert.IsEmpty(actualTopWords);
        }

        [Test]
        public void TopByPrefix_IsCorrectAnswer_UsualCase()
        {
            IReadOnlyList<string> phrases = new List<string>(new[] { "aaa", "aab", "aac", "aad", "baa", "bad" });
            var count = 3;
            var prefix = "aa";
            var actual = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
            var expected = new[] { "aaa", "aab", "aac" };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            IReadOnlyList<string> phrases = new List<string>(new[] { "first", "second", });
            var prefix = "";
            var actualCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
            var expectedCount = phrases.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void CountByPrefix_IsCorrectAnswer_UsualCase()
        {
            IReadOnlyList<string> phrases = new List<string>(new[] { "aaa", "aab", "aac", "aad", "baa", "bad" });
            var prefix = "a";
            var actualCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
            var expectedCount = 4;
            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}

