using Poker.Common;

namespace Poker.GtoBuilder
{
    public interface IHandSimulator
    {
        int? Seed { get; set; }

        Task<(double win, double draw, double loss)> SimulateWinChance((Rank, Suit)[] heroCards, int[] villainHandRangePercentages, (Rank, Suit)[] communityCards);
        Task<(double win, double draw, double loss)> SimulateWinChanceOld((Rank, Suit)[] heroCards, int[] villainHandRanges, (Rank, Suit)[] communityCards);
    }
}