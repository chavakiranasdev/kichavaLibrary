using System;

namespace TwoSum
{
    class Program
    {
        static void Main(string[] args)
        {
            var solution = new Solution();
            var nums = new int[] {3, 4, 5, 6, 7};
            var target = 13;
            var returnValue = solution.TwoSum(nums, target);
            Console.WriteLine($"{returnValue[0]} and {returnValue[1]} are answers");
            Console.WriteLine($"Values at those indexes are {nums[returnValue[0]]} and {nums[returnValue[1]]}");
            Console.WriteLine($"Target requested: {target}");
        }
    }
}
