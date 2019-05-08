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
        [DataRow(new int[] { 4, 5, 6, 7, 8 }, 12, new int[] { 0, 4 })]
        [DataRow(new int[] { 4, 5, 6, 7, 8 }, 9, new int[] { 0, 1 })]
        [DataRow(new int[] { 4, 5, 6, 7, 8 }, null, null)]
        [DataRow(new int[] { }, 9, null)]
        [DataRow(new int[] { 9 }, 9, new int[] { 0 })]
        [DataRow(new int[] { Int32.MinValue, Int32.MaxValue, 1 }, Int32.MaxValue, null)]
        [DataRow(null, null, null)]
        public void TwoSum_Positive_ShouldWork(int[] nums, int target, int[] expected)
        {
            var actual = new Solution().TwoSum(nums, target);
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual), $"Unexpected output. Expected: {JsonConvert.SerializeObject(expected)} but actual is " +
                $"{JsonConvert.SerializeObject(actual)} ");
        }
    }
}
