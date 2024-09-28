using System.Numerics;

namespace Tests
{
    public class StraightTest
    {
        [Fact]
        public void Bla()
        {
            // 4,x,x,7,8
            // 7,8,x,10,11
            int[] numbers = [2, 3, 9, 10, 11];
            uint bitValueOfNumbers = ConvertToBitValue(numbers);

            int size = 5;
            (uint minBitsOffInWindow, int min) = GetMinimumBitsOffInWindow(bitValueOfNumbers, size);

            List<int> missingValues = IndexOfMissingValues(minBitsOffInWindow);

            Assert.Equal(13, missingValues[1]);
            Assert.Equal(12, missingValues[0]);
            Assert.Equal(2, min);
        }

        private static uint ConvertToBitValue(int[] numbers)
        {
            // 13,12,0011,0010,009,008,007,06,05,04,03,02,01,00
            // 13,12,2048,1024,512,256,128,64,32,16,08,04,02,00

            uint bitValueOfNumbers = 0;
            foreach (int number in numbers)
            {
                bitValueOfNumbers |= 1u << number;
            }

            return bitValueOfNumbers;
        }

        private static (uint minBitsOffInWindow, int count) GetMinimumBitsOffInWindow(uint bitValueOfNumbers, int size)
        {
            uint minBitsOffInWindow = 0;
            uint location = bitValueOfNumbers;
            int min = size;

            for (int i = 0; i < 8; i++)
            {
                //Get only relevant bits for window of given size starting at position i
                uint window = (~0u >> (32 - (size + i))) & (~0u << i);
                uint bitsOffInWindow = ~bitValueOfNumbers & window;

                //Count the number of bits that are off (missing) in the window
                int missing = BitOperations.PopCount(bitsOffInWindow);
                if (missing <= min)
                {
                    min = missing;
                    minBitsOffInWindow = bitsOffInWindow;
                }

                min = Math.Min(min, missing);

                //Move i to the next bit that is on to start the window from there
                uint oldBitValueOfNumbers = location;
                location &= location - 1;
                uint positionOfNextOnBit = oldBitValueOfNumbers ^ location;
                i = BitOperations.Log2(positionOfNextOnBit) - 1;
            }

            return (minBitsOffInWindow, min);
        }

        private static List<int> IndexOfMissingValues(uint minBitsOffInWindow)
        {
            List<int> missingValues = new(5);
            uint location2 = minBitsOffInWindow;

            while (location2 > 0)
            {
                uint oldLocation = location2;
                location2 &= location2 - 1;
                uint positionOfNextOnBit = oldLocation ^ location2;
                missingValues.Add(BitOperations.Log2(positionOfNextOnBit));
            }

            return missingValues;
        }
    }
}