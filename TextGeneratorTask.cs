using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    public static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var currentPhrase = new StringBuilder(phraseBeginning);
            string lastWord;
            string lastTwoWords;
            while (wordsCount > 0)
            {
                var words = currentPhrase.ToString().Split(' ');
                lastTwoWords = words.Length > 1 ? words[words.Length - 2] + " " + words[words.Length - 1] : "";
                lastWord = words[words.Length - 1];
                
                if (nextWords.ContainsKey(lastTwoWords))
                    currentPhrase.Append(" " + nextWords[lastTwoWords]);
                else if (nextWords.ContainsKey(lastWord))
                    currentPhrase.Append(" " + nextWords[lastWord]);
                else
                    return currentPhrase.ToString();
                wordsCount--;
            }
            return currentPhrase.ToString();
        }
    }
}

