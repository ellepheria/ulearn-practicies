using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase(@"''", 0, @"", 2)]
        [TestCase(@"'a'", 0, @"a", 3)]
        [TestCase(@"''", 0, @"", 2)]
        [TestCase(@"'text\'text'", 0, "text'text", 12)]
        [TestCase(@"'", 0, @"", 1)]
        [TestCase(@"ttt'\\\\\''", 3, @"\\'", 8)]
        [TestCase(@"'abcde", 0, @"abcde", 6)]
        [TestCase(@"a'abc", 1, @"abc", 4)]
        [TestCase("'abc\\\"abc'", 0, "abc\"abc", 10)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            if (line.Length <= startIndex + 1)
                return new Token("", startIndex, 1);

            var quote = line[startIndex];
            var value = new StringBuilder();
            var length = 1;
            var isEscapable = false;

            for (var i = startIndex + 1; i < line.Length; i++)
            {
                length++;
                if (line[i] == '\\' && !isEscapable)
                {
                    isEscapable = true;
                }
                else if (isEscapable)
                {
                    isEscapable = false;
                    value.Append(line[i]);
                }
                else if (line[i] == quote && !isEscapable)
                    return new Token(value.ToString(), startIndex, length);
                else
                {
                    isEscapable = false;
                    value.Append(line[i]);
                }
            }
            return new Token(value.ToString(), startIndex, length);
        }
    }
}

