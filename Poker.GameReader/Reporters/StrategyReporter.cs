using Poker.Common;
using Poker.GameReader.Strategies;
using Poker.GtoBuilder;
using System.Text;

namespace Poker.GameReader.Reporters;

public class StrategyReporter
{
    private const string NoStrategy = "No Strategy";
    private int _lastPick;
    private StrategyData _lastStrategyData;
    private GameData _previousGameData;

    public StrategyData GetStrategy(GameData gameData)
    {
        StrategyData strategyData = IsAfterFlop(gameData)
            ? GetPostFlopStrategy(gameData)
            : GetPreFlopStrategy(gameData);

        return strategyData;
    }

    private static bool IsAfterFlop(GameData gameData)
    {
        return gameData.CommunityCards[0] != (CardRank.None, CardSuit.None)
            || gameData.CommunityCards[1] != (CardRank.None, CardSuit.None)
            || gameData.CommunityCards[2] != (CardRank.None, CardSuit.None)
            || gameData.HandCards[0] == (CardRank.None, CardSuit.None)
            || gameData.HandCards[1] == (CardRank.None, CardSuit.None);
    }

    private static StrategyData RunWinChanceSimulation(GameData gameData)
    {
        var strategyData = new StrategyData();
        var handSimulator = new HandSimulator();

        IEnumerable<(Rank, Suit)> knownCards = gameData.HandCards
            .Concat(gameData.CommunityCards)
            .Select(x => ((Rank)x.cardRank, (Suit)x.cardSuit));

        var handCards = gameData.HandCards.Select(x => ((Rank)x.cardRank, (Suit)x.cardSuit));
        var bets = gameData.Bets.Where(x => x > 0).Select(x => (int)x);
        var communityCards = gameData.CommunityCards.Select(x => ((Rank)x.cardRank, (Suit)x.cardSuit));
        var (win, draw, loss) = handSimulator.SimulateWinChance([.. handCards], [.. bets], [.. communityCards]);

        strategyData.PostFlopHandChances = new Dictionary<string, double>
        {
            { "Win", win},
            { "Draw", draw},
            { "Loss", loss},
        };

        return strategyData;
    }

    private StrategyData GetPostFlopStrategy(GameData gameData)
    {
        if (gameData.HandCards.Length == 0 || gameData.HandCards[0].cardRank == (int)Rank.None)
        {
            return new StrategyData
            {
                SugestedAction = NoStrategy
            };
        }

        //only simulate win chances when its my turn to act and community cards have changed
        if (gameData.CallAmount > 0 && HaveCommunityCardsChanged(gameData))
        {
            _previousGameData = gameData;
            _lastStrategyData = RunWinChanceSimulation(gameData);

            var sugestion = new StringBuilder();
            foreach (var hand in _lastStrategyData.PostFlopHandChances.OrderByDescending(x => x.Value))
            {
                if (hand.Value == 0)
                {
                    continue;
                }

                sugestion.AppendLine($"{hand.Key,13} {hand.Value,5:F2}%");

                if (hand.Value == 1)
                {
                    // no need to show made hands only top hand
                    break;
                }
            }

            _lastStrategyData.SugestedAction = sugestion.Length == 0
                ? NoStrategy
                : sugestion.ToString();
        }

        return _lastStrategyData;
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
            || gameData.HandCards[0].cardRank != _previousGameData.HandCards[0].cardRank
            || gameData.HandCards[0].cardSuit != _previousGameData.HandCards[0].cardSuit
            || gameData.HandCards[1].cardRank != _previousGameData.HandCards[1].cardRank
            || gameData.HandCards[1].cardSuit != _previousGameData.HandCards[1].cardSuit
            || gameData.HasBeenRaised != _previousGameData.HasBeenRaised)
        {
            int total = call + raise + fold;
            int min = total <= 0 ? 0 : 1;
            _lastPick = new Random().Next(min, total);
            _previousGameData = gameData;
        }

        string onlyAThought = string.Empty;
        if (gameData.CallAmount <= 0)
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

    private bool HaveCommunityCardsChanged(GameData gameData)
    {
        return (_previousGameData.HandCards is null
            || gameData.CommunityCards[0].cardRank != _previousGameData.CommunityCards[0].cardRank
            || gameData.CommunityCards[0].cardSuit != _previousGameData.CommunityCards[0].cardSuit
            || gameData.CommunityCards[1].cardRank != _previousGameData.CommunityCards[1].cardRank
            || gameData.CommunityCards[1].cardSuit != _previousGameData.CommunityCards[1].cardSuit
            || gameData.CommunityCards[2].cardRank != _previousGameData.CommunityCards[2].cardRank
            || gameData.CommunityCards[2].cardSuit != _previousGameData.CommunityCards[2].cardSuit
            || gameData.CommunityCards[3].cardRank != _previousGameData.CommunityCards[3].cardRank
            || gameData.CommunityCards[3].cardSuit != _previousGameData.CommunityCards[3].cardSuit
            || gameData.CommunityCards[4].cardRank != _previousGameData.CommunityCards[4].cardRank
            || gameData.CommunityCards[4].cardSuit != _previousGameData.CommunityCards[4].cardSuit)
    }
}