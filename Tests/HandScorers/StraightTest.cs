using Poker.Common;
using Poker.GtoBuilder;
using static Poker.Common.HandValues;

namespace Tests.HandScorers;

public class StraightTest
{
    [Fact]
    public void WhenStraightOfEightHigherThanStraightOfSeven()
    {
       var hero = new (Rank, Suit)[]
       {
            (Rank.Seven, Suit.Spade),
            (Rank.Eight, Suit.Club),
       };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Club),
            (Rank.King, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Diamond),
            (Rank.Six, Suit.Diamond),
            (Rank.Four, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Three, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= StraightValue && heroScore < FlushValue);
    }

    [Fact]
    public void WhenAceHighWins()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.King, Suit.Club),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Club),
            (Rank.King, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Queen, Suit.Diamond),
            (Rank.Jack, Suit.Diamond),
            (Rank.Ten, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Nine, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= StraightValue && heroScore < FlushValue);
    }

    [Fact]
    public void WhenAceHighThanTwoPair()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.Two, Suit.Club),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Club),
            (Rank.Seven, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Three, Suit.Diamond),
            (Rank.Jack, Suit.Diamond),
            (Rank.Four, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Five, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= StraightValue && heroScore < FlushValue);
    }

    [Fact]
    public void WhenAllEqual()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ten, Suit.Spade),
            (Rank.Queen, Suit.Club),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Club),
            (Rank.King, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Three, Suit.Diamond),
            (Rank.Six, Suit.Diamond),
            (Rank.Four, Suit.Hart),
            (Rank.Seven, Suit.Hart),
            (Rank.Five, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= StraightValue && heroScore < FlushValue);
    }
}
