using Poker.GtoBuilder;

namespace Tests.StartingHands;

public class StartingHandTests
{
    [Fact]
    public async Task Bla()
    {
        var startingHand = new StartingHand();

        await startingHand.SaveStartingHands();
    }

    [Fact]
    public void Bla2()
    {

        var something = StartingHand.ReadStartingHands();
        var list = something?
            .OrderByDescending(x => x.Item2)
            .Select(x => $"{x.Item1[0].Item1} {x.Item1[0].Item2} - {x.Item1[1].Item1} {x.Item1[1].Item2}")
            .ToList();
    }
}
