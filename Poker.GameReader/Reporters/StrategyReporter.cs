using Poker.GameReader.Hands;
using Poker.GameReader.Strategies;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.Text;

namespace Poker.GameReader.Reporters;

public class StrategyReporter
{
    private int _lastPick;
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

    private StrategyData GetPostFlopStrategy(GameData gameData)
    {
        var strategyData = new StrategyData();

        UpdatePostFlopHandChances(gameData, strategyData);

        var sugestion = new StringBuilder();
        foreach (var hand in strategyData.PostFlopHandChances)
        {
            if (hand.Value == 0 )
            {
                continue;
            }

            sugestion.AppendLine($"{hand.Key,8} {hand.Value * 100.0,5:F2}%");

            if (hand.Value == 1)
            {
                // no need to show made hands only top hand
                break;
            }
        }

        strategyData.SugestedAction = sugestion.Length == 0 
            ? "No Strategy" 
            : sugestion.ToString();

        return strategyData;
    }

    private double CalculatePotOdds(GameData gameData)
    {
        double odds = gameData.CallAmount / (gameData.CallAmount + gameData.PotTotal);

        return Math.Round(odds, 2);
    }

    private static void UpdatePostFlopHandChances(GameData gameData, StrategyData strategyData)
    {
        strategyData.PostFlopHandChances[Hand.FourOfAKind] = FourOfAKind.CalculateChance(gameData);
        strategyData.PostFlopHandChances[Hand.FullHouse] = FullHouse.CalculateChance(gameData);
        strategyData.PostFlopHandChances[Hand.Flush] = Flush.CalculateChance(gameData);
        strategyData.PostFlopHandChances[Hand.Straight] = Straight.CalculateChance(gameData);
        strategyData.PostFlopHandChances[Hand.ThreeOfAKind] = ThreeOfAKind.CalculateChance(gameData);
        strategyData.PostFlopHandChances[Hand.TwoPair] = TwoPair.CalculateChance(gameData);
        strategyData.PostFlopHandChances[Hand.OnePair] = TwoOfAKind.CalculateChance(gameData);
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