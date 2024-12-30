using Poker.GameReader.Reporters;

namespace Poker.GameReader.Hands;

public static class ThreeOfAKind
{
    public static bool Assert(IEnumerable<int> cards)
    {
        int maximumOfAKind = CalculateMaximumOfAKind(cards.ToList());

        return maximumOfAKind >= 3;
    }

    public static double CalculateChance(GameData gameData)
    {
        var cards = CreateCards(gameData);
        int maximumOfAKind = CalculateMaximumOfAKind(cards);
        int totalCards = cards.Count;

        double chance = CalculateChance(maximumOfAKind, totalCards, gameData);

        return chance;
    }

    private static double CalculateChance(int maximumOfAKind, int totalCards, GameData gameData)
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

    private static List<int> CreateCards(GameData gameData)
    {
        List<int> cards = new(7);
        foreach (var card in gameData.HandCards)
        {
            if (card.cardRank != CardRank.None)
            {
                cards.Add(card.cardRank);
            }
        }

        foreach (var card in gameData.CommunityCards)
        {
            if (card.cardRank != CardRank.None)
            {
                cards.Add(card.cardRank);
            }
        }

        return cards;
    }
}