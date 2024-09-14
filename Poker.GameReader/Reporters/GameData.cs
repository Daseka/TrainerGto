namespace Poker.GameReader.Reporters;

public readonly struct GameData
{
    public double CallAmount { get; init; }
    public (int cardSymbol, int cardSuit)[] HandCards { get; init; }
    public (int cardSymbol, int cardSuit)[] MiddleCards { get; init; }
    public Position Position { get; init; }
    public double PotTotal { get; init; }
}
