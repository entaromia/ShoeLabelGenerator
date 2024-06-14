using ShoeLabelGen.Common;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ImageSharpLabelGen.Helpers
{
    public static class ShoeCountDivider
    {
        /// <summary>
        /// Used for shoe count dividing
        /// <br/> Divides each element of a integer list into two lists
        /// <br/> Does not accept lists that have a sum smaller than 14 or greater than 24
        /// </summary>
        /// <param name="list">The list to divide</param>
        public static IEnumerable<List<int>> DivideShoeList(IEnumerable<int> list)
        {
            int sum = list.Sum();

            // no need to divide lists that are smaller than 14
            ArgumentOutOfRangeException.ThrowIfLessThan(sum, 14);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(sum, 24);

            // The divided lists
            List<int> firstList = [];
            List<int> secondList = [];

            // Bool that indicates which list should have the bigger number on a odd number split
            // If we are splitting number '3' into 2 lists, one list would have 2 while the other will have 1
            // If the list1 had the bigger number last time, list2 should have the bigger number next time
            // Set as true so the first big number owner will be list1
            bool doBigNumberOnNext = true;

            foreach (int num in list)
            {
                Debug.WriteLine($"List {(doBigNumberOnNext ? "one" : "two")} will have big number");

                // This is enough for even numbers
                int num1 = num / 2;

                // If even number or zero, add the number without checking the previous big number owner
                if (num % 2 == 0)
                {
                    Debug.WriteLine("Adding without bigNumber check");
                    firstList.Add(num1);
                    secondList.Add(num1);
                }
                else
                {
                    // Properly split odd numbers into integers
                    // The num2 will be always bigger on odd numbers
                    int num2 = num - num1;

                    firstList.Add(doBigNumberOnNext ? num2 : num1);
                    secondList.Add(doBigNumberOnNext ? num1 : num2);

                    // flipping the bigger number owner between list1 & list2 every time
                    doBigNumberOnNext = !doBigNumberOnNext;
                }
            }

            // The lists are alvays evenly divided before fixup, this is not applicable for 18 & 22
            if (sum == 18 || sum == 22)
            {
                // Divide into 10/8 for 18, 12/10 for 22
                int goalSum = sum == 18 ? 10 : 12;
                FixupEdgeCases(ref firstList, ref secondList, goalSum);
            }

            return [firstList, secondList];
        }

        public static IEnumerable<ShoeListItem> DivideShoeList(ShoeListItem item)
        {
            var list = new List<ShoeListItem>();
            var dividedLists = DivideShoeList(item.ShoeCounts);
            foreach (IEnumerable<int> shoeList in dividedLists)
            {
                list.Add(new ShoeListItem(item) { ShoeCounts = new ObservableCollection<int>(shoeList), Total = shoeList.Sum() });
            }
            return list;
        }

        /*
             * Fixup logic for 18 and 22,
             * as they should be divided into 10,8 and 12,10 respectively, not equally
             * 
             * TODO: If total count of the list exceeds 24, divide the list into three lists
             * The biggest shoe parcel box we have now is 12, so dividing lists larger than 24 should output three lists
             */
        private static void FixupEdgeCases(ref List<int> list1, ref List<int> list2, int goalSum)
        {
            // The sum of the lists are always same before fixup, no need the check other one
            var list1Sum = list1.Sum();

            // Try to fix using easier way,
            // Only works if there was any 1, 2 or 3 in the original undivided list
            Debug.WriteLine("Fixing up sum 18 / 22");

            // Take the first '1' from one of lists and add it to the other one
            if (list1.Contains(1))
            {
                int index = list1.IndexOf(1);
                list1[index] = 0;
                list2[index]++;
            }
            else if (list2.Contains(1))
            {
                int index = list2.IndexOf(1);
                list2[index] = 0;
                list1[index]++;
            }
            // None of the lists have '1', will need to do more complex fix
            else
            {
                Debug.WriteLine("Division Fixup: Basic method did not work, do more advanced fixup");

                // Loop through number lists until found a number that is non zero, ie. skip empty
                for (int index = 0; index <= 7; index++)
                {
                    Debug.WriteLine("Current index: " + index);

                    int tmpNumber = list1[index] + list2[index];
                    Debug.WriteLine("tmpNumber: " + tmpNumber);

                    // Skip zero values, useless for division fixup
                    if (tmpNumber != 0)
                    {
                        // Add the bigger half to the first list to make it reach the goal sum
                        Console.WriteLine(list1Sum + tmpNumber > goalSum);
                        int num1 = tmpNumber / 2;
                        int num2 = tmpNumber - num1;
                        list2[index] = num1;
                        list1[index] = num2;

                        if (list1.Sum() == goalSum || list2.Sum() == goalSum)
                        {
                            break;
                        }
                    }
                }
            }

            // Throw if none of the methods fixed the edge cases
            if (list1.Sum() != goalSum && list2.Sum() != goalSum)
            {
                throw new ArithmeticException($"Unable to fixup division:\n list1 sum is: {list1.Sum()}\nlist2 sum is: {list2.Sum()}");
            }
            else
            {
                Debug.WriteLine("Sum of the first list = " + list1.Sum());
                Debug.WriteLine("Sum of the second list = " + list2.Sum());
            }
        }
    }
}
