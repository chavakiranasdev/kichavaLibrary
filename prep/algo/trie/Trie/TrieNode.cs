namespace Kichava.Library.Trie
{
    using System;
    using System.Collections.Generic;
    public class TrieNode<T>
    {
        public bool IsComplete { get; set; }

        public Dictionary<char, TrieNode<T>> Children { get; set; }

        public T CompletePathValue { get; set; }
    }
}
