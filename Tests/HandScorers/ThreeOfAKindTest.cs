using Poker.Common;
using Poker.GtoBuilder;
using static Poker.Common.HandValues;

namespace Tests.HandScorers;

public class ThreeOfAKindTest
{
    [Fact]
    public void WhenSetOfAcesHigherThanSevens()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.Eight, Suit.Club),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Club),
            (Rank.Seven, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Diamond),
            (Rank.Six, Suit.Diamond),
            (Rank.Eight, Suit.Hart),
            (Rank.Ace, Suit.Hart),
            (Rank.Seven, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= ThreeOfAKindValue && heroScore < StraightValue);
    }

    [Fact]
    public void WhenHigherKickerWins()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.Eight, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Eight, Suit.Club),
            (Rank.Queen, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Nine, Suit.Diamond),
            (Rank.Nine, Suit.Club),
            (Rank.Nine, Suit.Hart),
            (Rank.Three, Suit.Hart),
            (Rank.Six, Suit.Spade),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= ThreeOfAKindValue && heroScore < StraightValue);
    }

    [Fact]
    public void WhenHigherKickerDoesntMatter()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Four, Suit.Spade),
            (Rank.Three, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Three, Suit.Club),
            (Rank.Two, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Nine, Suit.Diamond),
            (Rank.Nine, Suit.Club),
            (Rank.Nine, Suit.Hart),
            (Rank.King, Suit.Spade),
            (Rank.Seven, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= ThreeOfAKindValue && heroScore < StraightValue);
    }

    [Fact]
    public void WhenAllEqual()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ten, Suit.Spade),
            (Rank.Eight, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Ten, Suit.Club),
            (Rank.Eight, Suit.Club),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Nine, Suit.Diamond),
            (Rank.Nine, Suit.Club),
            (Rank.Nine, Suit.Spade),
            (Rank.King, Suit.Hart),
            (Rank.Seven, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= ThreeOfAKindValue && heroScore < StraightValue);
    }
}
