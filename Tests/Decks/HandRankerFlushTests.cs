using Poker.GtoBuilder;
using Poker.GtoBuilder.CardDisplay;

namespace Tests.Decks;

public class HandRankerFlushTests
{
    [Fact]
    public void WhenIsFlush()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.Jack, Suit.Diamond), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ace, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Two, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ten, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.True(hand.IsFlush(hands));
    }

    [Fact]
    public void WhenIsNotFlush()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.Jack, Suit.Diamond), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Two, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ten, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.False(hand.IsFlush(hands));
    }
}
