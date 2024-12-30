using Poker.GtoBuilder;
using Poker.GtoBuilder.CardDisplay;

namespace Tests.Decks;

public class DeckTest
{
    [Fact]
    public void GetCardsInDeckTwoDealt()
    {
        var deck = new Deck();
        deck.TryDeal(out _);
        deck.TryDeal(out _);

        var cards = deck.GetRemaining();
        int allCardsInDeckCount = 50;

        Assert.True(cards.Length == allCardsInDeckCount);
    }

    [Fact]
    public void GetCardsInDeckZeroDealt()
    {
        var deck = new Deck();
        var cards = deck.GetRemaining();

        int allCardsInDeckCount = 52;

        Assert.True(cards.Length == allCardsInDeckCount);
    }

    [Fact]
    public void WhenDealingSpecificCardAndNextFirstCardTheyShouldntBeInDeckAnymore()
    {
        var deck = new Deck();

        deck.Shuffle();
        deck.TryDeal((Rank.Ten, Suit.Diamond), out var specifictCard);
        deck.Shuffle();
        deck.TryDeal(out var firstCard);
        deck.Shuffle();

        var cards = deck.GetRemaining();

        Assert.Multiple(() =>
        {
            Assert.All(cards, x => Assert.NotEqual(x, specifictCard));
            Assert.All(cards, x => Assert.NotEqual(x, firstCard));
            Assert.True(cards.Length == 50);
        });
    }

    [Fact]
    public void WhenDealingSpecificCardItShouldntBeInDeckAnymore()
    {
        var deck = new Deck();
        deck.TryDeal((Rank.Ten, Suit.Diamond), out var cardDealt);

        var cards = deck.GetRemaining();

        Assert.All(cards, x => Assert.NotEqual(x, cardDealt));
    }

    [Fact]
    public void WhenShuffeldAfterDealingShouldntHaveDealtCards()
    {
        var deck = new Deck();
        deck.TryDeal(out var cardOne);
        deck.TryDeal(out var cardTwo);

        deck.Shuffle();
        deck.Shuffle();
        var cards = deck.GetRemaining();

        Assert.All(cards, x => Assert.NotEqual(x, cardTwo));
        Assert.All(cards, x => Assert.NotEqual(x, cardOne));

        deck.TryDeal(out var cardThree);
        deck.TryDeal(out var cardFour);

        deck.Shuffle();
        deck.Shuffle();
        cards = deck.GetRemaining();

        Assert.All(cards, x => Assert.NotEqual(x, cardThree));
        Assert.All(cards, x => Assert.NotEqual(x, cardFour));
    }

    [Fact]
    public void WhenShuffeldAfterDealingShouldStillHaveSameCount()
    {
        var deck = new Deck();
        deck.TryDeal(out _);
        deck.TryDeal(out _);

        deck.Shuffle();
        deck.Shuffle();

        var cards = deck.GetRemaining();
        int allCardsInDeckCount = 50;

        Assert.True(cards.Length == allCardsInDeckCount);
    }

    [Fact]
    public void WhenShuffeldWithSeed1234()
    {
        var deck = new Deck(1234);
        deck.Shuffle();

        var cards = deck.Peek(4);

        Assert.Multiple(() =>
        {
            Assert.Equal(cards[0], (Rank.Eight, Suit.Hart));
            Assert.Equal(cards[1], (Rank.Seven, Suit.Spade));
            Assert.Equal(cards[2], (Rank.Five, Suit.Hart));
            Assert.Equal(cards[3], (Rank.Ten, Suit.Spade));
            Assert.Equal(cards[4], (Rank.Seven, Suit.Hart));
        });
    }
}