using Poker.Common;
using Poker.GtoBuilder;
using System.Diagnostics;

namespace Tests.Decks;

public class HandRankerTest
{

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
        
        

        Debug.WriteLine($"{stopWatch.Elapsed.TotalSeconds} s");
    }
}