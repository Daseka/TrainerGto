
namespace Poker.GameReader.Reporters
{
    public interface IStrategyReporter
    {
        Task<StrategyData> GetStrategy(GameData gameData);
    }
}