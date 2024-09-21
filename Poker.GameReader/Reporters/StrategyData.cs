namespace Poker.GameReader.Reporters;

public readonly struct StrategyData
{
    public double Call { get; init; }
    public double Raise { get; init; }
    public double Fold { get; init; }
    public string SugestedAction { get; init; }
}