using static Poker.Common.HandValues;

namespace Poker.Common.Hands;

public static class ThreeOfAKind
{
    public static bool Assert(IEnumerable<int> cards)
    {
        int maximumOfAKind = CalculateMaximumOfAKind([.. cards]);

        return maximumOfAKind >= 3;
    }

    public static double CalculateChance(IEnumerable<(int rank, int suit)> hand)
    {
        var cards = CreateCards(hand);
        int maximumOfAKind = CalculateMaximumOfAKind([.. cards]);
        int totalCards = cards.Length;

        double chance = CalculateChance(maximumOfAKind, totalCards);

        return chance;
    }

    public static long Score(IEnumerable<int> cards)
    {
        int score = ThreeOfAKindScore([.. cards]);

        return score > 0
            ? ThreeOfAKindValue + score
            : 0;
    }

    private static double CalculateChance(int maximumOfAKind, int totalCards)
    {
        if (maximumOfAKind >= 3)
        {
            return 1;
        }

        if (maximumOfAKind == 2 && (totalCards == 5 || totalCards == 6))
        {
            return 0.04;
        }

        if (maximumOfAKind == 1 && totalCards == 5)
        {
            return 0.003;
        }

        return 0;
    }

    private static int CalculateMaximumOfAKind(List<int> cards)
    {
        int[] cardCounts = new int[14];

        int maximumOfAKind = 0;
        foreach (var card in cards)
        {
            cardCounts[card]++;

            maximumOfAKind = Math.Max(maximumOfAKind, cardCounts[card]);
        }

        return maximumOfAKind;
    }

    private static int ThreeOfAKindScore(int[] cards)
    {
        int[] cardCounts = new int[14];
        int threeOfAKind = 0;

        foreach (var card in cards)
        {
            cardCounts[card]++;

            if (cardCounts[card] != 3)
            {
                continue;
            }

            if (threeOfAKind < card)
            {
                threeOfAKind = card == (int)Rank.Ace ? 14 : card;
            }
        }

        int highestKicker = 0;
        int lowestKicker = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cardCounts[cards[i]] > 2)
            {
                continue;
            }

            int kicker = cards[i] == (int)Rank.Ace ? 14 : cards[i];

            if (kicker > highestKicker)
            {
                lowestKicker = highestKicker;
                highestKicker = kicker;
            }
            else if (kicker > lowestKicker)
            {
                lowestKicker = kicker;
            }
        }

        return threeOfAKind != 0
            ? threeOfAKind * 1000000 + highestKicker * 10000 + lowestKicker
            : 0;
    }

    private static int[] CreateCards(IEnumerable<(int rank, int suit)> hand)
    {
        List<int> cards = new(7);
        foreach (var card in hand.Select(x => x.rank))
        {
            if (card != (int)Rank.None)
            {
                cards.Add(card);
            }
        }

        return [.. cards];
    }
}