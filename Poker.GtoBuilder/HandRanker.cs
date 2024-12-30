using Poker.GameReader.Hands;
using Poker.GtoBuilder.CardDisplay;
using System.Numerics;

namespace Poker.GtoBuilder;

public class HandRanker
{
    private const int MaxCards = 7;
    private const int Maximum = 1000000;
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

    public Dictionary<string, double> CalculateProbabilities((Rank, Suit)[] knownCards)
    {
        int length = Math.Min(knownCards.Length, MaxCards);
        var hand = new (Rank, Suit)[length];

        ResetProbabilities();

        for (int i = 0; i < length; i++)
        {
            _deckOfCards.TryDeal(knownCards[i], out hand[i]);
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
        return Flush.Assert(hand.Select(x => (int)x.suit));
    }

    public bool IsFourOfAKind(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return FourOfAKind.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsFullHouse(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return FullHouse.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsPair(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Pair.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsStraight(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return Straight.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsThreeOfAKind(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return ThreeOfAKind.Assert(hand.Select(x => (int)x.rank));
    }

    public bool IsTwoPair(IEnumerable<(Rank rank, Suit suit)> hand)
    {
        return TwoPair.Assert(hand.Select(x => (int)x.rank));
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
        else if (IsFullHouse(hand))
        {
            _probabilities[HandNames.FullHouse]++;
        }
        else if (IsFlush(hand))
        {
            _probabilities[HandNames.Flush]++;
        }
        else if (IsStraight(hand))
        {
            _probabilities[HandNames.Straight]++;
        }
        else if (IsThreeOfAKind(hand))
        {
            _probabilities[HandNames.ThreeOfAKind]++;
        }
        else if (IsTwoPair(hand))
        {
            _probabilities[HandNames.TwoPair]++;
        }
        else if (IsPair(hand))
        {
            _probabilities[HandNames.Pair]++;
        }
        else
        {
            _probabilities[HandNames.HighCard]++;
        }
    }
}