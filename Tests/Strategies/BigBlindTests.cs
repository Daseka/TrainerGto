using Poker.GameReader.Reporters;
using Poker.GameReader.Strategies;

namespace Tests.Strategies;
public class BigBlindTests
{
    [Fact]
    public void WhenHasAceTenOffsuitBeenRaised()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Ace,CardSuit.Diamond),
                (CardRank.Ten,CardSuit.Club)
            ],
            Position = Position.BigBlind,
            Bets = [1, 2, 3, 0, 0]
        };

        var bigBlind = new BigBlind();
        var result = bigBlind.Solve(gameData);

        var expected = new StrategySolution
        {
            Fold = 0,
            Raise = 0.10,
            Call = 0.90,
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void WhenHasFoursBeenRaised()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Four,CardSuit.Hart),
                (CardRank.Four,CardSuit.Club)
            ],
            Position = Position.BigBlind,
            Bets = [1, 2, 3, 0, 0]
        };

        var bigBlind = new BigBlind();
        var result = bigBlind.Solve(gameData);

        var expected = new StrategySolution
        {
            Fold = 0,
            Raise = 0,
            Call = 1,
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void WhenHasKingJackOffsuitBeenRaised()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.King,CardSuit.Diamond),
                (CardRank.Jack,CardSuit.Club)
            ],
            Position = Position.BigBlind,
            Bets = [1, 2, 3, 0, 0]
        };

        var bigBlind = new BigBlind();
        var result = bigBlind.Solve(gameData);

        var expected = new StrategySolution
        {
            Fold = 0,
            Raise = 0.09,
            Call = 0.91,
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void WhenHasKingsHasBeenRaised()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.King,CardSuit.Hart),
                (CardRank.King,CardSuit.Club)
            ],
            Position = Position.BigBlind,
            Bets = [1, 2, 3, 0, 0]
        };

        var bigBlind = new BigBlind();
        var result = bigBlind.Solve(gameData);

        var expected = new StrategySolution
        {
            Fold = 0,
            Raise = 1,
            Call = 0,
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void WhenHasSevenFourSuitedBeenRaised()
    {
        var gameData = new GameData
        {
            HandCards =
            [
                (CardRank.Seven,CardSuit.Club),
                (CardRank.Four,CardSuit.Club)
            ],
            Position = Position.BigBlind,
            Bets = [1, 2, 3, 0, 0]
        };

        var bigBlind = new BigBlind();
        var result = bigBlind.Solve(gameData);

        var expected = new StrategySolution
        {
            Fold = 0,
            Raise = 0,
            Call = 1,
        };

        Assert.Equal(expected, result);
    }
}