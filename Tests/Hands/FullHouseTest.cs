using Poker.GameReader.Hands;
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
                (CardSymbol.Five, CardSuit.Club),
                (CardSymbol.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardSymbol.Two, CardSuit.Spade),
                (CardSymbol.Three, CardSuit.Club),
                (CardSymbol.Six, CardSuit.Club),
                (CardSymbol.Jack, CardSuit.Diamond),
                (CardSymbol.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData);

        Assert.Equal(0, chance);
    }

    [Fact]
    public void OneOfAKindTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardSymbol.Five, CardSuit.Club),
                (CardSymbol.Nine, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardSymbol.Two, CardSuit.Spade),
                (CardSymbol.Three, CardSuit.Club),
                (CardSymbol.None, CardSuit.None),
                (CardSymbol.Jack, CardSuit.Diamond),
                (CardSymbol.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData);

        Assert.Equal(0, chance);
    }

    [Fact]
    public void TwoOfAKindOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardSymbol.Five, CardSuit.Club),
                (CardSymbol.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardSymbol.Five, CardSuit.Spade),
                (CardSymbol.Three, CardSuit.Club),
                (CardSymbol.Two, CardSuit.Club),
                (CardSymbol.Ten, CardSuit.Diamond),
                (CardSymbol.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData);

        Assert.Equal(0, chance);
    }

    [Fact]
    public void TwoOfAKindTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardSymbol.Five, CardSuit.Club),
                (CardSymbol.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardSymbol.Five, CardSuit.Spade),
                (CardSymbol.Three, CardSuit.Club),
                (CardSymbol.Two, CardSuit.Club),
                (CardSymbol.None, CardSuit.None),
                (CardSymbol.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData);

        Assert.Equal(0.0009, chance);
    }

    [Fact]
    public void ThreeOfAKindOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardSymbol.Five, CardSuit.Club),
                (CardSymbol.Five, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardSymbol.Five, CardSuit.Spade),
                (CardSymbol.Three, CardSuit.Club),
                (CardSymbol.Two, CardSuit.Club),
                (CardSymbol.Ten, CardSuit.Diamond),
                (CardSymbol.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData);

        Assert.Equal(0.07, chance);
    }

    [Fact]
    public void ThreeOfAKindTwoCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardSymbol.Five, CardSuit.Club),
                (CardSymbol.Five, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardSymbol.Five, CardSuit.Spade),
                (CardSymbol.Three, CardSuit.Club),
                (CardSymbol.Two, CardSuit.Club),
                (CardSymbol.None, CardSuit.None),
                (CardSymbol.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData);

        Assert.Equal(0.07, chance);
    }

    [Fact]
    public void TwoPairTwoCard()    
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardSymbol.Five, CardSuit.Club),
                (CardSymbol.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardSymbol.Eight, CardSuit.Spade),
                (CardSymbol.Five, CardSuit.Diamond),
                (CardSymbol.Two, CardSuit.Club),
                (CardSymbol.None, CardSuit.None),
                (CardSymbol.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData);

        Assert.Equal(0.04, chance);
    }

    [Fact]
    public void TwoPairOneCard()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardSymbol.Five, CardSuit.Club),
                (CardSymbol.Eight, CardSuit.Hart)
            ],

            CommunityCards =
            [
                (CardSymbol.Eight, CardSuit.Spade),
                (CardSymbol.Five, CardSuit.Diamond),
                (CardSymbol.Two, CardSuit.Club),
                (CardSymbol.Ace, CardSuit.Hart),
                (CardSymbol.None, CardSuit.None),
            ]
        };

        double chance = FullHouse.CalculateChance(gameData);

        Assert.Equal(0.04, chance);
    }
}