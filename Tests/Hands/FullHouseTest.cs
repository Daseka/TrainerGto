using Poker.Common.Hands;
using Poker.GameReader.Reporters;

namespace Tests.Hands;

public class FullHouseTest
{
    [Fact]
    public void OneOfAKindOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Five, CardSuit.Club),
                (CardRank.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Two, CardSuit.Spade),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Six, CardSuit.Club),
                (CardRank.Jack, CardSuit.Diamond),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0, chance);
    }

    [Fact]
    public void OneOfAKindTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Five, CardSuit.Club),
                (CardRank.Nine, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Two, CardSuit.Spade),
                (CardRank.Three, CardSuit.Club),
                (CardRank.None, CardSuit.None),
                (CardRank.Jack, CardSuit.Diamond),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0, chance);
    }

    [Fact]
    public void TwoOfAKindOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Five, CardSuit.Club),
                (CardRank.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Five, CardSuit.Spade),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Two, CardSuit.Club),
                (CardRank.Ten, CardSuit.Diamond),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0, chance);
    }

    [Fact]
    public void TwoOfAKindTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Five, CardSuit.Club),
                (CardRank.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Five, CardSuit.Spade),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Two, CardSuit.Club),
                (CardRank.None, CardSuit.None),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.0009, chance);
    }

    [Fact]
    public void ThreeOfAKindOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Five, CardSuit.Club),
                (CardRank.Five, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Five, CardSuit.Spade),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Two, CardSuit.Club),
                (CardRank.Ten, CardSuit.Diamond),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.07, chance);
    }

    [Fact]
    public void ThreeOfAKindTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Five, CardSuit.Club),
                (CardRank.Five, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Five, CardSuit.Spade),
                (CardRank.Three, CardSuit.Club),
                (CardRank.Two, CardSuit.Club),
                (CardRank.None, CardSuit.None),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.07, chance);
    }

    [Fact]
    public void TwoPairTwoCard()    
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Five, CardSuit.Club),
                (CardRank.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Eight, CardSuit.Spade),
                (CardRank.Five, CardSuit.Diamond),
                (CardRank.Two, CardSuit.Club),
                (CardRank.None, CardSuit.None),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.04, chance);
    }

    [Fact]
    public void TwoPairOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Five, CardSuit.Club),
                (CardRank.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardRank.Eight, CardSuit.Spade),
                (CardRank.Five, CardSuit.Diamond),
                (CardRank.Two, CardSuit.Club),
                (CardRank.Ace, CardSuit.Hart),
                (CardRank.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.04, chance);
    }
}