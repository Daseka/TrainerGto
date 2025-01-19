using Poker.Common;
using Poker.GtoBuilder;
using static Poker.Common.HandValues;

namespace Tests.HandScorers;

public class FlushtTest
{
    [Fact]
    public void WhenAceFushHigherThanEight()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Hart),
            (Rank.Eight, Suit.Club),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Club),
            (Rank.Eight, Suit.Hart),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ten, Suit.Diamond),
            (Rank.Six, Suit.Hart),
            (Rank.Four, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Three, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= FlushValue && heroScore < FullHouseValue);
    }

    [Fact]
    public void WhenKingTwoFushHigherThanQueenJack()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.King, Suit.Hart),
            (Rank.Two, Suit.Hart),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Queen, Suit.Hart),
            (Rank.Eight, Suit.Hart),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ten, Suit.Diamond),
            (Rank.Six, Suit.Hart),
            (Rank.Four, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Three, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= FlushValue && heroScore < FullHouseValue);
    }

    [Fact]
    public void WhenHandCardDontMatter()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.King, Suit.Spade),
            (Rank.Two, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Queen, Suit.Diamond),
            (Rank.Five, Suit.Hart),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ten, Suit.Hart),
            (Rank.Six, Suit.Hart),
            (Rank.Queen, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.King, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= FlushValue && heroScore < FullHouseValue);
    }

    [Fact]
    public void WhenAllEqual()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.King, Suit.Spade),
            (Rank.Two, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Queen, Suit.Diamond),
            (Rank.Eight, Suit.Club),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Ten, Suit.Hart),
            (Rank.Six, Suit.Hart),
            (Rank.Four, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Three, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= FlushValue && heroScore < FullHouseValue);
    }
}
