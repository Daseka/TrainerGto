using Poker.GameReader.Reporters;
using Poker.GameReader.Strategies;

namespace Tests
{
    public class SmallBlindTests
    {
        [Fact]
        public void WhenHasKingsHasNotBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.King,CardSuit.Hart),
                    (CardSymbol.King,CardSuit.Club)
                ],
                Position = Position.SmallBlind,
                Bets = [1, 2, 0, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 0.86,
                Call = 0.14,
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
                    (CardSymbol.King,CardSuit.Hart),
                    (CardSymbol.King,CardSuit.Club)
                ],
                Position = Position.SmallBlind,
                Bets = [1, 2, 3, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 1,
                Call = 0,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenHasFoursNotBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Four,CardSuit.Hart),
                    (CardSymbol.Four,CardSuit.Club)
                ],
                Position = Position.SmallBlind,
                Bets = [1, 2, 0, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0.0,
                Raise = 0.57,
                Call = 0.43,
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
                Position = Position.SmallBlind,
                Bets = [1, 2, 3, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0.75,
                Raise = 0.0,
                Call = 0.25,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenHasSevenFourSuitedNotBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Seven,CardSuit.Club),
                    (CardSymbol.Four,CardSuit.Club)
                ],
                Position = Position.SmallBlind,
                Bets = [1, 2, 0, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 0.24,
                Call = 0.76,
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
                Position = Position.SmallBlind,
                Bets = [1, 2, 3, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 1,
                Raise = 0,
                Call = 0,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenHasAceTenOffsuitNotBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Ace,CardSuit.Diamond),
                    (CardSymbol.Ten,CardSuit.Club)
                ],
                Position = Position.SmallBlind,
                Bets = [1, 2, 0, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 0.69,
                Call = 0.31,
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
                Position = Position.SmallBlind,
                Bets = [1, 2, 3, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 1,
                Raise = 0,
                Call = 0,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenHasKingJackOffsuitNotBeenRaised()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.King,CardSuit.Diamond),
                    (CardSymbol.Jack,CardSuit.Club)
                ],
                Position = Position.SmallBlind,
                Bets = [1, 2, 0, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 0.58,
                Call = 0.42,
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
                Position = Position.SmallBlind,
                Bets = [1, 2, 3, 0, 0]
            };

            var smallBlind = new SmallBlind();
            var result = smallBlind.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 1,
                Raise = 0,
                Call = 0,
            };

            Assert.Equal(expected, result);
        }
    }
}