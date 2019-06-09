namespace Kichava.Library.Trie
{
    using System;
    using System.Collections.Generic;
    using Kichava.Library.Common;

    public class Trie<T>
    {
        private TrieNode<T> trieRoot;

        public Trie()
        {
            trieRoot = new TrieNode<T>();
        }

        public void Insert(T item)
        {
            var itemAsString = GetItemAsString(item);
            if (itemAsString == null)
            {
                return; // nothing to do
            }

            var currentNode = trieRoot;
            foreach (char c in itemAsString)
            {
                if (!currentNode.Children.ContainsKey(c))
                {
                    currentNode.Children.Add(c, new TrieNode<T>());
                    // for partial intermedia non complete nodes, we don't store complete path value
                }
                currentNode = currentNode.Children[c];
            }
            // We are at last node - mark it as complete.
            currentNode.IsComplete = true;
            currentNode.CompletePathValue = item;
        }

        public void Delete(T item)
        {
            var itemAsString = GetItemAsString(item);
            if (itemAsString == null)
            {
                return; // nothing to do
            }

            var currentNode = trieRoot;
            TrieNode<T> bottomestNodeToPreserve = null;
            char childKeyToDelete = 'z'; // without assingning compiler is giving error
            foreach (char c in itemAsString)
            {
                if (!currentNode.Children.ContainsKey(c))
                {
                    Console.WriteLine("Info: Given item is not present in current Trie");
                    return;
                }
                if (currentNode.Children.Count == 1 && bottomestNodeToPreserve == null)
                {
                    bottomestNodeToPreserve = currentNode;
                    childKeyToDelete = c;
                }
                else
                {
                    bottomestNodeToPreserve = null;
                }
                currentNode = currentNode.Children[c];
            }
            if (bottomestNodeToPreserve != null)
            {
                bottomestNodeToPreserve.Children.Remove(childKeyToDelete);
                // Question: Will GC take care of deleting rest of the nodes?
            }
        }

        public bool IsPresent(T item)
        {
            var itemAsString = GetItemAsString(item);
            if (itemAsString == null)
            {
                return false;
            }

            var currentNode = trieRoot;
            foreach (char c in itemAsString)
            {
                if (!currentNode.Children.ContainsKey(c))
                {
                    return false; // Not found.
                }
                currentNode = currentNode.Children[c];
            }

            return currentNode.IsComplete;
        }

        public List<T> DFS()
        {
            var result = new List<T>();
            var stack = new Stack<TrieNode<T>>();
            stack.Push(trieRoot);
            if (stack.Count != 0)
            {
                var currentNode = stack.Pop();
                if (currentNode.IsComplete)
                {
                    result.Add(currentNode.CompletePathValue);
                }
                foreach(var node in currentNode.Children)
                {
                    stack.Push(node.Value);
                }
            }
            return result;
        }

        private string GetItemAsString(T item)
        {
            if (item == null) 
            {
                Console.WriteLine("Info: Input item is null, returning without inserting anything");
                return null;
            }
            var itemAsString = item.ToString();
            if (String.IsNullOrEmpty(itemAsString))
            {
                Console.WriteLine("Info: Input item converted tos tring is either empty or null, returning without inserting anything");
                return null;
            }
            return itemAsString;
        }

    }
}