using static Poker.Common.HandValues;
using Poker.Common;
using Poker.Common.Hands;

namespace Poker.GtoBuilder;

public class HandScorer
{
    public static long ScoreHand((Rank rank, Suit suit)[] handCards, (Rank, Suit)[] communityCards)
    {
        long score = 0;
        (Rank rank, Suit suit)[] hand = [.. handCards, .. communityCards];

        if (IsFlush(hand) && TryScoreStraight(hand, out long flushStraight))
        {
            score = flushStraight + FlushValue;
        }
        else if (TryScoreFourOfAKind(hand, out long fourHouse))
        {
            score = fourHouse;
        }
        else if (TryScoreFullHouse(hand, out long fullHouse))
        {
            score = fullHouse;
        }
        else if (TryScoreFlush(hand, out long flush))
        {
            score = flush;
        }
        else if (TryScoreStraight(hand, out long straight))
        {
            score = straight;
        }
        else if (TryScoreThreeOfAKind(hand, out long threeOfAKind))
        {
            score = threeOfAKind ;
        }
        else if (TryScoreTwoPair(hand, out long twoPair))
        {
            score = twoPair ;
        }
        else if (TryScorePair(hand, out long pair))
        {
            score = pair ;
        }
        else if (TryScoreHighCard(hand, out long highCard))
        {
            score = highCard;
        }

        return score;
    }

    private static bool IsFlush(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Flush.Assert(hand.Select(x => (int)x.suit).Where(x => x != 0));
    }

    private static bool TryScoreHighCard(IEnumerable<(Rank rank, Suit suit)> hand, out long score)
    {
        score = (long)hand.Select(x => x.rank).Max();

        return score > 0;
    }

    private static bool TryScoreFourOfAKind(IEnumerable<(Rank rank, Suit suit)> hand, out long score)
    {
        score = FourOfAKind.Score(hand.Select(x => (int)x.rank).Where(x => x != 0));

        return score > 0;
    }

    private static bool TryScoreFullHouse(IEnumerable<(Rank rank, Suit suit)> hand, out long score)
    {
        score = FullHouse.Score(hand.Select(x => (int)x.rank).Where(x => x != 0));

        return score > 0;
    }

    private static bool TryScorePair(IEnumerable<(Rank rank, Suit suit)> hand, out long score)
    {
        score = Pair.Score(hand.Select(x => (int)x.rank).Where(x => x != 0));

        return score > 0;
    }

    private static bool TryScoreFlush(IEnumerable<(Rank rank, Suit suit)> hand, out long score)
    {
        score = Flush.Score(hand.Select(x => ((int)x.rank, (int)x.suit)).Where(x => x.Item1 != 0));

        return score > 0;
    }

    private static bool TryScoreStraight(IEnumerable<(Rank rank, Suit suit)> hand, out long score)
    {
        score = Straight.Score(hand.Select(x => (int)x.rank).Where(x => x != 0));

        return score > 0;
    }

    private static bool TryScoreThreeOfAKind(IEnumerable<(Rank rank, Suit suit)> hand, out long score)
    {
        score = ThreeOfAKind.Score(hand.Select(x => (int)x.rank).Where(x => x != 0));

        return score > 0;
    }

    private static bool TryScoreTwoPair(IEnumerable<(Rank rank, Suit suit)> hand, out long score)
    {
        score = TwoPair.Score(hand.Select(x => (int)x.rank).Where(x => x != 0));

        return score > 0;
    }
}
