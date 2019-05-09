// Definition for singly-linked list.
namespace KichavaLibrary.Leet.Moderate.AddNumLinkedList
{
    using System;

    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int x) { val = x; }
    }
    public class Solution
    {
        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            if (l1 == null && l2 == null) return null;
            if (l1 == null) return l2;
            if (l2 == null) return l1;

            ListNode l1Head = l1;
            ListNode l2head = l2;
            int length1, length2;

            GetLengths(l1, l2, out length1, out length2);

            ListNode sum = new ListNode(0);
            ListNode sumMover = sum;
            int carryover = 0;
            do
            {
                int digitSum = l1.val + l2.val + carryover;
                carryover = digitSum / 10;
                sumMover.next = new ListNode(digitSum % 10);
                sumMover = sumMover.next;
                l1 = l1.next;
                l2 = l2.next;
            } while (l1 != null && l2 != null);
            while (l1 != null)
            {
                int digitSum = l1.val + carryover;
                carryover = digitSum / 10;
                sumMover.next = new ListNode(digitSum % 10);
                sumMover = sumMover.next;
                l1 = l1.next;
            }
            while (l2 != null)
            {
                int digitSum = l2.val + carryover;
                carryover = digitSum / 10;
                sumMover.next = new ListNode(digitSum % 10);
                sumMover = sumMover.next;
                l2 = l2.next;
            }
            if (carryover != 0)
            {
                sumMover.next = new ListNode(carryover);
                sumMover = sumMover.next;
                carryover = 0;
            }

            if (l1 != null || l2 != null)
            {
                throw new Exception("Bug we have left overs in one of lists");
            }
            return sum.next;
        }
        private void GetLengths(ListNode l1, ListNode l2, out int length1, out int length2)
        {
            length1 = 0;
            length2 = 0;
            do
            {
                if (l1 != null)
                {
                    length1++;
                    l1 = l1.next;
                }
                if (l2 != null)
                {
                    length2++;
                    l2 = l2.next;
                }
            } while (l1 != null || l2 != null);
        }
    }
}