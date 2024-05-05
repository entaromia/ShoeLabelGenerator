using System.Diagnostics;

namespace ImageSharpLabelGen
{
    public static class ShoeCountDivider
    {
        /// <summary>
        /// Divides each element of a integer list into two lists,
        /// <br/>Used for shoe count dividing,
        /// does not accept lists that have a sum smaller than 14 or greater than 24
        /// </summary>
        /// <param name="list">The list to divide</param>
        public static List<List<int>> DivideShoeList(IEnumerable<int> list)
        {
            int sum = list.Sum();

            // no need to divide lists that are smaller than 14
            ArgumentOutOfRangeException.ThrowIfLessThan(sum, 14);

            /*
             * TODO: Custom logic for 18 and 22,
             * as they should be divided into 10,8 and 12,10 respectively
             * 
             * TODO: If total count of the list exceeds 24, divide the list into three lists
             * The biggest shoe parcel box we have now is 12, so dividing lists larger than 24 should output three lists
             */
            if (sum == 18 || sum == 22 || sum > 24)
            {
                throw new ArgumentOutOfRangeException(nameof(list), "Dividing 18, 22 or >24 is not implemented yet");
            }

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
                int num1 = 0, num2 = 0;

                // This is enough for even numbers
                num1 = num / 2;

                // An even number or zero, add the number without checking the previous big number owner
                if (num % 2 == 0)
                {
                    Debug.WriteLine("Adding without bigNumber check");
                    firstList.Add(num1);
                    secondList.Add(num1);
                }

                // Add the even number division to lists, flipping the bigger number owner between list1 & list2 every time
                else
                {
                    // Properly split odd numbers into integers
                    // The num2 will be always bigger on odd numbers
                    num2 = num - num1;

                    firstList.Add(doBigNumberOnNext ? num2 : num1);
                    secondList.Add(doBigNumberOnNext ? num1 : num2);
                    doBigNumberOnNext = !doBigNumberOnNext;
                }
            }

            Debug.WriteLine("Sum of the first list = " + firstList.Sum());
            Debug.WriteLine("Sum of the second list = " + secondList.Sum());

            return [firstList, secondList];
        }
    }
}
