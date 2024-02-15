using System.Linq;
using System.Collections.Generic;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var biGrammes = FindAllNGrammes(text, 2);
            var threeGrammes = FindAllNGrammes(text, 3);

            return MergeDict(GetUniqueValues(biGrammes), GetUniqueValues(threeGrammes));
        }
        
        private static Dictionary<string, string> MergeDict(
            Dictionary<string, string> dictBiGrammes,
            Dictionary<string, string> dictThreeGrammes
            )
        {
            return dictBiGrammes.Concat(dictThreeGrammes)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private static void DictUpdate(
            Dictionary<string, Dictionary<string, int>> dictNGrammes,
            string currentWord,
            string nextWord)
        {
            if (dictNGrammes.ContainsKey(currentWord))
            {
                if (dictNGrammes[currentWord].ContainsKey(nextWord))
                    dictNGrammes[currentWord][nextWord]++;
                else
                    dictNGrammes[currentWord].Add(nextWord, 1);
            }
            else
            {
                dictNGrammes.Add(currentWord, new Dictionary<string, int>());
                dictNGrammes[currentWord].Add(nextWord, 1);
            }
        }

        private static Dictionary<string, Dictionary<string, int>> FindAllNGrammes(List<List<string>> text, int n)
        {
            var dictNGrammes = new Dictionary<string, Dictionary<string, int>>();
            string currentWord;
            string nextWord;
            foreach (var sentence in text)
            {
                var limit = n == 2 ? sentence.Count - 1 : sentence.Count - 2;
                for (var i = 0; i < limit; i++)
                {
                    currentWord = n == 2 ? sentence[i] : sentence[i] + " " + sentence[i + 1];
                    nextWord = n == 2 ? sentence[i + 1] : sentence[i + 2];

                    DictUpdate(dictNGrammes, currentWord, nextWord);
                }
            }
            return dictNGrammes;
        }

        private static Dictionary<string, string> GetUniqueValues(Dictionary<string, Dictionary<string, int>> nGrammes)
        {
            var resultDict = new Dictionary<string, string>();
            foreach (var key in nGrammes.Keys)
            {
                var currentDict = nGrammes[key];
                var maxValue = currentDict.Values.Max();
                var currentKey = "";

                foreach (var k in currentDict.Keys.Where(k => currentDict[k] == maxValue))
                {
                    if (currentKey == "")
                        currentKey = k;
                    else
                        currentKey = string.CompareOrdinal(currentKey, k) < 0 ? currentKey : k;
                }
                resultDict[key] = currentKey;
            }
            return resultDict;
        }
    }
}