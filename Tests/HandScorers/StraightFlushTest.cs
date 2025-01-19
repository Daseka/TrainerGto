using Poker.Common;
using Poker.GtoBuilder;
using static Poker.Common.HandValues;

namespace Tests.HandScorers;

public class StraightFlushTest
{
    [Fact]
    public void WhenStraightOfEightHigherThanStraightOfSeven()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Spade),
            (Rank.Eight, Suit.Spade),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Club),
            (Rank.King, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Spade),
            (Rank.Six, Suit.Spade),
            (Rank.Four, Suit.Spade),
            (Rank.Jack, Suit.Hart),
            (Rank.Three, Suit.Spade),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= StraightFlushValue);
    }

    [Fact]
    public void WhenAceHighWins()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.King, Suit.Spade),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Club),
            (Rank.King, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Queen, Suit.Spade),
            (Rank.Jack, Suit.Spade),
            (Rank.Ten, Suit.Spade),
            (Rank.Jack, Suit.Hart),
            (Rank.Nine, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= StraightFlushValue);
    }

    [Fact]
    public void WhenAceHighThanTwoPair()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.Two, Suit.Spade),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Club),
            (Rank.Seven, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Three, Suit.Spade),
            (Rank.Jack, Suit.Diamond),
            (Rank.Four, Suit.Spade),
            (Rank.Jack, Suit.Hart),
            (Rank.Five, Suit.Spade),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= StraightFlushValue);
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
            (Rank.Four, Suit.Diamond),
            (Rank.Seven, Suit.Diamond),
            (Rank.Five, Suit.Diamond),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= StraightFlushValue);
    }
}
