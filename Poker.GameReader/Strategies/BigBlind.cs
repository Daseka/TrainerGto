using Poker.GameReader.Reporters;

namespace Poker.GameReader.Strategies;

public class BigBlind : BaseStrategy
{
    private static readonly string[] _defaultFoldData =
    [
        //if everyybody folded then Big blind won
    ];

    private static readonly string[] _defaultRaiseData =
    [
       //if everyybody folded then Big blind won
    ];

    private static readonly string[] _defaultCallData =
    [
       //if everyybody folded then Big blind won
    ];

    private static readonly string[] _raisedRaiseData =
    [
        "[0.609812]TdTh",
        "[0.8916280000000001]9d9h",
        "[0.9784599999999999]AdQh",
        "[1.06372]AhJh",
        "[1.32212]AhTh",
        "[1.53074]9h7h",
        "[2.42629]Ah4h",
        "[2.6114800000000002]Jh9h",
        "[3.0376]AhQh",
        "[3.04403]Qh3h",
        "[3.81359]Ah6h",
        "[3.84919]7d7h",
        "[4.01329]4h3h",
        "[4.65655]6d6h",
        "[4.89018]Jh8h",
        "[5.5831100000000005]Ad3h",
        "[6.4081399999999995]Qh2h",
        "[6.446159999999999]Ad9h",
        "[7.299189999999999]JhTh",
        "[7.86725]Ah3h",
        "[7.873900000000001]Kh9h",
        "[8.72304]KdJh",
        "[9.05271]KdTh",
        "[9.1197]Ad8h",
        "[10.3493]AdTh",
        "[10.597900000000001]KhQh",
        "[11.5661]Qh8h",
        "[12.8075]KdQh",
        "[13.8294]6h5h",
        "[14.041899999999998]9h8h",
        "[15.187999999999999]3h2h",
        "[18.4239]Ad4h",
        "[19.526]QhTh",
        "[19.534499999999998]Qh9h",
        "[19.6486]Th9h",
        "[21.3466]Kh8h",
        "[21.8616]Kh2h",
        "[22.7316]KhTh",
        "[23.4181]8h7h",
        "[24.0212]QdQh",
        "[27.5119]Kh7h",
        "[28.567999999999998]Kh5h",
        "[29.199599999999997]Kh3h",
        "[30.331799999999998]Kh6h",
        "[31.1508]Ad5h",
        "[31.1803]5h4h",
        "[33.123200000000004]Kh4h",
        "[42.9883]QhJh",
        "[44.8736]7h6h",
        "[48.9878]Ah5h",
        "[54.272299999999994]Ah2h",
        "[66.7727]AdKh",
        "[99.99]AdAh, AhKh, KdKh",
    ];

    private static readonly string[] _raisedFoldData =
    [
        "[4.40295]Qh2h",
        "[19.0632]8h4h",
        "[20.6523]Ad5h",
        "[45.1236]6d5h",
        "[45.169399999999996]Td9h",
        "[49.8409]Ad8h",
        "[52.3757]7d6h",
        "[57.7597]9d8h",
        "[66.7701]5d4h",
        "[69.56360000000001]8d7h",
        "[76.4174]Jh5h",
        "[81.5761]Ad4h",
        "[82.4708]Jh6h",
        "[93.14099999999999]Kd9h",
        "[94.4169]Ad3h",
        "[99.99]Ad7h, Ad6h, Ad2h, Kd8h, Kd7h, Kd6h, Kd5h, Kd4h, Kd3h, Kd2h, Qd9h, Qd8h, Qd7h, Qd6h, Qd5h, " +
        "Qd4h, Qd3h, Qd2h, Jd9h, Jd8h, Jd7h, Jd6h, Jd5h, Jd4h, Jd3h, Jd2h, Td8h, Td7h, Td6h, Td5h, Td4h, " +
        "Td3h, Td2h, 9d7h, 9d6h, 9d5h, 9d4h, 9d3h, 9d2h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, 7d5h, 7d4h, 7d3h, " +
        "7d2h, 6d4h, 6d3h, 6d2h, Th5h, 5d3h, 5d2h, Jh4h, Th4h, 9h4h, 4d3h, 4d2h, Jh3h, Th3h, 9h3h, 8h3h, " +
        "7h3h, 3d2h, Jh2h, Th2h, 9h2h, 8h2h, 7h2h, 6h2h"
    ];

