using Poker.Common;
using Poker.GtoBuilder;
using static Poker.Common.HandValues;

namespace Tests.HandScorers;

public class TwoPairTest
{
    [Fact]
    public void WhenAceFourHigherThanSevenSix()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Ace, Suit.Spade),
            (Rank.Four, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Seven, Suit.Club),
            (Rank.Six, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Six, Suit.Spade),
            (Rank.Seven, Suit.Diamond),
            (Rank.Ace, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Four, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= TwoPairValue && heroScore < ThreeOfAKindValue);
    }

    [Fact]
    public void WhenEightFourHigherThanEightThree()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Eight, Suit.Spade),
            (Rank.Four, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Eight, Suit.Club),
            (Rank.Three, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Three, Suit.Spade),
            (Rank.Seven, Suit.Diamond),
            (Rank.Eight, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Four, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= TwoPairValue && heroScore < ThreeOfAKindValue);
    }

    [Fact]
    public void WhenHigherKickerWins()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Eight, Suit.Spade),
            (Rank.Ace, Suit.Diamond),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Eight, Suit.Club),
            (Rank.Queen, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Four, Suit.Spade),
            (Rank.Seven, Suit.Diamond),
            (Rank.Eight, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Four, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore > villainScore);
        Assert.True(heroScore >= TwoPairValue && heroScore < ThreeOfAKindValue);
    }

    [Fact]
    public void WhenKickerDoesntMatter()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Eight, Suit.Spade),
            (Rank.Six, Suit.Spade),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Eight, Suit.Club),
            (Rank.Two, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Four, Suit.Spade),
            (Rank.Seven, Suit.Diamond),
            (Rank.Eight, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Four, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= TwoPairValue && heroScore < ThreeOfAKindValue);
    }

    [Fact]
    public void WhenAllIsEqual()
    {
        var hero = new (Rank, Suit)[]
        {
            (Rank.Eight, Suit.Spade),
            (Rank.Six, Suit.Spade),
        };

        var villain = new (Rank, Suit)[]
        {
            (Rank.Eight, Suit.Club),
            (Rank.Six, Suit.Diamond),
        };

        var community = new (Rank, Suit)[]
        {
            (Rank.Four, Suit.Spade),
            (Rank.Seven, Suit.Diamond),
            (Rank.Eight, Suit.Hart),
            (Rank.Jack, Suit.Hart),
            (Rank.Four, Suit.Hart),
        };

        var heroScore = HandScorer.ScoreHand(hero, community);
        var villainScore = HandScorer.ScoreHand(villain, community);

        Assert.True(heroScore == villainScore);
        Assert.True(heroScore >= TwoPairValue && heroScore < ThreeOfAKindValue);
    }
}
