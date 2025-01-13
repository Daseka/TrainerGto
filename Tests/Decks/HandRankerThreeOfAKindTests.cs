using Poker.Common;
using Poker.GtoBuilder;

namespace Tests.Decks;

public class HandRankerThreeOfAKindTests
{
    [Fact]
    public void WhenIsThreeOfAKind()
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
        deck.TryDeal((Rank.Queen, Suit.Diamond), out card);
        hands.Add(card);
        deck.TryDeal((Rank.Queen, Suit.Club), out card);
        hands.Add(card);

        Assert.True(hand.IsThreeOfAKind(hands));
    }

    [Fact]
    public void WhenIsNotThreeOfAKind()
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

        Assert.False(hand.IsThreeOfAKind(hands));
    }
}
