using Poker.Common;

namespace Poker.GtoBuilder
{
    public interface IHandScorer
    {
        long ScoreHand((Rank rank, Suit suit)[] handCards, (Rank, Suit)[] communityCards);
    }
}