using Poker.GameReader.Strategies;

namespace Poker.GameReader.Reporters;

public class StrategyReporter
{
    private int _lastPick;
    private GameData _previousGameData;

    public StrategyData GetStrategy(GameData gameData)
    {
        StrategyData strategyData = IsAfterFlop(gameData)
            ? new StrategyData { Call = 0, Fold = 0, Raise = 0, SugestedAction = "No strategy" }
            : GetPreFlopStrategy(gameData);

        return strategyData;
    }

    private static bool IsAfterFlop(GameData gameData)
    {
        return gameData.MiddleCards[0] != (CardSymbol.None, CardSuit.None)
            || gameData.MiddleCards[1] != (CardSymbol.None, CardSuit.None)
            || gameData.MiddleCards[2] != (CardSymbol.None, CardSuit.None)
            || gameData.HandCards[0] == (CardSymbol.None, CardSuit.None)
            || gameData.HandCards[1] == (CardSymbol.None, CardSuit.None);
    }

    private StrategyData GetPreFlopStrategy(GameData gameData)
    {
        StrategyBuilder strategyBuilder = new();
        IStrategy strategy = strategyBuilder.Build(gameData);
        StrategySolution strategySolution = strategy.Solve(gameData);

        int call = (int)Math.Round(strategySolution.Call * 100, 0);
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
            _lastPick = new Random().Next(min, total);
            _previousGameData = gameData;
        }

        string onlyAThought = string.Empty;
        if (gameData.CallAmount <= 0 )
        {
            onlyAThought = "Thinking";
        }

        string suggestion;
        if (gameData.Position == Position.BigBlind && !gameData.HasBeenRaised)
        {
            suggestion = " CHECK";
        }
        else if (_lastPick == 0)
        {
            suggestion = $" NONE";
        }
        else if (_lastPick <= call)
        {
            suggestion = $"{onlyAThought} CALL (roll {_lastPick})";
        }
        else if (_lastPick <= call + raise)
        {
            suggestion = $"{onlyAThought} RAISE (roll {_lastPick})";
        }
        else
        {
            suggestion = $"{onlyAThought} FOLD (roll {_lastPick})";
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