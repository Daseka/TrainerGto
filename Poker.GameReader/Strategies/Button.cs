using Poker.GameReader.Reporters;

namespace Poker.GameReader.Strategies;

public class Button : BaseStrategy
{
    private static readonly string[] _defaultFoldData =
    [
        "[8.260570000000001]8h5h",
        "[22.7434]Qh2h",
        "[24.1085]5h3h",
        "[38.5441]Td8h",
        "[62.188900000000004]9d8h",
        "[67.02749999999999]Kd7h",
        "[79.84989999999999]Ad3h",
        "[83.0862]Jd8h",
        "[96.2342]Th5h",
        "[99.99]Ad2h, Kd6h, Kd5h, Kd4h, Kd3h, Kd2h, Qd8h, Qd7h, Qd6h, Qd5h, Qd4h, Qd3h, Qd2h, " +
        "Jd7h, Jd6h, Jd5h, Jd4h, Jd3h, Jd2h, Td7h, Td6h, Td5h, Td4h, Td3h, Td2h, 9d7h, 9d6h, 9d5h, 9d4h, " +
        "9d3h, 9d2h, 8d7h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, 7d6h, 7d5h, 7d4h, 7d3h, 7d2h, 6d5h, 6d4h, 6d3h, " +
        "6d2h, 9h5h, 5d4h, 5d3h, 5d2h, Th4h, 9h4h, 8h4h, 7h4h, 4d3h, 4d2h, Jh3h, Th3h, 9h3h, 8h3h, 7h3h, " +
        "6h3h, 4h3h, 3d2h, Jh2h, Th2h, 9h2h, 8h2h, 7h2h, 6h2h, 5h2h, 4h2h, 3h2h"
    ];

    private static readonly string[] _defaultRaiseData =
    [
        "[3.7657599999999998]Th5h",
        "[16.913800000000002]Jd8h",
        "[20.150100000000002]Ad3h",
        "[32.9725]Kd7h",
        "[37.811]9d8h",
        "[61.4559]Td8h",
        "[75.89150000000001]5h3h",
        "[77.25659999999999]Qh2h",
        "[91.7394]8h5h",
        "[99.9776]Jh4h, AdAh, AdKh, AdQh, AdJh, AdTh, Ad9h, Ad8h, Ad7h, Ad6h, Ad5h, Ad4h, AhKh, KdKh, KdQh, " +
        "KdJh, KdTh, Kd9h, Kd8h, AhQh, KhQh, QdQh, QdJh, QdTh, Qd9h, AhJh, KhJh, QhJh, JdJh, JdTh, Jd9h, AhTh, " +
        "KhTh, QhTh, JhTh, TdTh, Td9h, Ah9h, Kh9h, Qh9h, Jh9h, Th9h, 9d9h, Ah8h, Kh8h, Qh8h, Jh8h, Th8h, 9h8h, " +
        "8d8h, Ah7h, Kh7h, Qh7h, Jh7h, Th7h, 9h7h, 8h7h, 7d7h, Ah6h, Kh6h, Qh6h, Jh6h, Th6h, 9h6h, 8h6h, 7h6h, " +
        "6d6h, Ah5h, Kh5h, Qh5h, Jh5h, 7h5h, 6h5h, 5d5h, Ah4h, Kh4h, Qh4h, 6h4h, 5h4h, 4d4h, Ah3h, Kh3h, Qh3h, " +
        "3d3h, Ah2h, Kh2h, 2d2h"
    ];

    private static readonly string[] _raisedCallData =
    [
        "[0.870811]2d2h",
        "[5.15287]AdJh",
        "[11.894200000000001]3d3h",
        "[13.048599999999999]AdKh",
        "[17.011599999999998]8h7h",
        "[17.683699999999998]Ah7h",
        "[20.6303]9h8h",
        "[21.3321]Kh9h",
        "[21.512]4d4h",
        "[22.1436]KdQh",
        "[24.3625]Ah4h",
        "[29.190700000000003]Ah3h",
        "[32.132]QdQh",
        "[32.6758]7h6h",
        "[34.7669]5d5h",
        "[36.175000000000004]6h5h",
        "[36.893]Ah8h",
        "[40.588]Th9h",
        "[42.5723]5h4h",
        "[43.9821]AhQh",
        "[45.793]Ah5h",
        "[47.9118]Ah9h",
        "[48.9458]KhTh",
        "[50.710699999999996]QhJh",
        "[51.6873]QhTh",
        "[52.607899999999994]6d6h",
        "[60.694199999999995]AdQh",
        "[64.7006]TdTh",
        "[67.3775]JhTh",
        "[68.8435]AhTh",
        "[71.6873]7d7h",
        "[71.9238]KhJh",
        "[73.774]8d8h",
        "[75.94340000000001]JdJh",
        "[81.0178]AhJh",
        "[82.2147]KhQh",
        "[84.924]9d9h",
    ];

