namespace TwoSum.Tests
{
    using System;
    using KichavaLibrary.Leet.Easy.TwoSum;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow(new int[] { 4, 5, 6, 1, 8 }, 12, new int[] { 0, 4 }, AlgorithmToUse.BruteForce)]
        [DataRow(new int[] { 4, 5, 6, 1, 8 }, 9, new int[] { 0, 1 }, AlgorithmToUse.BruteForce)]
        [DataRow(new int[] { 4, 5, 6, 1, 8 }, null, null, AlgorithmToUse.BruteForce)]
        [DataRow(new int[] { }, 9, null, AlgorithmToUse.BruteForce)]
        [DataRow(new int[] { 9 }, 9, new int[] {0}, AlgorithmToUse.BruteForce)]
        [DataRow(new int[] {Int32.MinValue, Int32.MaxValue, 1}, Int32.MaxValue, null, AlgorithmToUse.BruteForce)]
        [DataRow(null, null, null, AlgorithmToUse.BruteForce)]

        [DataRow(new int[] { 4, 5, 6, 1, 8 }, 12, new int[] { 0, 4 }, AlgorithmToUse.HashOnePass)]
        [DataRow(new int[] { 4, 5, 6, 1, 8 }, 9, new int[] { 0, 1 }, AlgorithmToUse.HashOnePass)]
        [DataRow(new int[] { 4, 5, 6, 1, 8 }, null, null, AlgorithmToUse.HashOnePass)]
        [DataRow(new int[] { }, 9, null, AlgorithmToUse.HashOnePass)]
        [DataRow(new int[] { 9 }, 9, new int[] {0}, AlgorithmToUse.HashOnePass)]
        [DataRow(new int[] {Int32.MinValue, Int32.MaxValue, 1}, Int32.MaxValue, null, AlgorithmToUse.HashOnePass)]
        [DataRow(null, null, null, AlgorithmToUse.HashOnePass)]
        public void TwoSum_Positive_ShouldWork(int[] nums, int target, int[] expected, AlgorithmToUse algorithmToUse)
        {
            var actual = new Solution().TwoSum(nums, target, algorithmToUse);
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual), $"Unexpected output. Expected: {JsonConvert.SerializeObject(expected)} but actual is " +
                $"{JsonConvert.SerializeObject(actual)} ");
        }
    }
}
