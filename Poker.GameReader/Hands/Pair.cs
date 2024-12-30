using Poker.GameReader.Reporters;

namespace Poker.GameReader.Hands;

public static class Pair
{
    public static bool Assert(IEnumerable<int> cards)
    {
        return HasPair(cards.ToList());
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
}