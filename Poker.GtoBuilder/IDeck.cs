using Poker.Common;

namespace Poker.GtoBuilder
{
    public interface IDeck
    {
        bool CanDeal((Rank, Suit) cardToDeal);
        (Rank, Suit)[] GetRemaining();
        (Rank, Suit)[] Peek(int count);
        void Reset();
        void Shuffle();
        bool TryDeal((Rank, Suit) cardToDeal, out (Rank, Suit) cardDealt);
        bool TryDeal(out (Rank, Suit)? cardDealt);
    }
}

