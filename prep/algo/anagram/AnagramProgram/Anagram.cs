namespace KichavaLibrary.AnagramProgram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Kichava.Library.Common;

    public class Anagram
    {
        public List<string> GetAnagrams(string input, List<string> givenDictionary, 
          AnagramAlgorithm anagramAlgorithm = AnagramAlgorithm.QuickLinq)
        {
            if (input.IsNullOrWhiteSpace() || givenDictionary.IsNullOrEmpty())
            {
                return new List<string>();
            }
            input = Regex.Replace(input, "\\s", String.Empty);
            switch (anagramAlgorithm)
            {
                case AnagramAlgorithm.QuickLinq:
                    return GetAnagrams_QuickLinq(input, givenDictionary);
                case AnagramAlgorithm.DictionaryMap:
                    return GetAnagrams_DictionaryMap(input, givenDictionary);
                default:
                    return GetAnagrams_QuickLinq(input, givenDictionary);
            }
        }

        private List<string> GetAnagrams_QuickLinq(string input, IList<string> givenDictionary)
        {
            Console.WriteLine("Inside QuickLinq");
            var listOfAnagrams = new List<string>();
            var sortedInput = String.Concat(input.OrderBy(c => c));
            foreach(var wordFromDictionary in givenDictionary)
            {
                var sortedCurrentWord = String.Concat(wordFromDictionary.OrderBy(c =>c));
                sortedCurrentWord = Regex.Replace(sortedCurrentWord, "\\s", String.Empty);
                if (String.Equals(sortedInput, sortedCurrentWord, StringComparison.OrdinalIgnoreCase))
                {
                    listOfAnagrams.Add(wordFromDictionary);
                }
            }
            return listOfAnagrams;
        }
        private List<string> GetAnagrams_DictionaryMap(string input, IList<string> givenDictionary)
        {
            Console.WriteLine("Inside DictionaryMap");
            var listOfAnagrams = new List<string>();
            var inputDictionaryMap = new Dictionary<char, int>();
            var movingDictionaryMap = new Dictionary<char, int>();
            foreach (char c in input)
            {
                if (!inputDictionaryMap.ContainsKey(c))
                {
                    inputDictionaryMap.Add(c, 0);
                }
                inputDictionaryMap[c]++;
            }

            foreach (var currentWord in givenDictionary)
            {
                bool isMatch = true;
                movingDictionaryMap = new Dictionary<char, int>();
                foreach (char c in currentWord)
                {
                    if (Char.IsWhiteSpace(c))
                    {
                        continue;
                    }
                    if (!inputDictionaryMap.ContainsKey(c))
                    {
                        // we have a letter which is missing in original string
                        isMatch = false;
                        break; // go to next string.
                    }
                    if (!movingDictionaryMap.ContainsKey(c))
                    {
                        movingDictionaryMap.Add(c, 0);
                    }
                    movingDictionaryMap[c]++;
                    if (inputDictionaryMap[c] < movingDictionaryMap[c])
                    {
                        // we have an extra letter. 
                        isMatch = false;
                        break; // go to next string
                    }
                }
                if (isMatch)
                {
                    listOfAnagrams.Add(currentWord);
                }
            }

            return listOfAnagrams;
        }

        public enum AnagramAlgorithm 
        {
            QuickLinq,
            DictionaryMap,

        }
    }
}
