using Poker.GtoBuilder;
using Poker.GtoBuilder.CardDisplay;
using System.Diagnostics;

namespace Tests.Decks;

public class HandRankerTest
{
    private const int Maximum = 100000;
    private const string Pair = "Pair";
    private const string TwoPair = "TwoPair";
    private const string ThreeOfAKind = "ThreeOfAKind";
    private const string FourOfAKind = "FourOfAKind";
    private const string Straight = "Straight";
    private const string Flush = "Flush";
    private const string FullHouse = "FullHouse";
    private const string HighCard = "HighCard";


    [Fact]
    public void MilionRunTest()
    {
        var deck = new Deck(45678);
        var handRanker = new HandRanker();

        var stopWatch = Stopwatch.StartNew();
        var hand = new (Rank, Suit)[7];
        var ranks = new Dictionary<string, int>
        {
            { HighCard, 0 },
            { Pair, 0 },
            { TwoPair, 0 },
            { ThreeOfAKind, 0 },
            { Straight, 0 },
            { Flush, 0 },
            { FullHouse, 0 },
            { FourOfAKind, 0 },
        };


        deck.TryDeal((Rank.Jack, Suit.Hart), out hand[0]);
        deck.TryDeal((Rank.Nine, Suit.Hart), out hand[1]);

        deck.TryDeal((Rank.Four, Suit.Hart), out hand[2]);
        deck.TryDeal((Rank.Ace, Suit.Spade), out hand[3]);
        deck.TryDeal((Rank.Five, Suit.Hart), out hand[4]);

        for (int i = 0; i < Maximum; i++)
        {
            deck.Shuffle();
            var communityCards = deck.Peek(2);
            hand[5] = communityCards[0];
            hand[6] = communityCards[1];

            
            if (handRanker.IsFourOfAKind(hand))
            {
                ranks[FourOfAKind]++;
            }
            else if (handRanker.IsFullHouse(hand))
            {
                ranks[FullHouse]++;
            }
            else if (handRanker.IsFlush(hand))
            {
                ranks[Flush]++;
            }
            else if (handRanker.IsStraight(hand))
            {
                ranks[Straight]++;
            }
            else if (handRanker.IsThreeOfAKind(hand))
            {
                ranks[ThreeOfAKind]++;
            }
            else if (handRanker.IsTwoPair(hand))
            {
                ranks[TwoPair]++;
            }
            else if (handRanker.IsPair(hand))
            {
                ranks[Pair]++;
            }
            else
            {
                ranks[HighCard]++;
            }
        }

        stopWatch.Stop();

        var results = ranks.Select(x => ( Math.Round((double)x.Value / Maximum * 100, 5), x.Key)).ToList();
        foreach (var res in results)
        {
            Debug.WriteLine($"{res.Item1, 5}% {res.Key}");
        }

        Debug.WriteLine($"{stopWatch.Elapsed.TotalSeconds} s");
  }
}
