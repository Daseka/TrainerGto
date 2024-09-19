using Poker.GameReader.Reporters;
using static Poker.GameReader.Reporters.CardSymbol;

namespace Poker.GameReader.Strategies;

public class StrategyBuilder
{
    public IStrategy Build(GameData gameData)
    {
        IStrategy strategy = gameData switch
        {
            { Position: Position.Button } => new Button(),
            { Position: Position.SmallBlind } => new SmallBlind(),
            { Position: Position.BigBlind } => new BigBlind(),
            { Position: Position.UnderTheGun } => new UnderTheGun(),
            { Position: Position.HighJack } => new HighJack(),
            { Position: Position.CutOff} => new CutOff(),
            _ => throw new NotImplementedException()
        };
        
        return strategy;
    }}
