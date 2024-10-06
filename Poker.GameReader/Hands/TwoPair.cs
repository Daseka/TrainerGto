﻿using Poker.GameReader.Reporters;

namespace Poker.GameReader.Hands;

public static class TwoPair
{
    public static double CalculateChance(GameData gameData)
    {
        var cards = CreateCards(gameData);
        int pairCount = MaximumPairs(cards);
        int totalCards = cards.Count;

        double chance = CalculateChance(pairCount, totalCards, gameData);

        return chance;
    }

    private static int MaximumPairs(List<int> cards)
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

    private static double CalculateChance(int pairCount, int totalCards, GameData gameData)
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
