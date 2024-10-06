using Poker.GameReader.Reporters;

namespace Poker.GameReader.Hands;
public static class Flush
{
    public static double CalculateChance(GameData gameData)
    {
        var cards = CreateCards(gameData);
        int maximumSuited = cards.Max();
        int totalCards = cards.Sum();

        double chance = CalculateChance(maximumSuited, totalCards, gameData);

        return chance;
    }

    private static double CalculateChance(int maximumSuited, int totalCards, GameData gameData)
    {
        if (maximumSuited == 5)
        {
            return 1;
        }

        if (totalCards - maximumSuited > 2 )
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

    private static int[] CreateCards(GameData gameData)
    {
        var suits = new int[5];
        foreach (var card in gameData.HandCards)
        {
            suits[card.cardSuit]++;
        }

        foreach (var card in gameData.CommunityCards)
        {
            suits[card.cardSuit]++;
        }

        //dont count none suit
        suits[0] = 0;

        return suits;
    }
}

