using static Poker.Common.HandValues;

namespace Poker.Common.Hands;

public static class FourOfAKind
{
    public static bool Assert(IEnumerable<int> cards)
    {
        var maximumOfAKind = CalculateMaximumOfAKind(cards.ToList());

        return maximumOfAKind >= 4;
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
        int score = FourOfAKindScore([.. cards]);

        return score > 0
            ? FourOfAKindValue + score
            : 0;
    }

    private static int FourOfAKindScore(int[] cards)
    {
        int[] cardCounts = new int[14];
        int fourOfAKind = 0;

        foreach (var card in cards)
        {
            cardCounts[card]++;

            if (cardCounts[card] != 4)
            {
                continue;
            }

            if (fourOfAKind < card)
            {
                fourOfAKind = card == (int)Rank.Ace ? 14 : card;
            }
        }

        int highestKicker = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cardCounts[cards[i]] > 3)
            {
                continue;
            }

            int kicker = cards[i] == (int)Rank.Ace ? 14 : cards[i];

            if (kicker > highestKicker)
            {
                highestKicker = kicker;
            }
        }

        return fourOfAKind != 0
            ? fourOfAKind * 1000000 + highestKicker * 10000
            : 0;
    }

    private static double CalculateChance(int maximumOfAKind, int totalCards)
    {
        if (maximumOfAKind < 2)
        {
            return 0;
        }

        if (maximumOfAKind == 4)
        {
            return 1;
        }

        if (maximumOfAKind == 2 && totalCards == 5)
        {
            return 0.001;
        }

        if (maximumOfAKind == 3 && (totalCards == 5 || totalCards == 6))
        {
            return 0.02;
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
}