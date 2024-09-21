using Poker.GameReader.Strategies;
using System.Diagnostics.CodeAnalysis;

namespace Poker.GameReader.Reporters;

public class StrategyReporter
{
    private GameData _previousGameData;
    private int _lastPick;

    public StrategyData GetStrategy(GameData gameData)
    {
        StrategyBuilder strategyBuilder = new ();
        IStrategy strategy =  strategyBuilder.Build(gameData);
        StrategySolution strategySolution = strategy.Solve(gameData);

        int call = (int)Math.Round(strategySolution.Call * 100,0);
        int raise = (int)Math.Round(strategySolution.Raise * 100, 0);
        int fold = (int)Math.Round(strategySolution.Fold * 100, 0);

        //only change sugestion when hand changes or HasBeenRaised changes
        if (_previousGameData.HandCards is null 
            || gameData.HandCards[0].cardSymbol != _previousGameData.HandCards[0].cardSymbol
            || gameData.HandCards[0].cardSuit != _previousGameData.HandCards[0].cardSuit
            || gameData.HandCards[1].cardSymbol != _previousGameData.HandCards[1].cardSymbol
            || gameData.HandCards[1].cardSuit != _previousGameData.HandCards[1].cardSuit
            || gameData.HasBeenRaised != _previousGameData.HasBeenRaised)
        {
            int total = call + raise + fold;
            int min = total <= 0 ? 0 : 1;
            _lastPick = new Random().Next(min,total);
            _previousGameData = gameData;
        }

        string suggestion;
        if(gameData.Position == Position.BigBlind && gameData.Bets.Max() == gameData.CallAmount )
        {
            suggestion = "CHECK";
        }
        else if (_lastPick == 0)
        {
            suggestion = $"NONE";
        }
        else if (_lastPick <= call)
        {
            suggestion = $"CALL (roll {_lastPick})";
        }
        else if (_lastPick <= call + raise)
        {
            suggestion = $"RAISE (roll {_lastPick})";
        }
        else
        {
            suggestion = $"FOLD (roll {_lastPick})";
        }

        StrategyData strategyData = new()
        {
            Call = call,
            Raise = raise,
            Fold = fold,
            SugestedAction = suggestion
        };

        return strategyData;
    }
}
