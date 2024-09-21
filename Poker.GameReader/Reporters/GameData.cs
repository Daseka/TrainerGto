namespace Poker.GameReader.Reporters;

public readonly struct GameData
{
    public double[] Bets { get; init; }
    public double CallAmount { get; init; }
    public (int cardSymbol, int cardSuit)[] HandCards { get; init; }
    public (int cardSymbol, int cardSuit)[] MiddleCards { get; init; }
    public Position Position { get; init; }
    public double PotTotal { get; init; }
    public double SmallBlind { get; init; }
    public double BigBlind { get; init; }

    public bool HasBeenRaised
    {
        get
        {
            return Bets.Max() > BigBlind;
        }
    }
}