    private static readonly string[] _raisedFoldData =
    [
        "[5.14285]Kh9h",
        "[21.6814]5h4h",
        "[27.5667]6d6h",
        "[28.5016]Th9h",
        "[31.3596]Ah2h",
        "[35.803200000000004]6h5h",
        "[51.334599999999995]Ah6h",
        "[55.342]7h6h",
        "[59.0167]5d5h",
        "[64.96039999999999]Kh5h",
        "[76.1623]8h7h",
        "[76.1968]AdJh",
        "[78.488]4d4h",
        "[79.3694]9h8h",
        "[79.6602]Kh8h",
        "[88.1058]3d3h",
        "[88.914]Kh7h",
        "[89.6324]KdJh",
        "[98.5399]Kh4h",
        "[99.1292]2d2h",
        "[99.9995]Th8h",
        "[99.99980000000001]6h4h",
        "[99.9999]Kh3h, AdTh, Ad9h, Ad8h, Ad7h, Ad6h, Ad5h, Ad4h, Ad3h, Ad2h, KdTh, Kd9h, Kd8h, Kd7h, Kd6h, Kd5h, Kd4h, Kd3h, Kd2h, QdJh, QdTh, Qd9h, Qd8h, Qd7h, Qd6h, Qd5h, Qd4h, Qd3h, Qd2h, JdTh, Jd9h, Jd8h, Jd7h, Jd6h, Jd5h, Jd4h, Jd3h, Jd2h, Td9h, Td8h, Td7h, Td6h, Td5h, Td4h, Td3h, Td2h, Qh9h, Jh9h, 9d8h, 9d7h, 9d6h, 9d5h, 9d4h, 9d3h, 9d2h, Qh8h, Jh8h, 8d7h, 8d6h, 8d5h, 8d4h, 8d3h, 8d2h, Qh7h, Jh7h, Th7h, 9h7h, 7d6h, 7d5h, 7d4h, 7d3h, 7d2h, Kh6h, Qh6h, Jh6h, Th6h, 9h6h, 8h6h, 6d5h, 6d4h, 6d3h, 6d2h, Qh5h, Jh5h, Th5h, 9h5h, 8h5h, 7h5h, 5d4h, 5d3h, 5d2h, Qh4h, Jh4h, Th4h, 9h4h, 8h4h, 7h4h, 4d3h, 4d2h, Qh3h, Jh3h, Th3h, 9h3h, 8h3h, 7h3h, 6h3h, 5h3h, 4h3h, 3d2h, Kh2h, Qh2h, Jh2h, Th2h, 9h2h, 8h2h, 7h2h, 6h2h, 5h2h, 4h2h, 3h2h"
    ];

    private static readonly string[] _raisedRaiseData =
    [
        "[1.46013]Kh4h",
        "[6.2163900000000005]5d5h",
        "[6.82609]8h7h",
        "[10.367600000000001]KdJh",
        "[11.086]Kh7h",
        "[11.982099999999999]7h6h",
        "[15.076]9d9h",
        "[17.7853]KhQh",
        "[18.6504]AdJh",
        "[18.9822]AhJh",
        "[19.825499999999998]6d6h",
        "[20.3398]Kh8h",
        "[24.0566]JdJh",
        "[26.226]8d8h",
        "[28.021800000000002]6h5h",
        "[28.0762]KhJh",
        "[28.312700000000003]7d7h",
        "[30.9104]Th9h",
        "[31.1564]AhTh",
        "[32.6225]JhTh",
        "[35.0396]Kh5h",
        "[35.2994]TdTh",
        "[35.7463]5h4h",
        "[39.305800000000005]AdQh",
        "[48.2562]Ah6h",
        "[48.3127]QhTh",
        "[49.2892]QhJh",
        "[51.05420000000001]KhTh",
        "[52.088100000000004]Ah9h",
        "[54.206900000000005]Ah5h",
        "[56.0179]AhQh",
        "[63.106700000000004]Ah8h",
        "[67.868]QdQh",
        "[68.19420000000001]Ah2h",
        "[70.8093]Ah3h",
        "[73.52499999999999]Kh9h",
        "[75.6375]Ah4h",
        "[77.7964]KdQh",
        "[82.31490000000001]Ah7h",
        "[99.9514]AdKh, AdAh, AhKh, KdKh"
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
        (int, int, Suited) hand = (gameData.HandCards[0].cardSymbol, gameData.HandCards[1].cardSymbol, suited);

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
