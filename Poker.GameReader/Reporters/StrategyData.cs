using Poker.GameReader.Hands;

namespace Poker.GameReader.Reporters;

public struct StrategyData
{
    public double Call { get; init; }
    public double Raise { get; init; }
    public double Fold { get; init; }
    public string SugestedAction { get; set; }

    public Dictionary<string, double> PostFlopHandChances { get; init; }

    public StrategyData()
    {
        SugestedAction = string.Empty;

        PostFlopHandChances = new Dictionary<string, double>
        {
            { Hand.FourOfAKind, 0 },
            { Hand.FullHouse, 0 },
            { Hand.Flush, 0 },
            { Hand.Straight, 0 },
            { Hand.ThreeOfAKind, 0 },
            { Hand.TwoPair, 0 },
            { Hand.OnePair, 0 },
        };
    }
}