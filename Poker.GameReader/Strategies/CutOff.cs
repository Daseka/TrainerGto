using Poker.GameReader.Reporters;

namespace Poker.GameReader.Strategies;

public class CutOff : BaseStrategy
{
    private static readonly string[] _defaultFoldData =
    [
        "[4.752549999999999]Qh6h",
        "[20.9069]5h4h",
        "[28.7122]Jh7h",
        "[40.1991]8h6h",
        "[49.967600000000004]Ad5h",
        "[64.6349]Kd9h",
        "[65.8569]Qh5h",
        "[69.1112]Th7h",
        "[73.6629]7h5h",
        "[79.1862]Kh2h",
        "[82.8661]2d2h",
        "[85.0819]Ad7h",
        "[97.20309999999999]Td9h",
        "[99.9999]9h6h, Ad6h, Ad4h, Ad3h, Ad2h, Kd8h, Kd7h, Kd6h, Kd5h, Kd4h, Kd3h, Kd2h, Qd9h, Qd8h, Qd7h, Qd6h, Qd5h, " +
        "Qd4h, Qd3h, Qd2h, Jd9h, Jd8h, Jd7h, Jd6h, Jd5h, Jd4h, Jd3h, Jd2h, Td8h, Td7h, Td6h, Td5h, Td4h, Td3h, Td2h, 9d8h, " +
        "9d7h, 9d6h, 9d5h, 9d4h, 9d3h, 9d2h, 8d7h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, 7d6h, 7d5h, 7d4h, 7d3h, 7d2h, Jh6h, Th6h, " +
        "6d5h, 6d4h, 6d3h, 6d2h, Jh5h, Th5h, 9h5h, 8h5h, 5d4h, 5d3h, 5d2h, Qh4h, Jh4h, Th4h, 9h4h, 8h4h, 7h4h, 6h4h, 4d3h, " +
        "4d2h, Qh3h, Jh3h, Th3h, 9h3h, 8h3h, 7h3h, 6h3h, 5h3h, 4h3h, 3d2h, Qh2h, Jh2h, Th2h, 9h2h, 8h2h, 7h2h, 6h2h, 5h2h, " +
        "4h2h, 3h2h"
    ];

    private static readonly string[] _defaultRaiseData =
    [
        "[2.7969]Td9h",
        "[14.9181]Ad7h",
        "[17.1339]2d2h",
        "[20.8138]Kh2h",
        "[26.337100000000003]7h5h",
        "[30.8888]Th7h",
        "[34.1431]Qh5h",
        "[35.3651]Kd9h",
        "[50.032399999999996]Ad5h",
        "[59.8009]8h6h",
        "[71.2878]Jh7h",
        "[79.0931]5h4h",
        "[95.2475]Qh6h",
        "[99.8993]Qh7h",
        "[99.992]Ad8h",
        "[99.99749999999999]3d3h",
        "[99.99900000000001]6h5h, AdAh, AdKh, AdQh, AdJh, AdTh, Ad9h, AhKh, KdKh, KdQh, KdJh, KdTh, AhQh, KhQh, QdQh, " +
        "QdJh, QdTh, AhJh, KhJh, QhJh, JdJh, JdTh, AhTh, KhTh, QhTh, JhTh, TdTh, Ah9h, Kh9h, Qh9h, Jh9h, Th9h, 9d9h, " +
        "Ah8h, Kh8h, Qh8h, Jh8h, Th8h, 9h8h, 8d8h, Ah7h, Kh7h, 9h7h, 8h7h, 7d7h, Ah6h, Kh6h, 7h6h, 6d6h, Ah5h, Kh5h, " +
        "5d5h, Ah4h, Kh4h, 4d4h, Ah3h, Kh3h, Ah2h"
    ];

    private static readonly string[] _raisedCallData =
    [
        "[0.931955]3d3h",
        "[3.43453]8h7h",
        "[3.61389]Kh9h",
        "[3.6446600000000005]9h8h",
        "[4.41936]4d4h",
        "[4.50114]Ah7h",
        "[8.57361]Th9h",
        "[10.8625]5d5h",
        "[10.907]AdKh",
        "[12.9185]Ah8h",
        "[13.631699999999999]Ah4h",
        "[13.713700000000001]Ah3h",
        "[15.918399999999998]7h6h",
        "[17.4638]6h5h",
        "[17.5333]Ah9h",
        "[17.7886]6d6h",
        "[20.7436]5h4h",
        "[24.4245]KhTh",
        "[27.010299999999997]Ah5h",
        "[27.8437]QdQh",
        "[28.1245]QhJh",
        "[28.6899]JhTh",
        "[28.985300000000002]AdQh",
        "[29.9106]QhTh",
        "[31.2947]7d7h",
        "[37.1235]KhJh",
        "[41.4193]AhQh",
        "[44.5062]8d8h",
        "[50.6011]AhTh",
        "[53.68730000000001]TdTh",
        "[63.58859999999999]9d9h",
        "[63.83219999999999]AhJh",
        "[68.2328]JdJh",
        "[77.336]KhQh",
    ];

