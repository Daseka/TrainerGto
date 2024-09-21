using Poker.GameReader.Reporters;
using Poker.GameReader.Strategies;

namespace Tests
{
    public class UnderTheGunTests
    {
        [Fact]
        public void WhenHasKings()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.King,CardSuit.Hart),
                    (CardSymbol.King,CardSuit.Club)
                ],
                Position = Position.UnderTheGun,
                Bets = [1, 2, 0, 0, 0]
            };

            var underTheGun = new UnderTheGun();
            var result = underTheGun.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 1,
                Call = 0,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenHasFours()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Four,CardSuit.Hart),
                    (CardSymbol.Four,CardSuit.Club)
                ],
                Position = Position.UnderTheGun,
                Bets = [1, 2, 0, 0, 0]
            };

            var underTheGun = new UnderTheGun();
            var result = underTheGun.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0.81,
                Raise = 0.19,
                Call = 0,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenHasSevenFourSuited()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Seven,CardSuit.Club),
                    (CardSymbol.Four,CardSuit.Club)
                ],
                Position = Position.UnderTheGun,
                Bets = [1, 2, 0, 0, 0]
            };

            var underTheGun = new UnderTheGun();
            var result = underTheGun.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 1,
                Raise = 0,
                Call = 0,
            };  

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenHasAceTenOffsuit()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.Ace,CardSuit.Diamond),
                    (CardSymbol.Ten,CardSuit.Club)
                ],
                Position = Position.UnderTheGun,
                Bets = [1, 2, 0, 0, 0]
            };

            var underTheGun = new UnderTheGun();
            var result = underTheGun.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0,
                Raise = 1,
                Call = 0,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenHasKingJackOffsuit()
        {
            var gameData = new GameData
            {
                HandCards =
                [
                    (CardSymbol.King,CardSuit.Diamond),
                    (CardSymbol.Jack,CardSuit.Club)
                ],
                Position = Position.UnderTheGun,
                Bets = [1, 2, 0, 0, 0]
            };

            var underTheGun = new UnderTheGun();
            var result = underTheGun.Solve(gameData);

            var expected = new StrategySolution
            {
                Fold = 0.03,
                Raise = 0.97,
                Call = 0,
            };

            Assert.Equal(expected, result);
        }
    }
}