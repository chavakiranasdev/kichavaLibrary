namespace KichavaLibrary.Leet.Easy.TwoSum
{
    using System;
    using System.Collections.Generic;

    public enum AlgorithmToUse
    {
        BruteForce,
        HashOnePass,
    }

    public class Solution
    {
        public int[] TwoSum(int[] nums, int target, AlgorithmToUse algorithmToUse = AlgorithmToUse.HashOnePass)
        {
            switch (algorithmToUse)
            {
                case AlgorithmToUse.BruteForce:
                    return BruteForce(nums, target);
                case AlgorithmToUse.HashOnePass:
                    return HashOnePass(nums, target);
                default:
                    return BruteForce(nums, target);
            }
        }

        private int[] HashOnePass(int[] nums, int target)
        {
            if (nums == null) return null;
            int length = nums.Length;
            if (length == 1)
            {
                if (target != nums[0])
                {
                    return null;
                }
                return new int[] { 0 };
            }

            var hash = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
            {
                int diff = target - nums[i];
                if (hash.ContainsKey(diff))
                {
                    return new int[] { hash[diff], i };
                }
                hash[nums[i]] = i;
            }
            return null;
        }

        private int[] BruteForce(int[] nums, int target)
        {
            if (nums == null) return null;
            var length = nums.Length;
            if (length == 0) { return null; }
            if (length == 1)
            {
                if (target != nums[0])
                {
                    return null;
                }
                return new int[] { 0 };
            }
            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (nums[i] + nums[j] == target)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return null;
        }
    }
}