using System.Collections.Generic;

namespace Passwords
{
    public static class CaseAlternatorTask
    {
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new List<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            return result;
        }

        private static void AlternateCharCases(char[] word, int startIndex, List<string> result)
        {
            while (true)
            {
                if (startIndex == word.Length)
                {
                    if (!result.Contains(new string(word))) result.Add(new string(word));
                    return;
                }

                if (char.IsLetter(word[startIndex]))
                {
                    word[startIndex] = char.ToLower(word[startIndex]);
                    AlternateCharCases(word, startIndex + 1, result);
                    word[startIndex] = char.ToUpper(word[startIndex]);
                    startIndex++;
                }
                else
                    startIndex++;
            }
        }
    }
}