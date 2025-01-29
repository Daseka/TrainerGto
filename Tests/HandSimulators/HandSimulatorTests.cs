using Poker.Common;
using Poker.GtoBuilder;
using System.Diagnostics;

namespace Tests.HandSimulators;


public class HandSimulatorTests
{
    [Fact]
    public async Task WhenHoldingKingsOnDryBoardVsTwo()
    {
        var sim = new HandSimulator(6879);

        var heroHand = new (Rank, Suit)[]
        {
            (Rank.King, Suit.Spade),
            (Rank.King, Suit.Diamond),
        };

        var community = new List<(Rank, Suit)>
        {
            (Rank.Queen, Suit.Spade),
            (Rank.Seven, Suit.Diamond),
            (Rank.Three, Suit.Hart),
        };

        var stop = Stopwatch.StartNew();
        var (win,draw,loss) =  await sim.SimulateWinChance(heroHand, [100, 100], [.. community]);
        stop.Stop();
        var time = stop.Elapsed.TotalSeconds;

        Assert.Equal(73.7, win);
        Assert.Equal(0.17, draw);
        Assert.Equal(26.08, loss);
    }

    [Fact]
    public async Task WhenHoldingKingsOnWetBoardVsTwo()
    {
        var sim = new HandSimulator(6879);

        var heroHand = new (Rank, Suit)[]
        {
            (Rank.Six, Suit.Club),
            (Rank.Six, Suit.Diamond),
        };

        var community = new List<(Rank, Suit)>
        {
            (Rank.Two, Suit.Hart),
            (Rank.Nine, Suit.Diamond),
            (Rank.Ace, Suit.Spade),
        };

        var stop = Stopwatch.StartNew();
        var (win, draw, loss) = await sim.SimulateWinChance(heroHand, [100, 100], [.. community]);
        stop.Stop();
        var time = stop.Elapsed.TotalSeconds;

        Assert.Equal(36.93, win);
        Assert.Equal(0.13, draw);
        Assert.Equal(62.88, loss);
    }
}
