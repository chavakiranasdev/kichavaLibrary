namespace KichavaLibrary.AnagramProgram
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var input = "ABCD";
            var givenDictionary = new List<string>() {
                "ACDB",
                "DADBCD",
                "HELLO",
                "DCBA" 
            };

            Console.WriteLine("First algorithm");
            var anagrams = new Anagram().GetAnagrams(input, givenDictionary);
            Console.WriteLine(String.Join("\n", anagrams));

            Console.WriteLine("Second algorithm");
            anagrams = new Anagram().GetAnagrams(input, givenDictionary, Anagram.AnagramAlgorithm.DictionaryMap);
            Console.WriteLine(String.Join("\n", anagrams));

            Console.WriteLine("The End");
        }
    }
}
