using Poker.GameReader.Reporters;

namespace Poker.GameReader.Strategies
{
    public interface IStrategy
    {
        StrategySolution Solve(GameData gameData);
    }
}