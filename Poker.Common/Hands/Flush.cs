using static Poker.Common.HandValues;

namespace Poker.Common.Hands;

public static class Flush
{
    public static bool Assert(IEnumerable<int> suits)
    {
        var cardSuits = new int[5];
        int max = 0;
        foreach (int suit in suits)
        {
            if (suit != 0)
            {
                cardSuits[suit]++;
                max = Math.Max(max, cardSuits[suit]);
            }
        }

        return max >= 5;
    }

    public static double CalculateChance(IEnumerable<(int rank, int suit)> hand)
    {
        var cards = CreateCards(hand);
        int maximumSuited = cards.Max();
        int totalCards = cards.Sum();

        double chance = CalculateChance(maximumSuited, totalCards);

        return chance;
    }

    public static long Score(IEnumerable<(int rank, int suit)> hand)
    {
        long score = FlushScore([.. hand]);

        return score > 0
            ? FlushValue + score
            : 0;
    }

    private static double CalculateChance(int maximumSuited, int totalCards)
    {
        if (maximumSuited == 5)
        {
            return 1;
        }

        if (totalCards - maximumSuited > 2)
        {
            return 0;
        }

        if (maximumSuited == 3 && totalCards == 5)
        {
            return 0.04;
        }

        if (maximumSuited == 4 && totalCards == 5)
        {
            return 0.35;
        }

        if (maximumSuited == 4 && totalCards == 6)
        {
            return 0.19;
        }

        return 0;
    }

    private static int[] CreateCards(IEnumerable<(int rank, int suit)> hand)
    {
        var suits = new int[5];
        foreach (var card in hand.Select(x => x.suit))
        {
            suits[card]++;
        }

        //dont count none suit
        suits[0] = 0;

        return suits;
    }

    private static long FlushScore(IEnumerable<(int rank, int suit)> hand)
    {
        var cardSuits = new int[5];
        int max = 0;
        int suitOfMax = 0;
        foreach (var (rank, suit) in hand)
        {
            if (suit == 0)
            {
                continue;
            }

            cardSuits[suit]++;

            if (cardSuits[suit] > max)
            {
                max = cardSuits[suit];
                suitOfMax = suit;
            }

            max = Math.Max(max, cardSuits[suit]);
        }

        if (max >= 5)
        {
            var ranks = new List<long>(5);
            foreach (var (rank, suit) in hand)
            {
                if (suit != suitOfMax)
                {
                    continue;
                }

                ranks.Add(rank == (int)Rank.Ace ? 14: rank);
            }

            ranks = [.. ranks.OrderDescending()];
            
            return ranks[0] * 100000000 + ranks[1] * 1000000 + ranks[2] * 10000 + ranks[3] * 100 + ranks[4];
        }

        return 0;
    }
}