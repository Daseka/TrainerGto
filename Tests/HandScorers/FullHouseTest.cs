using Poker.Common;
using Poker.GtoBuilder;
using static Poker.Common.HandValues;

namespace Tests.HandScorers;

public class FullHouseTest
{
    [Fact]
    public void WhenAceKingHigherThanKingAce()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.Ace, Suit.Hart),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Club),
            (Rank.King, Suit.Spade),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Diamond),
            (Rank.King, Suit.Diamond),
            (Rank.King, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Three, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= FullHouseValue && heroScore < FourOfAKindValue);
    }

    [Fact]
    public void WhenAceKingHigherThanAceQueen()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.King, Suit.Hart),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Club),
            (Rank.Queen, Suit.Spade),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Diamond),
            (Rank.Ace, Suit.Hart),
            (Rank.King, Suit.Club),
            (Rank.Queen, Suit.Hart),
            (Rank.Three, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= FullHouseValue && heroScore < FourOfAKindValue);
    }

    [Fact]
    public void WhenAllEqual()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Spade),
            (Rank.King, Suit.Hart),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Club),
            (Rank.Queen, Suit.Spade),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Diamond),
            (Rank.Ace, Suit.Hart),
            (Rank.Ace, Suit.Club),
            (Rank.Five, Suit.Hart),
            (Rank.Three, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= FullHouseValue && heroScore < FourOfAKindValue);
    }

    [Fact]
    public void WhenNothingMatters()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Six, Suit.Spade),
            (Rank.King, Suit.Hart),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Six, Suit.Club),
            (Rank.Queen, Suit.Spade),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Diamond),
            (Rank.Ace, Suit.Hart),
            (Rank.Ace, Suit.Club),
            (Rank.Five, Suit.Hart),
            (Rank.Five, Suit.Spade),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= FullHouseValue && heroScore < FourOfAKindValue);
    }
}
