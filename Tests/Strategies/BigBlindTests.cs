using Poker.GameReader.Reporters;
using Poker.GameReader.Strategies;

namespace Tests.Strategies
{
    public class BigBlindTests
    {
        [Fact]
        public void WhenHasKingsHasBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.King,CardSuit.Hart),
                    (CardSymbol.King,CardSuit.Club)
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
        public void WhenHasFoursBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Four,CardSuit.Hart),
                    (CardSymbol.Four,CardSuit.Club)
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
        public void WhenHasSevenFourSuitedBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Seven,CardSuit.Club),
                    (CardSymbol.Four,CardSuit.Club)
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
        public void WhenHasAceTenOffsuitBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Ace,CardSuit.Diamond),
                    (CardSymbol.Ten,CardSuit.Club)
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
        public void WhenHasKingJackOffsuitBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.King,CardSuit.Diamond),
                    (CardSymbol.Jack,CardSuit.Club)
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
    }
}