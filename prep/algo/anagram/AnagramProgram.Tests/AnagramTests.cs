namespace AnagramProgram.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using KichavaLibrary.AnagramProgram;
    using System.Linq;
    using System;

    [TestClass]
    public class AnagramTests
    {
        [TestMethod]
        [DataRow(Anagram.AnagramAlgorithm.DictionaryMap)]
        [DataRow(Anagram.AnagramAlgorithm.QuickLinq)]
        public void Anagram_Positive_ShouldReturnExpected(Anagram.AnagramAlgorithm algorithm)
        {
            var input = "ABCD";
            var givenDictionary = new List<string>() {
                "ACDB",
                "DADBCD",
                "HELLO",
                "DCBA" 
            };
            var expected = new List<string>() {
                "ACDB",
                "DCBA",
                "DCBA" 
            };
            var output = new Anagram().GetAnagrams(input, givenDictionary);
            Assert.AreEqual(output.Count, 2);
            Console.WriteLine(String.Join("\n", output));
            Assert.IsFalse(output.Except(expected).Any(), "not expected");
        }
    }
}
