using Poker.GameReader.Reporters;

namespace Poker.GameReader.Strategies;

public class SmallBlind : BaseStrategy
{
    private static readonly string[] _defaultFoldData =
    [
        "[39.750099999999996]Qd8h",
        "[41.443400000000004]Kd7h",
        "[56.9211]Ad2h",
        "[57.2802]Jd8h",
        "[68.74900000000001]8d7h",
        "[74.40830000000001]Th4h",
        "[80.5389]Kd6h",
        "[82.0736]7d6h",
        "[85.5055]4h3h",
        "[90.35849999999999]9d7h",
        "[94.0635]6h3h",
        "[99.3787]Td7h",
        "[99.97160000000001]Jh2h",
        "[99.9958]Kd5h",
        "[99.9983]6d5h",
        "[99.9996]Qd7h, Kd4h, Kd3h, Kd2h, Qd6h, Qd5h, Qd4h, Qd3h, Qd2h, Jd7h, Jd6h, Jd5h, Jd4h, Jd3h, " +
        "Jd2h, Td6h, Td5h, Td4h, Td3h, Td2h, 9d6h, 9d5h, 9d4h, 9d3h, 9d2h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, " +
        "7d5h, 7d4h, 7d3h, 7d2h, 6d4h, 6d3h, 6d2h, 5d4h, 5d3h, 5d2h, 9h4h, 8h4h, 4d3h, 4d2h, Th3h, 9h3h, " +
        "8h3h, 7h3h, 3d2h, Th2h, 9h2h, 8h2h, 7h2h, 6h2h, 5h2h, 4h2h, 3h2h"
    ];

    private static readonly string[] _defaultRaiseData =
    [
        "[2.46734]9h5h",
        "[3.1227899999999997]8d7h",
        "[5.38818]4h3h",
        "[8.76766]Ad2h",
        "[14.5842]Jh3h",
        "[15.4731]Jd8h",
        "[23.8354]Qh9h",
        "[24.0155]7h4h",
        "[27.744799999999998]Jh9h",
        "[29.404200000000003]Th9h",
        "[30.4042]Ah3h",
        "[34.8597]Ah6h",
        "[37.2794]Kd8h",
        "[39.9401]Jh4h",
        "[42.9236]Kh8h",
        "[43.3378]Kh9h",
        "[43.5922]9h8h",
        "[47.269099999999995]Td8h",
        "[48.1012]Ah4h",
        "[48.499700000000004]Th8h",
        "[48.554700000000004]Ah7h",
        "[48.6633]Qh2h",
        "[48.9715]Ah2h",
        "[50.7621]Jh5h",
        "[52.0951]Ah5h",
        "[52.191500000000005]9d9h",
        "[53.7663]Ad3h",
        "[55.984100000000005]Kh7h",
        "[56.78490000000001]6d6h",
        "[56.994299999999996]8d8h",
        "[56.999100000000006]4d4h",
        "[57.0704]7d7h",
        "[57.9254]KdJh",
        "[58.51650000000001]9d8h",
        "[59.2622]KdTh",
        "[59.6537]Jh8h",
        "[60.76820000000001]Ad6h",
        "[60.7906]5d5h",
        "[60.9336]Ah8h",
        "[62.287099999999995]3d3h",
        "[62.828799999999994]QdJh",
        "[64.1515]Qd9h",
        "[64.95729999999999]6h5h",
        "[65.72099999999999]5h4h",
        "[66.3854]Ad4h",
        "[66.53750000000001]Jd9h",
        "[66.8141]JdTh",
        "[67.121]5h3h",
        "[67.57809999999999]QdTh",
        "[69.1279]AdTh",
        "[69.364]Th6h",
        "[69.5521]KdQh",
        "[70.9846]7h6h",
        "[71.0861]Qh8h",
        "[73.71799999999999]AdJh",
        "[74.651]Kh5h",
        "[74.958]Kh6h",
        "[75.88719999999999]Td9h",
        "[76.84830000000001]8h7h",
        "[78.11789999999999]Th7h",
        "[79.8675]2d2h",
        "[80.8179]6h4h",
        "[81.6672]JdJh",
        "[82.22840000000001]AdKh",
        "[83.20779999999999]7h5h",
        "[84.18639999999999]TdTh",
        "[85.1607]9h6h",
        "[85.5953]KdKh",
        "[85.7127]JhTh",
        "[85.73530000000001]9h7h",
        "[85.7766]QdQh",
        "[86.0109]8h5h",
        "[86.1398]Qh7h",
        "[86.2877]Qh6h",
        "[86.8301]Qh5h",
        "[88.3142]Jh6h",
        "[88.3152]AdQh",
        "[88.6927]KhQh",
        "[88.7272]Ah9h",
        "[90.238]8h6h",
        "[90.3956]Kd9h",
        "[92.44019999999999]Jh7h",
        "[92.9537]Ad8h",
        "[93.4598]Kh4h",
        "[93.8163]Ad9h",
        "[94.6676]Ad5h",
        "[94.7495]QhTh",
        "[95.36489999999999]AhJh",
        "[95.5202]QhJh",
        "[97.6328]AdAh",
        "[98.24680000000001]Ad7h",
        "[99.842]Qh3h",
        "[99.96509999999999]Qh4h",
        "[99.9939]Kh3h",
        "[99.99980000000001]KhTh, AhKh, AhQh, KhJh, AhTh, Kh2h"
    ];

