using Poker.GameReader.Reporters;

namespace Poker.GameReader.Hands;

public static class FullHouse
{
    public static double CalculateChance(GameData gameData)
    {
        var cards = CreateCards(gameData);
        var (pairCount,tripple) = MaximumCounts(cards);
        int totalCards = cards.Count;

        double chance = CalculateChance(pairCount, tripple, totalCards, gameData);

        return chance;
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

    private static double CalculateChance(int pairCount, int trippleCount, int totalCards, GameData gameData)
    {
        if (pairCount == 1 && trippleCount == 1)
        {
            return 1;
        }

        if (pairCount == 1 && totalCards == 5 )
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

    private static List<int> CreateCards(GameData gameData)
    {
        List<int> cards = new(7);
        foreach (var card in gameData.HandCards)
        {
            if (card.cardSymbol != CardSymbol.None)
            {
                cards.Add(card.cardSymbol);
            }
        }

        foreach (var card in gameData.CommunityCards)
        {
            if (card.cardSymbol != CardSymbol.None)
            {
                cards.Add(card.cardSymbol);
            }
        }

        return cards;
    }
}

