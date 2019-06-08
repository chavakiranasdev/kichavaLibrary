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

            var anagrams = new Anagram().GetAnagrams(input, givenDictionary);
            Console.WriteLine(String.Join("\n", anagrams));

            Console.WriteLine("Press any key to exit");
        }
    }
}
