using Poker.GameReader.Reporters;
using static Poker.GameReader.Reporters.CardSymbol;

namespace Poker.GameReader.Strategies;

public class StrategySelector
{
    public StrategySolution Sovle(GameData gameData)
    {
        if (IsUnderTheGun(gameData))
        {
            return UnderTheGun.GetSolution(gameData);
        }

        return new StrategySolution();
    }

    private static bool IsUnderTheGun(GameData gameData)
    {
        return gameData.HandCards[0].cardSymbol != None 
            && gameData.HandCards[1].cardSymbol != None 
            && gameData.Position == Position.UnderTheGun;
    }
}
