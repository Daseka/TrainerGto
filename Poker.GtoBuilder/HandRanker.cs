using Poker.Common;
using Poker.Common.Hands;

namespace Poker.GtoBuilder;

public class HandRanker
{
    private const int MaxCards = 7;
    private const int Maximum = 100000;
    private readonly Deck _deckOfCards;

    private readonly Dictionary<string, double> _probabilities;

    public HandRanker(int? seed = null)
    {
        _deckOfCards = new Deck(seed);
        _probabilities = new Dictionary<string, double>
        {
            { HandNames.HighCard, 0 },
            { HandNames.Pair, 0 },
            { HandNames.TwoPair, 0 },
            { HandNames.ThreeOfAKind, 0 },
            { HandNames.Straight, 0 },
            { HandNames.Flush, 0 },
            { HandNames.FullHouse, 0 },
            { HandNames.FourOfAKind, 0 },
        };
    }

    public Dictionary<string, double> CalculateProbabilities(IEnumerable<(Rank, Suit)> knownCards)
    {
        var cards = knownCards.Where(x => x.Item1 != Rank.None).ToArray();

        int length = Math.Min(cards.Length, MaxCards);
        var hand = new (Rank, Suit)[length];

        ResetProbabilities();

        for (int i = 0; i < length; i++)
        {
            _deckOfCards.TryDeal(cards[i], out hand[i]);
        }

        for (int i = 0; i < Maximum; i++)
        {
            _deckOfCards.Shuffle();
            (Rank, Suit)[] peeked = [.. _deckOfCards.Peek(MaxCards - (length + 1))];
            var possibleResult = hand.Concat(peeked);

            UpdateProbabilities(possibleResult.ToArray());
        }

        foreach (var key in _probabilities.Keys)
        {
            _probabilities[key] = (Math.Round(_probabilities[key] / Maximum * 100, 2));
        }

        return _probabilities;
    }

    public bool IsFlush(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Flush.Assert(hand.Select(x => (int)x.suit).Where(x => x != 0));
    }

    public bool IsFourOfAKind(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return FourOfAKind.Assert(hand.Select(x => (int)x.rank).Where(x => x != 0));
    }

    public bool IsFullHouse(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return FullHouse.Assert(hand.Select(x => (int)x.rank).Where(x => x != 0));
    }

    public bool IsPair(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Pair.Assert(hand.Select(x => (int)x.rank).Where(x => x != 0));
    }

    public bool IsStraight(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Straight.Assert(hand.Select(x => (int)x.rank).Where(x => x != 0));
    }

    public bool IsThreeOfAKind(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return ThreeOfAKind.Assert(hand.Select(x => (int)x.rank).Where(x => x != 0));
    }

    public bool IsTwoPair(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return TwoPair.Assert(hand.Select(x => (int)x.rank).Where(x => x != 0));
    }

    private void ResetProbabilities()
    {
        foreach (var key in _probabilities.Keys)
        {
            _probabilities[key] = 0;
        }
    }

    private void UpdateProbabilities((Rank, Suit)[] hand)
    {
        if (IsFourOfAKind(hand))
        {
            _probabilities[HandNames.FourOfAKind]++;
        }
        if (IsFullHouse(hand))
        {
            _probabilities[HandNames.FullHouse]++;
        }
        if (IsFlush(hand))
        {
            _probabilities[HandNames.Flush]++;
        }
        if (IsStraight(hand))
        {
            _probabilities[HandNames.Straight]++;
        }
        if (IsThreeOfAKind(hand))
        {
            _probabilities[HandNames.ThreeOfAKind]++;
        }
        if (IsTwoPair(hand))
        {
            _probabilities[HandNames.TwoPair]++;
        }
        if (IsPair(hand))
        {
            _probabilities[HandNames.Pair]++;
        }
        //else
        //{
        //    _probabilities[HandNames.HighCard]++;
        //}
    }
}