    private static readonly string[] _defaultCallData =
    [
        "[0.621346]Td7h",
        "[1.75324]Ad7h",
        "[2.36717]AdAh",
        "[4.47982]QhJh",
        "[4.63507]AhJh",
        "[5.25051]QhTh",
        "[5.33241]Ad5h",
        "[5.9365000000000006]6h3h",
        "[6.1837]Ad9h",
        "[6.54024]Kh4h",
        "[7.0463499999999994]Ad8h",
        "[7.559829999999999]Jh7h",
        "[9.10632]4h3h",
        "[9.60435]Kd9h",
        "[9.64151]9d7h",
        "[9.76196]8h6h",
        "[11.2728]Ah9h",
        "[11.307300000000001]KhQh",
        "[11.6848]AdQh",
        "[11.6858]Jh6h",
        "[13.169900000000002]Qh5h",
        "[13.712299999999999]Qh6h",
        "[13.8602]Qh7h",
        "[13.989099999999999]8h5h",
        "[14.2234]QdQh",
        "[14.2647]9h7h",
        "[14.2873]JhTh",
        "[14.4047]KdKh",
        "[14.8393]9h6h",
        "[15.8136]TdTh",
        "[16.792199999999998]7h5h",
        "[17.771600000000003]AdKh",
        "[17.9264]7d6h",
        "[18.3328]JdJh",
        "[19.1821]6h4h",
        "[19.461000000000002]Kd6h",
        "[20.1325]2d2h",
        "[21.882099999999998]Th7h",
        "[23.1517]8h7h",
        "[24.1128]Td9h",
        "[25.041999999999998]Kh6h",
        "[25.349]Kh5h",
        "[25.5917]Th4h",
        "[26.282]AdJh",
        "[27.246599999999997]Jd8h",
        "[28.128199999999996]8d7h",
        "[28.913899999999998]Qh8h",
        "[29.015400000000003]7h6h",
        "[30.4479]KdQh",
        "[30.636000000000003]Th6h",
        "[30.872100000000003]AdTh",
        "[32.4219]QdTh",
        "[32.879000000000005]5h3h",
        "[33.185900000000004]JdTh",
        "[33.4625]Jd9h",
        "[33.6146]Ad4h",
        "[34.278999999999996]5h4h",
        "[34.3112]Ad2h",
        "[35.042699999999996]6h5h",
        "[35.8485]Qd9h",
        "[37.1712]QdJh",
        "[37.7129]3d3h",
        "[39.0664]Ah8h",
        "[39.2094]5d5h",
        "[39.231700000000004]Ad6h",
        "[40.3463]Jh8h",
        "[40.7378]KdTh",
        "[41.4835]9d8h",
        "[42.074600000000004]KdJh",
        "[42.9296]7d7h",
        "[43.000899999999994]4d4h",
        "[43.005700000000004]8d8h",
        "[43.2151]6d6h",
        "[44.0159]Kh7h",
        "[46.2337]Ad3h",
        "[47.808499999999995]9d9h",
        "[47.9049]Ah5h",
        "[49.2379]Jh5h",
        "[51.0285]Ah2h",
        "[51.3367]Qh2h",
        "[51.4453]Ah7h",
        "[51.500299999999996]Th8h",
        "[51.8988]Ah4h",
        "[52.5312]Td8h",
        "[56.407799999999995]9h8h",
        "[56.6622]Kh9h",
        "[57.07640000000001]Kh8h",
        "[58.473600000000005]Kd7h",
        "[60.0599]Jh4h",
        "[60.068999999999996]Qd8h",
        "[62.720600000000005]Kd8h",
        "[65.1403]Ah6h",
        "[69.5958]Ah3h",
        "[70.5958]Th9h",
        "[72.2552]Jh9h",
        "[75.9845]7h4h",
        "[76.16460000000001]Qh9h",
        "[85.41579999999999]Jh3h",
        "[97.5327]9h5h",
        "[99.99]Th5h"
    ];

