using Poker.GameReader.Reporters;
using Poker.GameReader.Strategies;

namespace Tests
{
    public class StrategyBuilderTests
    {
        [Fact]
        public void WhenPositionIsBigBlindItShouldReturnBigBlindStrategy()
        {
            var gameData = new GameData { Position = Position.BigBlind };

            var strategySelector = new StrategyBuilder();
            IStrategy strategy = strategySelector.Build(gameData);

            Assert.IsType<BigBlind>(strategy);
        }

        [Fact]
        public void WhenPositionIsButtonItShouldReturnButtonStrategy()
        {
            var gameData = new GameData { Position = Position.Button };

            var strategySelector = new StrategyBuilder();
            IStrategy strategy = strategySelector.Build(gameData);

            Assert.IsType<Button>(strategy);
        }

        [Fact]
        public void WhenPositionIsCutOffItShouldReturnCutOffStrategy()
        {
            var gameData = new GameData { Position = Position.CutOff };

            var strategySelector = new StrategyBuilder();
            IStrategy strategy = strategySelector.Build(gameData);

            Assert.IsType<CutOff>(strategy);
        }

        [Fact]
        public void WhenPositionIsHighJackItShouldReturnHighJackStrategy()
        {
            var gameData = new GameData { Position = Position.HighJack };

            var strategySelector = new StrategyBuilder();
            IStrategy strategy = strategySelector.Build(gameData);

            Assert.IsType<HighJack>(strategy);
        }

        [Fact]
        public void WhenPositionIsSmallBlindItShouldReturnSmallBlindStrategy()
        {
            var gameData = new GameData { Position = Position.SmallBlind };

            var strategySelector = new StrategyBuilder();
            IStrategy strategy = strategySelector.Build(gameData);

            Assert.IsType<SmallBlind>(strategy);
        }

        [Fact]
        public void WhenPositionIsUnderTheGunItShouldReturnUnderTheGunStrategy()
        {
            var gameData = new GameData { Position = Position.UnderTheGun };

            var strategySelector = new StrategyBuilder();
            IStrategy strategy = strategySelector.Build(gameData);

            Assert.IsType<UnderTheGun>(strategy);
        }
    }
}