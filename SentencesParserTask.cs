using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    internal static class SentencesParserTask
    {
        static readonly char[] separators = new[] { '.', ':', ';', '(', ')', '!', '?' };
        
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            var sentences = text.Split(
                separators,
                StringSplitOptions.RemoveEmptyEntries
            );
            foreach (var sentence in sentences)
            {
                var words = ParseWords(sentence);
                if (words.Count != 0)
                    sentencesList.Add(words);
            }
            return sentencesList;
        }

        private static List<string> ParseWords(string sentence)
        {
            sentence = sentence.Trim() + " ";
            var words = new List<string>();
            var word = new StringBuilder();
            foreach (var letter in sentence)
            {
                if (letter == '\'' || char.IsLetter(letter))
                    word.Append(char.ToLower(letter));
                else
                {
                    if (word.Length > 0)
                        words.Add(word.ToString());
                    word.Clear();
                }
            }
            return words;
        }
    }
}