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
        [DataRow(Anagram.AnagramAlgorithm.PreProcessDict)]
        [DataRow(Anagram.AnagramAlgorithm.PreProcessDictInputWithinDict)]
        public void Anagram_Positive_ShouldReturnExpected(Anagram.AnagramAlgorithm algorithm)
        {
            var input = "ABCD";
            var givenDictionary = new List<string>() {
                "ACDB",
                "AACDB",
                "DADBCD",
                "HELLO",
                "DCBA" ,
                " ",
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

        [TestMethod]
        [DataRow(Anagram.AnagramAlgorithm.DictionaryMap)]
        [DataRow(Anagram.AnagramAlgorithm.QuickLinq)]
        [DataRow(Anagram.AnagramAlgorithm.PreProcessDict)]
        [DataRow(Anagram.AnagramAlgorithm.PreProcessDictInputWithinDict)]
        public void Anagram_Null_ShouldReturnEmpty(Anagram.AnagramAlgorithm algorithm)
        {
            string input = null;
            var givenDictionary = new List<string>() {
                "ACDB",
                "DADBCD",
                "HELLO",
                "DCBA" 
            };
            var expected = new List<string>();
            var output = new Anagram().GetAnagrams(input, givenDictionary);
            Assert.AreEqual(output.Count, 0);
            Console.WriteLine(String.Join("\n", output));
            Assert.IsFalse(output.Except(expected).Any(), "not expected");
        }

        [TestMethod]
        [DataRow(Anagram.AnagramAlgorithm.DictionaryMap)]
        [DataRow(Anagram.AnagramAlgorithm.QuickLinq)]
        [DataRow(Anagram.AnagramAlgorithm.PreProcessDict)]
        [DataRow(Anagram.AnagramAlgorithm.PreProcessDictInputWithinDict)]
        public void Anagram_DictionaryNull_ShouldReturnEmpty(Anagram.AnagramAlgorithm algorithm)
        {
            var input = "ABCD";
            var expected = new List<string>();
            var output = new Anagram().GetAnagrams(input, null);
            Assert.AreEqual(output.Count, 0);
            Console.WriteLine(String.Join("\n", output));
            Assert.IsFalse(output.Except(expected).Any(), "not expected");
        }

        [TestMethod]
        [DataRow(Anagram.AnagramAlgorithm.DictionaryMap)]
        [DataRow(Anagram.AnagramAlgorithm.QuickLinq)]
        [DataRow(Anagram.AnagramAlgorithm.PreProcessDict)]
        [DataRow(Anagram.AnagramAlgorithm.PreProcessDictInputWithinDict)]
        public void Anagram_InputSpaces_ShouldReturnExpected(Anagram.AnagramAlgorithm algorithm)
        {
            var input = "AB C D";
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
