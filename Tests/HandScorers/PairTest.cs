using Poker.Common;
using Poker.GtoBuilder;
using static Poker.Common.HandValues;

namespace Tests.HandScorers;

public class PairTest
{
    [Fact]
    public void WhenAceHigherThanThrees()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.Two, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Three, Suit.Club),
            (Rank.King, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Diamond),
            (Rank.Three, Suit.Diamond),
            (Rank.Eight, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Ten, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= PairValue && heroScore < TwoPairValue);
    }

    [Fact]
    public void WhenHigherKickerWins()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Spade),
            (Rank.King, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Club),
            (Rank.Four, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Diamond),
            (Rank.Three, Suit.Diamond),
            (Rank.Eight, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Ten, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= PairValue && heroScore < TwoPairValue);
    }

    [Fact]
    public void WhenHigherKickerDoesntMatter()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Spade),
            (Rank.Six, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Club),
            (Rank.Four, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Diamond),
            (Rank.Queen, Suit.Diamond),
            (Rank.Eight, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Ten, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= PairValue && heroScore < TwoPairValue);
    }

    [Fact]
    public void WhenAllIsEqual()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Spade),
            (Rank.King, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Club),
            (Rank.King, Suit.Hart),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Five, Suit.Diamond),
            (Rank.Three, Suit.Diamond),
            (Rank.Eight, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Ten, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= PairValue && heroScore < TwoPairValue);
    }
}