    private static readonly string[] _raisedFoldData =
                [
        "[8.6792]QhJh",
        "[22.154]8d8h",
        "[26.672800000000002]KdQh",
        "[32.6139]Ah9h",
        "[35.674299999999995]Kh9h",
        "[36.7232]Ah7h",
        "[42.057]Kh5h",
        "[52.6485]7d7h",
        "[57.7649]QhTh",
        "[58.2342]JhTh",
        "[62.751400000000004]5h4h",
        "[65.2254]6h5h",
        "[69.22359999999999]6d6h",
        "[70.2509]7h6h",
        "[76.5486]Ah2h",
        "[79.05879999999999]Ah6h",
        "[85.6737]Th9h",
        "[85.8386]5d5h",
        "[92.1743]8h7h",
        "[92.8969]Kh7h",
        "[95.46249999999999]Kh4h",
        "[95.5806]4d4h",
        "[96.3553]9h8h",
        "[96.6723]Kh8h",
        "[97.491]Kh6h",
        "[99.068]3d3h",
        "[99.6939]KdJh, AdJh, AdTh, Ad9h, Ad8h, Ad7h, Ad6h, Ad5h, Ad4h, Ad3h, Ad2h, KdTh, Kd9h, Kd8h, Kd7h, Kd6h, Kd5h, " +
        "Kd4h, Kd3h, Kd2h, QdJh, QdTh, Qd9h, Qd8h, Qd7h, Qd6h, Qd5h, Qd4h, Qd3h, Qd2h, JdTh, Jd9h, Jd8h, Jd7h, Jd6h, Jd5h, " +
        "Jd4h, Jd3h, Jd2h, Td9h, Td8h, Td7h, Td6h, Td5h, Td4h, Td3h, Td2h, Qh9h, Jh9h, 9d8h, 9d7h, 9d6h, 9d5h, 9d4h, 9d3h, " +
        "9d2h, Qh8h, Jh8h, Th8h, 8d7h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, Qh7h, Jh7h, Th7h, 9h7h, 7d6h, 7d5h, 7d4h, 7d3h, 7d2h, " +
        "Qh6h, Jh6h, Th6h, 9h6h, 8h6h, 6d5h, 6d4h, 6d3h, 6d2h, Qh5h, Jh5h, Th5h, 9h5h, 8h5h, 7h5h, 5d4h, 5d3h, 5d2h, Qh4h, " +
        "Jh4h, Th4h, 9h4h, 8h4h, 7h4h, 6h4h, 4d3h, 4d2h, Kh3h, Qh3h, Jh3h, Th3h, 9h3h, 8h3h, 7h3h, 6h3h, 5h3h, 4h3h, 3d2h, " +
        "Kh2h, Qh2h, Jh2h, Th2h, 9h2h, 8h2h, 7h2h, 6h2h, 5h2h, 4h2h, 3h2h, 2d2h"
    ];

    private static readonly string[] _raisedRaiseData =
    [
        "[2.50901]Kh6h",
        "[3.2989200000000003]5d5h",
        "[3.3276600000000003]Kh8h",
        "[4.39119]8h7h",
        "[4.53751]Kh4h",
        "[5.75269]Th9h",
        "[7.103149999999999]Kh7h",
        "[12.324499999999999]QhTh",
        "[12.9878]6d6h",
        "[13.075899999999999]JhTh",
        "[13.830700000000002]7h6h",
        "[16.0567]7d7h",
        "[16.505]5h4h",
        "[17.3108]6h5h",
        "[20.9412]Ah6h",
        "[22.664]KhQh",
        "[23.4509]Ah2h",
        "[31.767200000000003]JdJh",
        "[33.339800000000004]8d8h",
        "[36.1678]AhJh",
        "[36.4114]9d9h",
        "[46.3127]TdTh",
        "[49.3989]AhTh",
        "[49.8528]Ah9h",
        "[57.9431]Kh5h",
        "[58.58069999999999]AhQh",
        "[58.7756]Ah7h",
        "[60.711800000000004]Kh9h",
        "[62.8765]KhJh",
        "[63.19630000000001]QhJh",
        "[71.01469999999999]AdQh",
        "[72.1563]QdQh",
        "[72.9897]Ah5h",
        "[73.1847]KdQh",
        "[75.57549999999999]KhTh",
        "[86.2799]Ah3h",
        "[86.3683]Ah4h",
        "[86.9541]Ah8h",
        "[99.993]AdKh, AdAh, AhKh, KdKh"
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