    private static readonly string[] _raisedCallData =
    [
        "[12.2945]8h7h",
        "[15.609799999999998]9h8h",
        "[15.8024]Ah3h",
        "[16.312099999999997]3d3h",
        "[17.0178]Kh9h",
        "[22.1035]AdKh",
        "[23.6827]Ah7h",
        "[24.6775]4d4h",
        "[25.182700000000004]Ah5h",
        "[25.973800000000004]7h6h",
        "[26.8802]Ah4h",
        "[27.161099999999998]KdQh",
        "[28.8196]Th9h",
        "[33.9591]6h5h",
        "[37.0563]5h4h",
        "[41.9842]KhQh",
        "[45.905]KhTh",
        "[46.0345]QdQh",
        "[51.546099999999996]Ah8h",
        "[57.43129999999999]5d5h",
        "[64.8218]QhJh",
        "[66.2079]QhTh",
        "[66.6214]AhQh",
        "[66.6918]6d6h",
        "[66.9126]AdQh",
        "[68.9592]KhJh",
        "[76.2126]JhTh",
        "[77.4135]Ah9h",
        "[81.4657]AhTh",
        "[82.3917]7d7h",
        "[86.0086]AhJh",
        "[87.37049999999999]TdTh",
        "[89.2708]8d8h",
        "[91.5264]JdJh",
        "[92.53479999999999]9d9h",
    ];

