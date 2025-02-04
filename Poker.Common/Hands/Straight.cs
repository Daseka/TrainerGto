﻿using static Poker.Common.HandValues;
using System.Numerics;

namespace Poker.Common.Hands;

public static class Straight
{
    private const int MaximumCards = 7;
    private const int MaxIndexForOpenEndedStraight = 10;
    private const int MinIndexForOpenEndedStraight = 2;
    private const int StraightSize = 5;

    public static bool Assert(IEnumerable<int> cards)
    {
        (_, List<int> missingValues) = GetMissingValues(cards);

        return missingValues.Count == 0;
    }

    public static double CalculateChance(IEnumerable<(int rank, int suit)> hand)
    {
        List<int> cards = CreateCards(hand);

        (int startIndexWindow, List<int> missingValues) = GetMissingValues(cards);

        var chance = CalculateChance(missingValues, cards, startIndexWindow);

        return chance;
    }

    public static long Score(IEnumerable<int> cards)
    {
        int score = StraightScore([.. cards]);

        return score > 0
            ? StraightValue + score
            : 0;
    }

    private static int StraightScore(IEnumerable<int> cards)
    {
        (int startIndexWindow, List<int> missingValues) = GetMissingValues(cards);

        return missingValues.Count == 0 ? startIndexWindow + 4 : 0;
    }

    private static double CalculateChance(List<int> missingValues, List<int> cards, int startIndexWindow)
    {
        if (missingValues.Count == 0)
        {
            return 1;
        }

        if (DrawingDead(missingValues, cards) || cards.Count < 5)
        {
            return 0;
        }

        if (IsGutShot(missingValues, startIndexWindow))
        {
            return MaximumCards - cards.Count == 1
                && (startIndexWindow <= MinIndexForOpenEndedStraight
                    || startIndexWindow + StraightSize - 1 >= MaxIndexForOpenEndedStraight)
                    ? 0.09
                    : 0.17;
        }

        if (IsBackDoorGutShot(missingValues, startIndexWindow))
        {
            return 0.02;
        }

        if (IsBackDoorOpenEnded(missingValues, startIndexWindow))
        {
            return 0.04;
        }

        return MaximumCards - cards.Count == 1
                && (startIndexWindow <= MinIndexForOpenEndedStraight
                    || startIndexWindow + StraightSize - 1 >= MaxIndexForOpenEndedStraight)
                    ? 0.17
                    : 0.32;
    }

    private static uint ConvertToBitValue(List<int> numbers)
    {
        // 13,12,0011,0010,009,008,007,06,05,04,03,02,01,00
        // 13,12,2048,1024,512,256,128,64,32,16,08,04,02,00

        uint bitValueOfNumbers = 0;
        foreach (int number in numbers)
        {
            bitValueOfNumbers |= 1u << number;
            if (number == 1)
            {
                bitValueOfNumbers |= 1u << 14;
            }
        }

        return bitValueOfNumbers;
    }

    private static List<int> CreateCards(IEnumerable<(int rank, int suit)> hand)
    {
        List<int> cards = new(7);
        foreach (var card in hand.Select(x => x.rank))
        {
            if (card != (int)Rank.None)
            {
                cards.Add(card);
            }
        }

        return cards;
    }

    private static bool DrawingDead(List<int> missingValues, List<int> cards)
    {
        return MaximumCards - cards.Count < missingValues.Count;
    }

    private static (uint bitWindow, int windowStartIndex) GetMinimumBitsOffInWindow(uint bitValueOfNumbers, int size)
    {
        uint bitWindow = 0;
        uint location = bitValueOfNumbers;
        int min = size;
        int windowStartIndex = 0;

        for (int i = 0; i <= 10; i++)
        {
            //Get only relevant bits for window of given size starting at position i
            uint window = (~0u >> (32 - (size + i))) & (~0u << i);
            uint bitsOffInWindow = ~bitValueOfNumbers & window;

            //Count the number of bits that are off (missing) in the window
            int missing = BitOperations.PopCount(bitsOffInWindow);
            if (missing <= min)
            {
                min = missing;
                bitWindow = bitsOffInWindow;

                if (min == 0)
                {
                    windowStartIndex = i;
                }
            }

            //Move i to the next bit that is on to start the window from there
            uint oldBitValueOfNumbers = location;
            location &= location - 1;
            uint positionOfNextOnBit = oldBitValueOfNumbers ^ location;
            i = BitOperations.Log2(positionOfNextOnBit) - 1;

            if (i == -1)
            {
                break;
            }
        }

        return (bitWindow, windowStartIndex);
    }

    private static (int startIndexWindow, List<int> missingValues) GetMissingValues(IEnumerable<int> cards)
    {
        uint bitValueOfNumbers = ConvertToBitValue(cards.ToList());
        (uint bitWindow, int startIndexWindow) = GetMinimumBitsOffInWindow(bitValueOfNumbers, StraightSize);
        List<int> missingValues = IndexOfMissingValues(bitWindow);

        return (startIndexWindow, missingValues);
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

    private static bool IsBackDoorGutShot(List<int> missingValues, int startIndexWindow)
    {
        return missingValues.Count == 2
            && ((startIndexWindow + StraightSize - 1 == missingValues[1] && missingValues[1] - missingValues[0] != 1)
                || (startIndexWindow + StraightSize - 1 != missingValues[1] && missingValues[1] - missingValues[0] == 1));
    }

    private static bool IsBackDoorOpenEnded(List<int> missingValues, int startIndexWindow)
    {
        return missingValues.Count == 2
            && startIndexWindow + StraightSize - 1 == missingValues[1]
            && missingValues[1] - missingValues[0] == 1;
    }

    private static bool IsGutShot(List<int> missingValues, int startIndexWindow)
    {
        return missingValues.Count == 1
            && startIndexWindow + StraightSize - 1 != missingValues[0]
            && startIndexWindow != missingValues[0];
    }
}