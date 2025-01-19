using static Poker.Common.HandValues;

namespace Poker.Common.Hands;

public static class FullHouse
{
    public static bool Assert(IEnumerable<int> cards)
    {
        var (pairCount, tripple) = MaximumCounts(cards.ToList());

        return pairCount >= 1 && tripple >= 1;
    }

    public static double CalculateChance(IEnumerable<(int rank, int suit)> hand)
    {
        var cards = CreateCards(hand);
        var (pairCount, tripple) = MaximumCounts(cards);
        int totalCards = cards.Count;

        double chance = CalculateChance(pairCount, tripple, totalCards);

        return chance;
    }

    public static long Score(IEnumerable<int> cards)
    {
        int score = FullHouseScore([.. cards]);

        return score > 0
            ? FullHouseValue + score
            : 0;
    }

    private static int FullHouseScore(int[] cards)
    {
        int[] cardCounts = new int[14];
        int pair = 0;
        int tripple = 0;
        int oldPair = 0;

        foreach (var card in cards)
        {
            cardCounts[card]++;
            
            if (cardCounts[card] == 2 && pair < card)
            {
                oldPair = pair;
                pair = card == (int)Rank.Ace ? 14 : card;
            }

            if (cardCounts[card] == 3 && tripple < card)
            {
                tripple = card == (int)Rank.Ace ? 14 : card;

                if (pair == tripple)
                {
                    pair = oldPair;
                }
            }
        }

        return pair != 0 && tripple != 0
            ? tripple * 1000000 + pair * 10000 
            : 0;
    }

    private static double CalculateChance(int pairCount, int trippleCount, int totalCards)
    {
        if (pairCount == 1 && trippleCount == 1)
        {
            return 1;
        }

        if (pairCount == 1 && totalCards == 5)
        {
            return 0.0009;
        }

        if (pairCount == 2 && (totalCards == 5 || totalCards == 6))
        {
            return 0.04;
        }

        if (trippleCount == 1 && (totalCards == 5 || totalCards == 6))
        {
            return 0.07;
        }

        return 0;
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

    private static (int pair, int tripple) MaximumCounts(List<int> cards)
    {
        int[] cardCounts = new int[14];

        int PairCount = 0;
        int TrippleCount = 0;
        foreach (var card in cards)
        {
            cardCounts[card]++;

            if (cardCounts[card] == 2)
            {
                PairCount++;
            }

            if (cardCounts[card] == 3)
            {
                PairCount--;
                TrippleCount++;
            }
        }

        return (PairCount, TrippleCount);
    }
}