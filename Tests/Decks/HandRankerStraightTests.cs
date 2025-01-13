using Poker.Common;
using Poker.GtoBuilder;

namespace Tests.Decks;

public class HandRankerStraightTests
{
    [Fact]
    public void WhenIsAceHightStraight()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.Jack, Suit.Club), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Ace, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ten, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Nine, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.True(hand.IsStraight(hands));
    }

    [Fact]
    public void WhenIsNotStraight()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.Jack, Suit.Club), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Three, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ten, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Three, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.False(hand.IsStraight(hands));
    }

    [Fact]
    public void WhenIsFiveHighStraight()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.Nine, Suit.Club), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Three, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ace, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Two, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Five, Suit.Club), out card);
        hands.Add(card);
        
        Assert.True(hand.IsStraight(hands));
    }
}