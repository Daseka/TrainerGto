using Poker.GameReader.Reporters;
using Poker.GameReader.Strategies;
using System.Collections.Generic;

namespace Tests.Strategies
{
    public class CutOffTests
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
                Position = Position.CutOff,
                Bets = [1, 2, 0, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 1,
                Call = 0,
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
                Position = Position.CutOff,
                Bets = [1, 2, 3, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

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
                SmallBlind = 1,
                BigBlind = 2,
                HandCards =
                [
                    (CardSymbol.Four,CardSuit.Hart),
                    (CardSymbol.Four,CardSuit.Club)
                ],
                Position = Position.CutOff,
                Bets = [1, 2, 0, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0.0,
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
                Position = Position.CutOff,
                Bets = [1, 2, 3, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0.96,
                Raise = 0.0,
                Call = 0.04,
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
                Position = Position.CutOff,
                Bets = [1, 2, 0, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 1,
                Raise = 0,
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
                    (CardSymbol.Seven,CardSuit.Club),
                    (CardSymbol.Four,CardSuit.Club)
                ],
                Position = Position.CutOff,
                Bets = [1, 2, 3, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

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
                SmallBlind = 1,
                BigBlind = 2,
                HandCards =
                [
                    (CardSymbol.Ace,CardSuit.Diamond),
                    (CardSymbol.Ten,CardSuit.Club)
                ],
                Position = Position.CutOff,
                Bets = [1, 2, 0, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 1,
                Call = 0,
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
                Position = Position.CutOff,
                Bets = [1, 2, 3, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

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
                SmallBlind = 1,
                BigBlind = 2,
                HandCards =
                [
                    (CardSymbol.King,CardSuit.Diamond),
                    (CardSymbol.Jack,CardSuit.Club)
                ],
                Position = Position.CutOff,
                Bets = [1, 2, 0, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 1,
                Call = 0,
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
                Position = Position.CutOff,
                Bets = [1, 2, 3, 0, 0]
            };

            var cutOff = new CutOff();
            var result = cutOff.Solve(gameData);

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