using Poker.GameReader.Hands;
using Poker.GtoBuilder.CardDisplay;
using System.Numerics;

namespace Poker.GtoBuilder;

public class HandRanker
{
    public bool IsPair(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Pair.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsTwoPair(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return TwoPair.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsStraight(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Straight.Assert(hand.Select(x => (int)x.rank));
    }
}
