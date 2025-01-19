using static Poker.Common.HandValues;

namespace Poker.Common.Hands;

public static class Pair
{
    public static bool Assert(IEnumerable<int> cards)
    {
        return HasPair(cards.ToList());
    }

    public static double CalculateChance(IEnumerable<(int rank, int suit)> hand)
    {
        var cards = CreateCards(hand);
        int maximumOfAKind = CalculateMaximumOfAKind(cards);
        int totalCards = cards.Count;

        double chance = CalculateChance(maximumOfAKind, totalCards);

        return chance;
    }

    public static long Score(IEnumerable<int> cards)
    {
        int score = PairScore([.. cards]);

        return score > 0
            ? PairValue + score
            : 0;
    }

    private static double CalculateChance(int maximumOfAKind, int totalCards)
    {
        if (maximumOfAKind >= 2)
        {
            return 1;
        }

        if (maximumOfAKind == 1 && (totalCards == 5 || totalCards == 6))
        {
            return 0.13;
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

    private static bool HasPair(List<int> cards)
    {
        int[] cardCounts = new int[14];

        foreach (var card in cards)
        {
            cardCounts[card]++;

            if (cardCounts[card] == 2)
            {
                return true;
            }
        }

        return false;
    }

    private static int PairScore(int[] cards)
    {
        int[] cardCounts = new int[14];
        int pair = 0;

        foreach (var card in cards)
        {
            cardCounts[card]++;

            if (cardCounts[card] != 2)
            {
                continue;
            }

            if (pair < card)
            {
                pair = card == (int)Rank.Ace ? 14 : card;
            }
        }

        int highestKicker = 0;
        int middleKicker = 0;
        int lowestKicker = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cardCounts[cards[i]] > 1)
            {
                continue;
            }

            int kicker = cards[i] == (int)Rank.Ace ? 14 : cards[i];

            if (kicker > highestKicker)
            {
                lowestKicker = middleKicker;
                middleKicker = highestKicker;
                highestKicker = kicker;
            }
            else if (kicker > middleKicker)
            {
                lowestKicker = middleKicker;
                middleKicker = kicker;
            }
            else if (kicker > lowestKicker)
            {
                lowestKicker = kicker;
            }
        }

        return pair != 0
            ? pair * 1000000 + highestKicker * 10000 + middleKicker * 100 + lowestKicker
            : 0;
    }
}