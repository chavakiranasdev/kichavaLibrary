namespace KichavaLibrary.AnagramProgram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Kichava.Library.Common;

    public class Anagram
    {
        public List<string> GetAnagrams(string input, List<string> givenDictionary, 
          AnagramAlgorithm anagramAlgorithm = AnagramAlgorithm.QuickLinq)
        {
            switch (anagramAlgorithm)
            {
                case AnagramAlgorithm.QuickLinq:
                    return GetAnagrams_QuickLinq(input, givenDictionary);
                default:
                    return GetAnagrams_QuickLinq(input, givenDictionary);
            }
        }

        private List<string> GetAnagrams_QuickLinq(string input, IList<string> givenDictionary)
        {
            var listOfAnagrams = new List<string>();
            if (input.IsNullOrWhiteSpace() || givenDictionary.IsNullOrEmpty())
            {
                return listOfAnagrams;
            }
            var sortedInput = String.Concat(input.OrderBy(c => c)).ToLower();
            foreach(var wordFromDictionary in givenDictionary)
            {
                var sortedCurrentWord = String.Concat(wordFromDictionary.OrderBy(c =>c)).ToLower();
                if (sortedCurrentWord.Equals(sortedInput))
                {
                    listOfAnagrams.Add(wordFromDictionary);
                }
            }
            return listOfAnagrams;
        }

        public enum AnagramAlgorithm 
        {
            QuickLinq,

        }
    }
}
