using Microsoft.VisualStudio.TestTools.UnitTesting;
using KichavaLibrary.Leet.Moderate.AddNumLinkedList;

namespace AddNumLinkedList.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow(123, 456, 579)]
        [DataRow(89, 1, 90)]
        [DataRow(342, 465, 807)]
        [DataRow(999, 888, 1887)]
        [DataRow(0, 0, 0)]
        [DataRow(0, 1, 1)]
        public void AddTwoNumbers_Positive_ShouldWork(int num1, int num2, int expected)
        {
            ListNode l1 = ConvertNumberToListNode(num1);
            ListNode l2 = ConvertNumberToListNode(num2);
            var actual = new Solution().AddTwoNumbers(l1, l2);
            var actualNumber = ValidateAndGetNumberFromList(actual);
            Assert.AreEqual(expected, actualNumber, "Incorrect value received");
        }

        private int ValidateAndGetNumberFromList(ListNode node)
        {
            if(node == null) return 0;
            int num = 0;
            int i = 1;
            do
            {
                Assert.AreEqual(node.val / 10, 0, "A node value can not have more than one digit");
                num = num + i * node.val;
                node = node.next;
                i = i * 10;
            }while (node != null);
            return num;
        }

        private ListNode ConvertNumberToListNode(int num)
        {
            if (num == 0) return null;
            int x; 
            x = num % 10;
            ListNode returnNode = new ListNode(x);
            ListNode returnMover = returnNode;
            num = num / 10;
            while (num != 0)
            {
                x = num %10;
                returnMover.next = new ListNode(x);
                returnMover = returnMover.next;
                num = num / 10;
            }
            return returnNode;
        }
    }
}
