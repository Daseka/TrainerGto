using Poker.Common;
using Poker.GtoBuilder;
using System.Diagnostics;

namespace Tests.HandSimulators;

public class HandSimulatorTests
{
    [Fact]
    public void WhenHoldingKingsOnDryBoardVsTwo()
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
        var (win,draw,loss) =  sim.SimulateWinChance(heroHand, [100, 100], [.. community]);
        stop.Stop();
        var time = stop.Elapsed.TotalSeconds;

        Assert.Equal(71.06, win);
        Assert.Equal(0.38, draw);
        Assert.Equal(28.56, loss);
    }

    [Fact]
    public void WhenHoldingKingsOnWetBoardVsTwo()
    {
        var sim = new HandSimulator(6879);

        var heroHand = new (Rank, Suit)[]
        {
            (Rank.King, Suit.Hart),
            (Rank.King, Suit.Diamond),
        };

        var community = new List<(Rank, Suit)>
        {
            (Rank.Six, Suit.Club),
            (Rank.Eight, Suit.Diamond),
            (Rank.Seven, Suit.Club),
        };

        var stop = Stopwatch.StartNew();
        var (win, draw, loss) = sim.SimulateWinChance(heroHand, [100, 100], [.. community]);
        stop.Stop();
        var time = stop.Elapsed.TotalSeconds;

        Assert.Equal(36.33, win);
        Assert.Equal(3.17, draw);
        Assert.Equal(60.5, loss);
    }
}
