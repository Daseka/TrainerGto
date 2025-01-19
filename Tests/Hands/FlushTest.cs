using Poker.Common.Hands;
using Poker.GameReader.Reporters;

namespace Tests.Hands;

public class FlushTest
{
    [Fact]
    public void ThreeSuitedTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Jack, CardSuit.Hart),
                (CardRank.Five, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Ten, CardSuit.Hart),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Two, CardSuit.Club),
                (CardRank.None, CardSuit.None),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = Flush.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.04, chance);
    }

    [Fact]
    public void FourSuitedOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Jack, CardSuit.Hart),
                (CardRank.Five, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Ten, CardSuit.Hart),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Two, CardSuit.Club),
                (CardRank.Seven, CardSuit.Hart),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = Flush.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.19, chance);
    }

    [Fact]
    public void FourSuitedTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Jack, CardSuit.Diamond),
                (CardRank.Five, CardSuit.Diamond)
            ],

            CommunityCards =
            [
                (CardRank.Ten, CardSuit.Diamond),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Seven, CardSuit.Diamond),
                (CardRank.None, CardSuit.None),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = Flush.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.35, chance);
    }

    [Fact]
    public void SpadeFlushTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Jack, CardSuit.Spade),
                (CardRank.Five, CardSuit.Spade)
            ],

            CommunityCards =
            [
                (CardRank.Ten, CardSuit.Spade),
                (CardRank.Three, CardSuit.Spade),
                (CardRank.Seven, CardSuit.Spade),
                (CardRank.None, CardSuit.None),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = Flush.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(1, chance);
    }

    [Fact]
    public void DiamondFlushOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Jack, CardSuit.Diamond),
                (CardRank.Five, CardSuit.Diamond)
            ],

            CommunityCards =
            [
                (CardRank.Ten, CardSuit.Diamond),
                (CardRank.Three, CardSuit.Diamond),
                (CardRank.Seven, CardSuit.Diamond),
                (CardRank.Two, CardSuit.Hart),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = Flush.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(1, chance);
    }

    [Fact]
    public void DrawingDeadTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Jack, CardSuit.Spade),
                (CardRank.Five, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Ten, CardSuit.Diamond),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Seven, CardSuit.Spade),
                (CardRank.None, CardSuit.None),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = Flush.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0, chance);
    }

    [Fact]
    public void DrawingDeadOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Jack, CardSuit.Spade),
                (CardRank.Five, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Ten, CardSuit.Diamond),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Seven, CardSuit.Spade),
                (CardRank.Ace, CardSuit.Spade),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = Flush.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0, chance);
    }
}