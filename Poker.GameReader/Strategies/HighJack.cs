using Poker.GameReader.Reporters;

namespace Poker.GameReader.Strategies;

public class HighJack : BaseStrategy
{
    private static readonly string[] _defaultFoldData =
    [
        "[3.48425]Qh8h",
        "[8.0957]Th8h",
        "[29.5514]8h7h",
        "[38.0334]6h5h",
        "[46.2829]7h6h",
        "[53.148300000000006]Kh4h",
        "[66.1045]5h4h",
        "[70.7375]9h7h",
        "[75.9692]Jh8h",
        "[99.99980000000001]8h6h, Qh7h, Jh7h, Th7h, Qh6h, Jh6h, Th6h, 9h6h, Qh5h, Jh5h, Th5h, 9h5h, 8h5h, 7h5h, Qh4h, " +
        "Jh4h, Th4h, 9h4h, 8h4h, 7h4h, 6h4h, Kh3h, Qh3h, Jh3h, Th3h, 9h3h, 8h3h, 7h3h, 6h3h, 5h3h, 4h3h, Kh2h, Qh2h, " +
        "Jh2h, Th2h, 9h2h, 8h2h, 7h2h, 6h2h, 5h2h, 4h2h, 3h2h",

        "[1.0236]KdTh",
        "[19.1942]Ad9h",
        "[30.5883]4d4h",
        "[65.3219]QdTh",
        "[73.9003]3d3h",
        "[81.4732]JdTh",
        "[96.9262]2d2h, Ad8h, Ad7h, Ad6h, Ad5h, Ad4h, Ad3h, Ad2h, Kd9h, Kd8h, Kd7h, Kd6h, Kd5h, Kd4h, Kd3h, Kd2h, Qd9h, " +
        "Qd8h, Qd7h, Qd6h, Qd5h, Qd4h, Qd3h, Qd2h, Jd9h, Jd8h, Jd7h, Jd6h, Jd5h, Jd4h, Jd3h, Jd2h, Td9h, Td8h, Td7h, Td6h, " +
        "Td5h, Td4h, Td3h, Td2h, 9d8h, 9d7h, 9d6h, 9d5h, 9d4h, 9d3h, 9d2h, 8d7h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, 7d6h, 7d5h, " +
        "7d4h, 7d3h, 7d2h, 6d5h, 6d4h, 6d3h, 6d2h, 5d4h, 5d3h, 5d2h, 4d3h, 4d2h, 3d2h"
    ];
    private static readonly string[] _defaultRaiseData =
    [
        "[24.0308]Jh8h",
        "[29.262500000000003]9h7h",
        "[33.8955]5h4h",
        "[46.8517]Kh4h",
        "[53.717099999999995]7h6h",
        "[61.96660000000001]6h5h",
        "[70.4486]8h7h",
        "[91.9043]Th8h",
        "[96.51570000000001]Qh8h",
        "[99.9997]9h8h, Kh5h, AhKh, AhQh, KhQh, AhJh, KhJh, QhJh, AhTh, KhTh, QhTh, JhTh, Ah9h, Kh9h, Qh9h, Jh9h, Th9h, " +
        "Ah8h, Kh8h, Ah7h, Kh7h, Ah6h, Kh6h, Ah5h, Ah4h, Ah3h, Ah2h",

        "[3.0737799999999997]2d2h",
        "[18.526799999999998]JdTh",
        "[26.0997]3d3h",
        "[34.6781]QdTh",
        "[69.4117]4d4h",
        "[80.8058]Ad9h",
        "[98.9764]KdTh",
        "[99.9999]QdJh, AdAh, AdKh, AdQh, AdJh, AdTh, KdKh, KdQh, KdJh, QdQh, JdJh, TdTh, 9d9h, 8d8h, 7d7h, 6d6h, 5d5h"
    ];
    private static readonly string[] _raisedCallData =
    [
        "[0.98197]8h7h",
        "[1.86942]Ah9h",
        "[2.9872300000000003]9h8h",
        "[3.64164]Th9h",
        "[4.71787]Ah8h",
        "[5.30089]Ah3h",
        "[7.40053]Ah4h",
        "[7.78089]7h6h",
        "[9.99422]QhTh",
        "[10.335700000000001]6h5h",
        "[11.3743]QhJh",
        "[11.626100000000001]JhTh",
        "[11.9778]5h4h",
        "[14.434]KhTh",
        "[15.201300000000002]Ah5h",
        "[16.660800000000002]KhJh",
        "[33.1255]AhTh",
        "[37.7597]AhQh",
        "[49.2061]KhQh",
        "[50.30329999999999]AhJh",

        "[0.708712]4d4h",
        "[4.89888]5d5h",
        "[7.125439999999999]6d6h",
        "[9.97663]AdQh",
        "[11.3413]AdKh",
        "[14.7638]7d7h",
        "[19.2031]8d8h",
        "[23.7458]QdQh",
        "[31.0701]9d9h",
        "[44.1709]TdTh",
        "[48.857]JdJh",
    ];
    private static readonly string[] _raisedFoldData =
                [
        "[10.6518]Ah3h",
        "[41.4882]Ah9h",
        "[42.7443]Ah8h",
        "[53.28810000000001]Ah7h",
        "[53.563700000000004]Kh5h",
        "[69.281]6h5h",
        "[73.2478]5h4h",
        "[74.94969999999999]Kh7h",
        "[75.5332]Kh8h",
        "[76.17699999999999]Kh9h",
        "[79.55799999999999]QhJh",
        "[79.6709]7h6h",
        "[84.8643]Ah6h",
        "[85.3077]JhTh",
        "[90.00580000000001]QhTh",
        "[92.4687]Kh4h",
        "[92.5092]Th9h",
        "[95.4242]8h7h",
        "[97.0128]9h8h",
        "[98.8116]Kh6h",
        "[99.9195]Ah2h",
        "[99.99980000000001]6h4h, Qh9h, Jh9h, Qh8h, Jh8h, Th8h, Qh7h, Jh7h, Th7h, 9h7h, Qh6h, Jh6h, Th6h, 9h6h, " +
        "8h6h, Qh5h, Jh5h, Th5h, 9h5h, 8h5h, 7h5h, Qh4h, Jh4h, Th4h, 9h4h, 8h4h, 7h4h, Kh3h, Qh3h, Jh3h, Th3h, " +
        "9h3h, 8h3h, 7h3h, 6h3h, 5h3h, 4h3h, Kh2h, Qh2h, Jh2h, Th2h, 9h2h, 8h2h, 7h2h, 6h2h, 5h2h, 4h2h, 3h2h",

        "[7.34042]AdQh",
        "[39.8357]KdQh",
        "[44.0238]9d9h",
        "[61.4836]8d8h",
        "[72.8475]7d7h",
        "[81.5571]6d6h",
        "[92.3026]5d5h",
        "[99.2913]4d4h",
        "[99.9992]KdJh, AdJh, AdTh, Ad9h, Ad8h, Ad7h, Ad6h, Ad5h, Ad4h, Ad3h, Ad2h, KdTh, Kd9h, Kd8h, Kd7h, Kd6h, " +
        "Kd5h, Kd4h, Kd3h, Kd2h, QdJh, QdTh, Qd9h, Qd8h, Qd7h, Qd6h, Qd5h, Qd4h, Qd3h, Qd2h, JdTh, Jd9h, Jd8h, Jd7h, " +
        "Jd6h, Jd5h, Jd4h, Jd3h, Jd2h, Td9h, Td8h, Td7h, Td6h, Td5h, Td4h, Td3h, Td2h, 9d8h, 9d7h, 9d6h, 9d5h, 9d4h, " +
        "9d3h, 9d2h, 8d7h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, 7d6h, 7d5h, 7d4h, 7d3h, 7d2h, 6d5h, 6d4h, 6d3h, 6d2h, 5d4h, " +
        "5d3h, 5d2h, 4d3h, 4d2h, 3d3h, 3d2h, 2d2h"
    ];

