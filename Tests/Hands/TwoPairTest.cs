using Poker.Common.Hands;
using Poker.GameReader.Reporters;

namespace Tests.Hands;

public class TwoPairTest
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

        double chance = TwoPair.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

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

        double chance = TwoPair.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.008, chance);
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

        double chance = TwoPair.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.06, chance);
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

        double chance = TwoPair.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(0.06, chance);
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

        double chance = TwoPair.CalculateChance(gameData.HandCards.Concat(gameData.CommunityCards));

        Assert.Equal(1, chance);
    }
}