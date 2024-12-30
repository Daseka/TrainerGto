using Poker.GtoBuilder;
using Poker.GtoBuilder.CardDisplay;

namespace Tests.Decks;

public class HandRankerPairTests
{
    [Fact]
    public void WhenIsTwoPair()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.Jack, Suit.Club), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ten, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.True(hand.IsPair(hands));
    }
    
    [Fact]
    public void WhenIsPair()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.Jack, Suit.Club), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ten, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ace, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.True(hand.IsPair(hands));
    }

    [Fact]
    public void WhenIsNotTwoPair()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.Jack, Suit.Club), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ten, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Five, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Nine, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.False(hand.IsPair(hands));
    }
}
