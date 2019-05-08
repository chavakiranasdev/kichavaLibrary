public enum AlgorithmToUse
{
    BruteForce,
}
public class Solution
{
    public int[] TwoSum(int[] nums, int target, AlgorithmToUse algorithmToUse = AlgorithmToUse.BruteForce)
    {
        switch (algorithmToUse)
        {
            case AlgorithmToUse.BruteForce:
                return BruteForce(nums, target);
            default:
                return BruteForce(nums, target);
        }
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