    private static readonly string[] _raisedFoldData =
    [
        "[21.9847]6d6h",
        "[27.0354]QhTh",
        "[32.609100000000005]Ah7h",
        "[36.5163]KdQh",
        "[38.209199999999996]5d5h",
        "[39.3166]5h4h",
        "[45.2764]6h5h",
        "[45.540000000000006]Kh9h",
        "[59.8855]Ah2h",
        "[60.8356]7h6h",
        "[63.1305]Kh8h",
        "[63.421099999999996]Th9h",
        "[75.3225]4d4h",
        "[77.37270000000001]Kh5h",
        "[80.7998]8h7h",
        "[81.6314]Ah6h",
        "[83.6879]3d3h",
        "[84.39020000000001]9h8h",
        "[86.77040000000001]Kh7h",
        "[95.2586]Kh4h",
        "[98.6201]Kh6h",
        "[99.7922]AdJh",
        "[99.9997]8h6h",
        "[99.9999]9h7h, 6h4h, AdTh, Ad9h, Ad8h, Ad7h, Ad6h, Ad5h, Ad4h, Ad3h, Ad2h, KdJh, KdTh, " +
        "Kd9h, Kd8h, Kd7h, Kd6h, Kd5h, Kd4h, Kd3h, Kd2h, QdJh, QdTh, Qd9h, Qd8h, Qd7h, Qd6h, Qd5h, " +
        "Qd4h, Qd3h, Qd2h, JdTh, Jd9h, Jd8h, Jd7h, Jd6h, Jd5h, Jd4h, Jd3h, Jd2h, Td9h, Td8h, Td7h, " +
        "Td6h, Td5h, Td4h, Td3h, Td2h, Qh9h, Jh9h, 9d8h, 9d7h, 9d6h, 9d5h, 9d4h, 9d3h, 9d2h, Qh8h, " +
        "Jh8h, Th8h, 8d7h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, Qh7h, Jh7h, Th7h, 7d6h, 7d5h, 7d4h, 7d3h, " +
        "7d2h, Qh6h, Jh6h, Th6h, 9h6h, 6d5h, 6d4h, 6d3h, 6d2h, Qh5h, Jh5h, Th5h, 9h5h, 8h5h, 7h5h, " +
        "5d4h, 5d3h, 5d2h, Qh4h, Jh4h, Th4h, 9h4h, 8h4h, 7h4h, 4d3h, 4d2h, Kh3h, Qh3h, Jh3h, Th3h, " +
        "9h3h, 8h3h, 7h3h, 6h3h, 5h3h, 4h3h, 3d2h, Kh2h, Qh2h, Jh2h, Th2h, 9h2h, 8h2h, 7h2h, 6h2h, " +
        "5h2h, 4h2h, 3h2h, 2d2h"
    ];

    private static readonly string[] _raisedRaiseData =
    [
        "[1.3799000000000001]Kh6h",
        "[4.35952]5d5h",
        "[4.7413799999999995]Kh4h",
        "[6.75661]QhTh",
        "[6.90571]8h7h",
        "[7.46522]9d9h",
        "[7.75933]Th9h",
        "[8.473600000000001]JdJh",
        "[10.7292]8d8h",
        "[11.323500000000001]6d6h",
        "[12.629499999999998]TdTh",
        "[13.1907]7h6h",
        "[13.2296]Kh7h",
        "[13.9914]AhJh",
        "[17.6083]7d7h",
        "[18.3682]Ah6h",
        "[18.5343]AhTh",
        "[20.764499999999998]6h5h",
        "[22.586100000000002]Ah9h",
        "[22.6273]Kh5h",
        "[23.627100000000002]5h4h",
        "[23.787]JhTh",
        "[31.0408]KhJh",
        "[33.0874]AdQh",
        "[33.378600000000006]AhQh",
        "[35.1779]QhJh",
        "[36.322500000000005]KdQh",
        "[36.8695]Kh8h",
        "[37.4422]Kh9h",
        "[40.1132]Ah2h",
        "[43.708200000000005]Ah7h",
        "[48.4335]Ah8h",
        "[53.9655]QdQh",
        "[54.095000000000006]KhTh",
        "[58.0158]KhQh",
        "[73.1198]Ah4h",
        "[74.8173]Ah5h",
        "[77.8965]AdKh",
        "[84.1976]Ah3h",
        "[99.9976]AdAh, AhKh, KdKh"
    ];

    private Dictionary<(int, int, Suited), double>? _defaultFoldRange;
    private Dictionary<(int, int, Suited), double>? _defaultRaiseRange;
    private Dictionary<(int, int, Suited), double>? _defaultCallRange;
    private Dictionary<(int, int, Suited), double>? _raisedCallRange;
    private Dictionary<(int, int, Suited), double>? _raisedFoldRange;
    private Dictionary<(int, int, Suited), double>? _raisedRaiseRange;
    private Dictionary<(int, int, Suited), double> DefaultCall
    {
        get
        {
            _defaultCallRange ??= BuildRange(_defaultCallData);

            return _defaultCallRange;
        }
    }

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
