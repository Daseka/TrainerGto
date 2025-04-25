using Poker.Common;
using Poker.GameReader.Strategies;
using Poker.GtoBuilder;
using Poker.GtoBuilder.GameSims;
using System.Text;

namespace Poker.GameReader.Reporters;

public class StrategyReporter : IStrategyReporter
{
    private const double CrushEmFactor = 3;
    private const int DefaultPlayPercentage = 50;
    private const string NoStrategy = "No Strategy";
    private const double PokeEmFactor = 1.5;
    private static Dictionary<ulong, double> _handEquityIndex = StartingHand
        .ReadStartingHands()
        .Select(x => (HandToKey(x.Item1), x.Item2))
        .ToDictionary((key) => key.Item1, (value) => value.Item2);
    private readonly IHandSimulator _handSimulator;
    private readonly Dictionary<Position, int> _positionalPreflopRaises = new()
    {
        {Position.None, 0 },
        {Position.Button, 2 },
        {Position.CutOff, 3 },
        {Position.HighJack, 4 },
        {Position.UnderTheGun, 5 },
        {Position.BigBlind, 12 },
        {Position.SmallBlind, 3 },
    };
    private int _lastPick;
    private StrategyData _lastStrategyData;
    private GameData _previousGameData;

    public StrategyReporter(IHandSimulator handSimulator)
    {
        _handSimulator = handSimulator;
    }

    public async Task<StrategyData> GetStrategy(GameData gameData)
    {
        StrategyData strategyData = IsAfterFlop(gameData)
            ? await GetPostFlopStrategy(gameData)
            : GetPreFlopStrategy(gameData);

        return strategyData;
    }

    private static double GetHandEquity(GameData gameData)
    {
        if (gameData.HandCards == null || gameData.HandCards.Any(x => x.cardRank == 0 || x.cardRank == 0))
        {
            return 0;
        }

        ulong key = HandToKey([.. gameData.HandCards.Select(x => ((Rank)x.cardRank, (Suit)x.cardSuit))]);

        return key == 0 ? 0 : _handEquityIndex[key];
    }

    private static ulong HandToKey((Rank, Suit)[] hand)
    {
        ulong card1 = (0x1UL << (int)hand[0].Item1) << (13 * ((int)hand[0].Item2 - 1));
        ulong card2 = (0x1UL << (int)hand[1].Item1) << (13 * ((int)hand[1].Item2 - 1));

        return card1 + card2;
    }

    private static bool IsAfterFlop(GameData gameData)
    {
        return gameData.CommunityCards[0] != (CardRank.None, CardSuit.None)
            || gameData.CommunityCards[1] != (CardRank.None, CardSuit.None)
            || gameData.CommunityCards[2] != (CardRank.None, CardSuit.None);
    }

    private static async Task<StrategyData> RunWinChanceSimulation(GameData gameData, IHandSimulator handSimulator)
    {
        var handCards = gameData.HandCards.Select(x => ((Rank)x.cardRank, (Suit)x.cardSuit));
        var villains = gameData.VillainsPlaying.Where(x => x).Select(x => DefaultPlayPercentage);
        var communityCards = gameData.CommunityCards
            .Where(x => x.cardSuit != (int)Suit.None)
            .Select(x => ((Rank)x.cardRank, (Suit)x.cardSuit));

        //var (win, draw, loss) = await handSimulator.SimulateWinChance([.. handCards], [.. villains], [.. communityCards]);
        var (win, draw, loss) = await handSimulator.SimulateWinChanceOld([.. handCards], [.. villains], [.. communityCards]);

        return new StrategyData
        {
            PostFlopHandChances = new Dictionary<string, double>
            {
                { "Win", win},
                { "Draw", draw},
                { "Loss", loss},
            },
            HandEquity = GetHandEquity(gameData),
        };
    }

    private async Task<StrategyData> GetPostFlopStrategy(GameData gameData)
    {
        if (gameData.HandCards.Length == 0 || gameData.HandCards[0].cardRank == (int)Rank.None)
        {
            return new StrategyData
            {
                SugestedAction = NoStrategy
            };
        }

        //only simulate win chances when community cards have changed
        if (HaveCommunityCardsChanged(gameData))
        {
            _previousGameData = gameData;
            _lastStrategyData = await RunWinChanceSimulation(gameData, _handSimulator);

            var sugestion = new StringBuilder();
            foreach (var hand in _lastStrategyData.PostFlopHandChances.OrderByDescending(x => x.Value))
            {
                sugestion.AppendLine($"{hand.Key,6} {hand.Value,5:F2}%");
            }

            _lastStrategyData.SugestedAction = sugestion.Length == 0
                ? NoStrategy
                : sugestion.ToString();
        }

        if (_lastStrategyData.PostFlopHandChances?.TryGetValue("Win", out double value) == true && value > 0)
        {
            double winChance = value;
            double lossChance = 100 - winChance;
            _lastStrategyData.MaxBet = (winChance / 100 * gameData.PotTotal);
            _lastStrategyData.MinBet = Math.Max(0, _lastStrategyData.MaxBet - (lossChance / 100 * _lastStrategyData.MaxBet));
            _lastStrategyData.EvBet = (winChance / 100 * gameData.PotTotal) - (gameData.Bets.Max() * lossChance / 100);
        }
        else
        {
            _lastStrategyData.MaxBet = 0;
            _lastStrategyData.MinBet = 0;
            _lastStrategyData.EvBet = 0;
        }

        //clear cached preflop data since nolonger relevant
        _lastPick = 0;

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
        double maxBetAmount = 0;
        double minBetAmount = 0;
        double callAmount = gameData.Bets.Max();
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
            maxBetAmount = Math.Max(gameData.Bets.Max(), gameData.BigBlind);
            minBetAmount = maxBetAmount;
        }
        else if (_lastPick <= call + raise)
        {
            suggestion = $"{onlyAThought} RAISE (roll {_lastPick})";
            maxBetAmount = Math.Max(0, (CrushEmFactor * (callAmount - gameData.BigBlind))
                + _positionalPreflopRaises[gameData.Position] * gameData.BigBlind);
            minBetAmount = Math.Min(maxBetAmount, gameData.BigBlind * 2 + gameData.CallAmount);
        }
        else
        {
            suggestion = $"{onlyAThought} FOLD (roll {_lastPick})";
            maxBetAmount = Math.Max(0, (PokeEmFactor * (callAmount - gameData.BigBlind))
                + _positionalPreflopRaises[gameData.Position] * gameData.BigBlind);
            minBetAmount = -1;
        }

        StrategyData strategyData = new()
        {
            Call = call,
            Raise = raise,
            Fold = fold,
            SugestedAction = suggestion,
            MaxBet = maxBetAmount,
            MinBet = minBetAmount,
            EvBet = minBetAmount,
            HandEquity = GetHandEquity(gameData),
        };

        //clear cached postflop data since nolonger relevant
        _lastStrategyData = new StrategyData();

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
            || gameData.CommunityCards[4].cardSuit != _previousGameData.CommunityCards[4].cardSuit);
    }
}