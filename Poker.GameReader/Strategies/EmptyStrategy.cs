using Poker.GameReader.Reporters;

namespace Poker.GameReader.Strategies;

public class EmptyStrategy : BaseStrategy
{
    public override StrategySolution Solve(GameData gameData)
    {
        return new StrategySolution();
    }
}
