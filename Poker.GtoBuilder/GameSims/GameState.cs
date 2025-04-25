using Poker.Common;

namespace Poker.GtoBuilder.GameSims;

public class GameState
{
    public double[] Bets { get; init; }
    public double BigBlind { get; init; }
    public (Rank cardRank, Suit cardSuit)[] CommunityCards { get; set; }
    public (Rank cardRank, Suit cardSuit)[] HeroCards { get; init; }
    public bool[] TotalPlayersPlaying { get; init; }
    public Position HeroPosition { get; init; }
    public double PotTotal { get; set; }
    public double SmallBlind { get; init; }
}
