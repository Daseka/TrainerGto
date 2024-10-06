using Poker.GameReader.Reporters;

namespace Poker.GameReader.Hands;

public static class TwoOfAKind
{
    public static double CalculateChance(GameData gameData)
    {
        var cards = CreateCards(gameData);
        int maximumOfAKind = CalculateMaximumOfAKind(cards);
        int totalCards = cards.Count;

        double chance = CalculateChance(maximumOfAKind, totalCards, gameData);

        return chance;

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

    private static double CalculateChance(int maximumOfAKind, int totalCards, GameData gameData)
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
