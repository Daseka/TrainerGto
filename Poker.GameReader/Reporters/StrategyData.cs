using Poker.Common;

namespace Poker.GameReader.Reporters;

public struct StrategyData
{
    public double Call { get; init; }
    public double Fold { get; init; }
    public Dictionary<string, double> PostFlopHandChances { get; set; }
    public double Raise { get; init; }
    public string SugestedAction { get; set; }

    public StrategyData()
    {
        SugestedAction = string.Empty;

        PostFlopHandChances = new Dictionary<string, double>
        {
            { HandNames.FourOfAKind, 0 },
            { HandNames.FullHouse, 0 },
            { HandNames.Flush, 0 },
            { HandNames.Straight, 0 },
            { HandNames.ThreeOfAKind, 0 },
            { HandNames.TwoPair, 0 },
            { HandNames.Pair, 0 },
        };
    }
}