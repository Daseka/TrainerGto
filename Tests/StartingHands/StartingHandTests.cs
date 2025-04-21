using Poker.GtoBuilder;
using System.Diagnostics;

namespace Tests.StartingHands;

public class StartingHandTests
{
    [Fact]
    public void Bla2()
    {

        var something = StartingHand.ReadStartingHands();
        var list = something?
            .OrderByDescending(x => x.Item2)
            .Select(x => $"{x.Item1[0].Item1} {x.Item1[0].Item2} - {x.Item1[1].Item1} {x.Item1[1].Item2}")
            .ToList();
    }

    [Fact]
    public async Task Bla3()
    {
        var something = new StartingHand(new HandSimulator(new FastDeckBuilder(), new FastHandScorer()));
        var sw = Stopwatch.StartNew();
        
        var result = await something.GetStartingHands();
        var sorted = result
            .OrderByDescending(x => x.Item2)
            .ToList();


        sw.Stop();
        var time = sw.Elapsed.TotalSeconds;
    }
}
