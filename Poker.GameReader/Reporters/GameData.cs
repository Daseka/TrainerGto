namespace Poker.GameReader.Reporters;

public readonly struct GameData
{
    public double[] Bets { get; init; }
    public bool[] VillainsPlaying { get; init; }
    public double BigBlind { get; init; }
    public double CallAmount { get; init; }
    public (int cardRank, int cardSuit)[] CommunityCards { get; init; }
    public (int cardRank, int cardSuit)[] HandCards { get; init; }
    public bool HasBeenRaised => Bets.Max() > BigBlind;
    public Position Position { get; init; }
    public double PotOdds => Math.Round(CallAmount / (CallAmount + PotTotal), 2);
    public double PotTotal { get; init; }
    public double SmallBlind { get; init; }
}