    private static readonly string[] _raisedCallData =
    [
        "[6.858980000000001]Kd9h",
        "[17.5292]Jh6h",
        "[23.5826]Jh5h",
        "[30.436400000000003]8d7h",
        "[33.2273]AdKh",
        "[33.2299]5d4h",
        "[41.0394]Ad8h",
        "[42.2403]9d8h",
        "[45.7277]Ah2h",
        "[47.624300000000005]7d6h",
        "[48.1969]Ad5h",
        "[51.01219999999999]Ah5h",
        "[54.8306]Td9h",
        "[54.876400000000004]6d5h",
        "[55.1264]7h6h",
        "[57.0117]QhJh",
        "[66.8768]Kh4h",
        "[68.8197]5h4h",
        "[69.6682]Kh6h",
        "[70.8004]Kh3h",
        "[71.432]Kh5h",
        "[72.4881]Kh7h",
        "[75.9788]QdQh",
        "[76.5819]8h7h",
        "[77.2684]KhTh",
        "[78.13839999999999]Kh2h",
        "[78.65339999999999]Kh8h",
        "[80.3514]Th9h",
        "[80.4655]Qh9h",
        "[80.474]QhTh",
        "[80.93679999999999]8h4h",
        "[84.6617]3h2h",
        "[85.9581]9h8h",
        "[86.1706]6h5h",
        "[87.1925]KdQh",
        "[88.4339]Qh8h",
        "[89.1889]Qh2h",
        "[89.40209999999999]KhQh",
        "[89.6507]AdTh",
        "[90.9472]KdTh",
        "[91.27690000000001]KdJh",
        "[92.1261]Kh9h",
        "[92.1327]Ah3h",
        "[92.7008]JhTh",
        "[93.55250000000001]Ad9h",
        "[95.1098]Jh8h",
        "[95.3435]6d6h",
        "[95.9867]4h3h",
        "[96.1508]7d7h",
        "[96.1864]Ah6h",
        "[96.9496]Qh3h",
        "[96.9624]AhQh",
        "[97.3885]Jh9h",
        "[97.5737]Ah4h",
        "[98.4693]9h7h",
        "[98.6779]AhTh",
        "[98.9363]AhJh",
        "[99.02149999999999]AdQh",
        "[99.1084]9d9h",
        "[99.3902]TdTh",
        "[99.8775]8d8h",
        "[99.9771]8h6h",
        "[99.98559999999999]Ah9h",
        "[99.9912]Ah7h",
        "[99.99130000000001]Th6h",
        "[99.9968]7h5h",
        "[99.997]9h5h",
        "[99.9978]QdTh",
        "[99.9979]Th7h",
        "[99.9983]Th8h",
        "[99.9993]Qh5h",
        "[99.9995]JdTh, Qh4h",
        "[99.9996]Jh7h, 4h2h",
        "[99.9997]Qh6h",
        "[99.99980000000001]KhJh, Qh7h",
        "[99.9999]QdJh, 5d5h, 6h4h, 6h3h, 5h3h, 5h2h, AdJh, JdJh, Ah8h, 9h6h, 8h5h, 7h4h, 4d4h, 3d3h, 2d2h"
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
        (int, int, Suited) hand = (gameData.HandCards[0].cardSymbol, gameData.HandCards[1].cardSymbol, suited);

        double raise;
        double fold;
        double call;

        RaisedRaise.TryGetValue(hand, out raise);
        RaisedFold.TryGetValue(hand, out fold);
        RaisedCall.TryGetValue(hand, out call);

        return new StrategySolution { Call = call, Fold = fold, Raise = raise };
    }
}
