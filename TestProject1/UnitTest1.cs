using Poker.GameReader.Reporters;
using Poker.GameReader.Strategies;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            StrategySelector strategySelector = new StrategySelector();
            var gameData = new GameData
            {
                HandCards = 
                [
                    (CardSymbol.King, CardSuit.Diamond),
                    (CardSymbol.Six, CardSuit.Diamond)
                ],
                Position = Position.UnderTheGun,

            };

            var thing = strategySelector.Sovle(gameData);

            var call = thing.Call;
            var raise = thing.Raise;
            var fold = thing.Fold;

            Assert.Equal(0, call);
            Assert.Equal(0.32, raise);
            Assert.Equal(0.68, fold);
        }
    }
}