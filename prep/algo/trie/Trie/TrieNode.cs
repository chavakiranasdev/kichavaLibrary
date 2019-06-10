namespace Kichava.Library.TrieLib
{
    using System;
    using System.Collections.Generic;
    public class TrieNode<T>
    {
        public TrieNode()
        {
            Children = new Dictionary<char, TrieNode<T>>();
        }

        public bool IsComplete { get; set; }

        public Dictionary<char, TrieNode<T>> Children { get; set; }

        public T CompletePathValue { get; set; }

    }
}
