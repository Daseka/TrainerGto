using static Poker.Common.HandValues;

namespace Poker.Common.Hands;

public static class TwoPair
{
    public static bool Assert(IEnumerable<int> cards)
    {
        int pairCount = MaximumPairs([.. cards]);

        return pairCount >= 2;
    }

    public static double CalculateChance(IEnumerable<(int rank, int suit)> hand)
    {
        var cards = CreateCards(hand);
        int pairCount = MaximumPairs(cards);
        int totalCards = cards.Length;

        double chance = CalculateChance(pairCount, totalCards);

        return chance;
    }

    public static long Score(IEnumerable<int> cards)
    {
        int score = TwoPairScore([.. cards]);

        return score > 0
            ? TwoPairValue + score
            : 0;
    }

    private static double CalculateChance(int pairCount, int totalCards)
    {
        if (pairCount >= 2)
        {
            return 1;
        }

        if (pairCount == 1 && (totalCards == 5 || totalCards == 6))
        {
            return 0.06;
        }

        if (pairCount == 0 && totalCards == 5)
        {
            return 0.008;
        }

        return 0;
    }

    private static int[] CreateCards(IEnumerable<(int rank, int suit)> hand)
    {
        var cardList = new List<int>();
        foreach (var card in hand.Select(x => x.rank))
        {
            if (card != (int)Rank.None)
            {
                cardList.Add((int)card);
            }
        }

        return [.. cardList];
    }

    private static int MaximumPairs(int[] cards)
    {
        int[] cardCounts = new int[14];

        int PairCount = 0;
        foreach (var card in cards)
        {
            cardCounts[card]++;

            if (cardCounts[card] == 2)
            {
                PairCount++;
            }
        }

        return PairCount;
    }

    private static int TwoPairScore(int[] cards)
    {
        int[] cardCounts = new int[14];
        int firstPair = 0;
        int secondPair = 0;

        foreach (var card in cards)
        {
            cardCounts[card]++;

            if (cardCounts[card] != 2)
            {
                continue;
            }

            if (firstPair < card)
            {
                secondPair = firstPair;
                firstPair = card == (int)Rank.Ace ? 14 : card;
            }
            else if (secondPair < card)
            {
                secondPair = card == (int)Rank.Ace ? 14 : card;
            }
        }

        int highestKicker = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cardCounts[cards[i]] > 1)
            {
                continue;
            }

            int kicker = cards[i] == (int)Rank.Ace ? 14 : cards[i];

            highestKicker = Math.Max(highestKicker, kicker);
        }

        return secondPair != 0
            ? firstPair * 1000000 + secondPair * 10000 + highestKicker
            : 0;
    }
}