    private static readonly string[] _raisedRaiseData =
    [
        "[1.1883700000000001]Kh6h",
        "[3.06617]JhTh",
        "[3.5938]8h7h",
        "[3.84912]Th9h",
        "[7.531300000000001]Kh4h",
        "[9.067699999999999]QhJh",
        "[12.548200000000001]7h6h",
        "[14.774499999999998]5h4h",
        "[15.1357]Ah6h",
        "[20.3833]6h5h",
        "[23.823]Kh9h",
        "[24.4668]Kh8h",
        "[25.050299999999996]Kh7h",
        "[46.4363]Kh5h",
        "[46.706599999999995]Ah7h",
        "[49.6967]AhJh",
        "[50.7939]KhQh",
        "[52.537800000000004]Ah8h",
        "[56.6424]Ah9h",
        "[62.240300000000005]AhQh",
        "[66.8745]AhTh",
        "[83.3392]KhJh",
        "[84.0473]Ah3h",
        "[84.79870000000001]Ah5h",
        "[85.566]KhTh",
        "[92.5995]Ah4h",
        "[99.9999]AhKh",

        "[2.79851]5d5h",
        "[11.317499999999999]6d6h",
        "[12.3887]7d7h",
        "[19.313299999999998]8d8h",
        "[24.906200000000002]9d9h",
        "[51.14300000000001]JdJh",
        "[55.8291]TdTh",
        "[60.164300000000004]KdQh",
        "[76.25420000000001]QdQh",
        "[82.6829]AdQh",
        "[88.6587]AdKh",
        "[99.9888]KdKh, AdAh"
    ];
    private Dictionary<(int, int, Suited), double>? _defaultFoldRange;
    private Dictionary<(int, int, Suited), double>? _defaultRaiseRange;
    private Dictionary<(int, int, Suited), double>? _raisedCallRange;
    private Dictionary<(int, int, Suited), double>? _raisedFoldRange;
    private Dictionary<(int, int, Suited), double>? _raisedRaiseRange;
    private Dictionary<(int, int, Suited), double> DefaultCall { get => []; }
    private Dictionary<(int, int, Suited), double> DefaultFold
    {
        get
        {
            _defaultFoldRange ??= BuildRange(_defaultFoldData);

            return _defaultFoldRange;
        }
    }

