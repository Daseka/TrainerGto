using Poker.GameReader.Reporters;
using static Poker.GameReader.Reporters.CardRank;

namespace Poker.GameReader.Strategies;

public class UnderTheGun : BaseStrategy
{
    private static readonly string[] _foldData =
    [
        "[0.535077]Kh7h",
        "[45.9038]Kh5h",
        "[46.340599999999995]Jh9h",
        "[53.0831]9h8h",
        "[68.2737]Kh6h",
        "[69.45989999999999]Th8h",
        "[73.6506]6h5h",
        "[75.1101]5h4h",
        "[75.4432]7h6h",
        "[77.5876]8h7h",
        "[99.9999]9h7h, Qh8h, Jh8h, Qh7h, Jh7h, Th7h, Qh6h, Jh6h, Th6h, 9h6h, 8h6h, Qh5h, Jh5h, Th5h, 9h5h, 8h5h, 7h5h, Kh4h, Qh4h, Jh4h, " +
        "Th4h, 9h4h, 8h4h, 7h4h, 6h4h, Kh3h, Qh3h, Jh3h, Th3h, 9h3h, 8h3h, 7h3h, 6h3h, 5h3h, 4h3h, Kh2h, Qh2h, Jh2h, Th2h, 9h2h, 8h2h, 7h2h, " +
        "6h2h, 5h2h, 4h2h, 3h2h",
        "[2.5359]KdJh",
        "[14.902099999999999]5d5h",
        "[68.7368]KdTh",
        "[71.0323]QdJh",
        "[80.5857]4d4h",
        "[91.0116]3d3h",
        "[98.268]QdTh, Ad9h, Ad8h, Ad7h, Ad6h, Ad5h, Ad4h, Ad3h, Ad2h, Kd9h, Kd8h, Kd7h, Kd6h, Kd5h, Kd4h, Kd3h, Kd2h, Qd9h, Qd8h, " +
        "Qd7h, Qd6h, Qd5h, Qd4h, Qd3h, Qd2h, JdTh, Jd9h, Jd8h, Jd7h, Jd6h, Jd5h, Jd4h, Jd3h, Jd2h, Td9h, Td8h, Td7h, Td6h, Td5h, Td4h, " +
        "Td3h, Td2h, 9d8h, 9d7h, 9d6h, 9d5h, 9d4h, 9d3h, 9d2h, 8d7h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, 7d6h, 7d5h, 7d4h, 7d3h, 7d2h, 6d5h, " +
        "6d4h, 6d3h, 6d2h, 5d4h, 5d3h, 5d2h, 4d3h, 4d2h, 3d2h, 2d2h"
    ];

    private static readonly string[] _raiseData =
    [
        "[22.412399999999998]8h7h",
        "[24.556800000000003]7h6h",
        "[24.8899]5h4h",
        "[26.3494]6h5h",
        "[30.5401]Th8h",
        "[31.726300000000002]Kh6h",
        "[46.916999999999994]9h8h",
        "[53.659400000000005]Jh9h",
        "[54.0962]Kh5h",
        "[99.4649]Kh7h",
        "[99.9774]Qh9h",
        "[99.9995]Kh8h, AhKh, AhQh, KhQh, AhJh, KhJh, QhJh, AhTh, KhTh, QhTh, JhTh, Ah9h, Kh9h, Th9h, " +
        "Ah8h, Ah7h, Ah6h, Ah5h, Ah4h, Ah3h, Ah2h",
        "[1.7319999999999998]QdTh",
        "[8.98844]3d3h",
        "[19.4143]4d4h",
        "[28.9677]QdJh",
        "[31.2632]KdTh",
        "[85.09790000000001]5d5h",
        "[97.4641]KdJh",
        "[99.9999]AdTh, AdAh, AdKh, AdQh, AdJh, KdKh, KdQh, QdQh, JdJh, TdTh, 9d9h, 8d8h, 7d7h, 6d6h",
    ];

    private Dictionary<(int, int, Suited), double>? _fold;
    private Dictionary<(int, int, Suited), double>? _raise;

    private Dictionary<(int, int, Suited), double> Call
    {
        get => [];
    }

    private Dictionary<(int, int, Suited), double> Fold
    {
        get
        {
            _fold ??= BuildRange(_foldData);

            return _fold;
        }
    }

    private Dictionary<(int, int, Suited), double> Raise
    {
        get
        {
            _raise ??= BuildRange(_raiseData);

            return _raise;
        }
    }

    public override StrategySolution Solve(GameData gameData)
    {
        Suited suited = GetSuitedState(gameData);
        (int, int, Suited) result = (gameData.HandCards[0].cardRank, gameData.HandCards[1].cardRank, suited);

        Raise.TryGetValue(result, out double raise);
        Call.TryGetValue(result, out double call);
        Fold.TryGetValue(result, out double fold);

        return new StrategySolution { Call = call, Fold = fold, Raise = raise };
    }
}