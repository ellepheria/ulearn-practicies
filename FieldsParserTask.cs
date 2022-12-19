using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        private static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (var i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("'hello'", new[] { "hello" })]
        [TestCase("\"world\"", new[] { "world" })]
        [TestCase("'quoted text'", new[] { "quoted text" })]
        [TestCase("\"double-quoted text\"", new[] { "double-quoted text" })]
        [TestCase("", new string[0])]
        [TestCase("'te \" quote \" xt'", new[] { "te \" quote \" xt" })]
        [TestCase("\"te ' quote ' xt\"", new[] { "te ' quote ' xt" })]
        [TestCase("'te\\\'xt'", new[] { "te'xt" })]
        [TestCase("te     xt", new[] { "te", "xt" })]
        [TestCase("\"te\\\"xt\"", new[] { "te\"xt" })]
        [TestCase("te\"haha\"xt", new[] { "te", "haha", "xt" })]
        [TestCase("'text \\\\'", new[] { "text \\" })]
        [TestCase("\" 'field1' 'field2' \"", new[] { " 'field1' 'field2' " })]
        [TestCase(@"""\\""", new[] { "\\" })]
        [TestCase("text", new[] { "text" })]
        [TestCase(@"""\\""", new[] { "\\" })]
        [TestCase(@" '' ", new[] { "" })]
        [TestCase(@"'a\'a\'a'", new[] { "a'a'a" })]
        [TestCase("'x ", new[] { "x " })]
        [TestCase("a \"bcd ef\" 'x y'", new[] { "a", "bcd ef", "x y" })]
        [TestCase("\"a \\\"c\\\"\"", new[] { "a \"c\"" })]
        [TestCase("'hard' simple", new[] { "hard", "simple" })]

        public static void RunTests(string input, string[] expectedOutput)
            => Test(input, expectedOutput);
    }

    public class FieldsParserTask
    {
        private static readonly char[] Quotes = new[] { '"', '\'' };
        private static readonly char[] Separators = new[] { ' ', '"', '\'', '\\' };
        public static List<Token> ParseLine(string line)
        {
            var tokens = new List<Token>();
            var currentStartIndex = 0;
            while (currentStartIndex < line.Length)
            {
                if (line[currentStartIndex] == ' ')
                {
                    currentStartIndex++;
                    continue;
                }
                var currentField = ReadField(line, currentStartIndex);
                currentStartIndex += currentField.Length;
                tokens.Add(currentField);
            }
            return tokens;
        }

        private static Token ReadField(string line, int startIndex)
        {
            return Quotes.Any(quote => line[startIndex] == quote) ?
                ReadQuotedField(line, startIndex) :
                ReadSimpleField(line, startIndex);
        }

        private static Token ReadQuotedField(string line, int startIndex)
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
                    continue;
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

        private static Token ReadSimpleField(string line, int startIndex)
        {
            var value = new StringBuilder();
            var length = 0;

            for (var i = startIndex; i < line.Length; i++)
            {
                if (Separators.Any(separator => line[i] == separator))
                    return new Token(value.ToString(), startIndex, length);
                length++;
                value.Append(line[i]);
            }
            return new Token(value.ToString(), startIndex, length);
        }
    }
}