    private Dictionary<(int, int, Suited), double> DefaultRaise
    {
        get
        {
            _defaultRaiseRange ??= BuildRange(_defaultRaiseData);

            return _defaultRaiseRange;
        }
    }
    private Dictionary<(int, int, Suited), double> RaisedCall
    {
        get
        {
            _raisedCallRange ??= BuildRange(_raisedCallData);

            return _raisedCallRange;
        }
    }
    private Dictionary<(int, int, Suited), double> RaisedFold
    {
        get
        {
            _raisedFoldRange ??= BuildRange(_raisedFoldData);

            return _raisedFoldRange;
        }
    }

    private Dictionary<(int, int, Suited), double> RaisedRaise
    {
        get
        {
            _raisedRaiseRange ??= BuildRange(_raisedRaiseData);

            return _raisedRaiseRange;
        }
    }

    public override StrategySolution Solve(GameData gameData)
    {
        Suited suited = GetSuitedState(gameData);
        (int, int, Suited) hand = (gameData.HandCards[0].cardRank, gameData.HandCards[1].cardRank, suited);

        double raise;
        double fold;
        double call;

        if (gameData.HasBeenRaised)
        {
            RaisedRaise.TryGetValue(hand, out raise);
            RaisedFold.TryGetValue(hand, out fold);
            RaisedCall.TryGetValue(hand, out call);
        }
        else
        {
            DefaultRaise.TryGetValue(hand, out raise);
            DefaultFold.TryGetValue(hand, out fold);
            DefaultCall.TryGetValue(hand, out call);
        }

        return new StrategySolution { Call = call, Fold = fold, Raise = raise };
    }
}