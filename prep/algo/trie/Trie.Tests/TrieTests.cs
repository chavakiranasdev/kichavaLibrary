namespace Kichava.Library.Unittests.Trie.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Kichava.Library.TrieLib;

    [TestClass]
    public class TrieTest
    {
        [TestMethod]
        public void Insert_Single_Positive()
        {
            var tri = new Trie<string>();
            tri.Insert("One");
            var result = tri.DFS();
            Console.WriteLine(String.Join("\n", result));
            Assert.IsTrue(result.Contains("One"), "Expected inserted string is not found");
        }

        [TestMethod]
        public void Insert_Multiple_Positive()
        {
            var tri = new Trie<string>();
            tri.Insert("One");
            tri.Insert("Two");
            tri.Insert("Three");
            var result = tri.DFS();
            Console.WriteLine(String.Join("\n", result));
            Assert.IsTrue(result.Contains("One"), "Expected inserted string One is not found");
            Assert.IsTrue(result.Contains("Two"), "Expected inserted string Two is not found");
            Assert.IsTrue(result.Contains("Three"), "Expected inserted string Three is not found");
        }

        [TestMethod]
        public void Delete_Single_Positive()
        {
            var tri = new Trie<string>();
            tri.Insert("One");
            tri.Delete("One");
            var result = tri.DFS();
            Console.WriteLine(String.Join("\n", result));
            Assert.IsFalse(result.Contains("One"), "Expected deleted string is found");
        }

        [TestMethod]
        public void IsPresent_Single_Positive()
        {
            var tri = new Trie<string>();
            tri.Insert("One");
            Assert.IsTrue(tri.IsPresent("One"), "Expected inserted string is not found via IsPresent method");
        }
    }
}
