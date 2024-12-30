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

    public bool IsFullHouse(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return FullHouse.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsFourOfAKind(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return FourOfAKind.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsTwoPair(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return TwoPair.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsThreeOfAKind(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return ThreeOfAKind.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsStraight(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Straight.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsFlush(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Flush.Assert(hand.Select(x => (int)x.suit));
    }
}
