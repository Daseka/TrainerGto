using Poker.GameReader.Hands;
using Poker.GameReader.Reporters;

namespace Tests.Hands;

public class TwoOfAKindTest
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

        double chance = TwoOfAKind.CalculateChance(gameData);

        Assert.Equal(0.13, chance);
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

        double chance = TwoOfAKind.CalculateChance(gameData);

        Assert.Equal(0.13, chance);
    }

    [Fact]
    public void TwoOfAKindOneCard()
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

        double chance = TwoOfAKind.CalculateChance(gameData);

        Assert.Equal(1, chance);
    }

    [Fact]
    public void TwoOfAKindTwoCard()
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
                (CardSymbol.Ten, CardSuit.Spade),
                (CardSymbol.Three, CardSuit.Club),
                (CardSymbol.Two, CardSuit.Club),
                (CardSymbol.None, CardSuit.None),
                (CardSymbol.None, CardSuit.None),
            ]
        };

        double chance = TwoOfAKind.CalculateChance(gameData);

        Assert.Equal(1, chance);
    }
}
