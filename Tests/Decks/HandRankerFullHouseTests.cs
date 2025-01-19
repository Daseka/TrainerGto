using Poker.Common;
using Poker.GtoBuilder;

namespace Tests.Decks;

public class HandRankerFullHouseTests
{
    [Fact]
    public void WhenIsFullHouse()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.King, Suit.Club), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ten, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.True(hand.IsFullHouse(hands));
    }

    [Fact]
    public void WhenTreeOfAKindIsNotFullHouse()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.King, Suit.Club), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ace, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Ten, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.False(hand.IsFullHouse(hands));
    }

    [Fact]
    public void WhenIsNotFullHouse()
    {
        var hand = new HandRanker();
        var deck = new Deck();

        var hands = new List<(Rank, Suit)>();

        deck.TryDeal((Rank.Jack, Suit.Club), out var card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Four, Suit.Hart), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Spade), out card);
        hands.Add(card);
        deck.TryDeal((Rank.King, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.False(hand.IsFullHouse(hands));
    }
}
