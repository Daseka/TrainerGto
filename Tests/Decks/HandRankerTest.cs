using Poker.GtoBuilder;
using Poker.GtoBuilder.CardDisplay;
using System.Diagnostics;

namespace Tests.Decks;

public class HandRankerTest
{
    private const int Maximum = 100000;

    [Fact]
    public void MilionRunTest()
    {
        var stopWatch = Stopwatch.StartNew();
        var handRanker = new HandRanker(45678);

        var cards = new (Rank, Suit)[]
        {
            (Rank.Jack, Suit.Hart),
            (Rank.Nine, Suit.Hart),
                
            (Rank.Four, Suit.Hart),
            (Rank.Ace, Suit.Spade),
            (Rank.Five, Suit.Hart)
        };

        var ranks = handRanker.CalculateProbabilities(cards);

        stopWatch.Stop();

        var results = ranks.OrderByDescending(x=> x.Value).Select(x => (Math.Round((double)x.Value / Maximum * 100, 2), x.Key)).ToList();
        foreach (var res in results)
        {
            Debug.WriteLine($"{res.Item1,5}% {res.Key}");
        }

        Debug.WriteLine($"{stopWatch.Elapsed.TotalSeconds} s");
    }
}