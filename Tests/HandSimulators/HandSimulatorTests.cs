using HandEvaluator;
using Poker.Common;
using Poker.GtoBuilder;
using System.Diagnostics;

namespace Tests.HandSimulators;

public class HandSimulatorTests
{
    [Fact]
    public void Bla8()
    {
        List<((Rank, Suit)[], double)> list = StartingHand.ReadStartingHands();
        var keyVall = list
                .Select(x => (HandToKey(x.Item1), x.Item2))
                .ToArray();



        Dictionary<ulong, double> _handEquityIndex = keyVall
        .ToDictionary((key) => key.Item1, (value) => value.Item2);

        ulong HandToKey((Rank, Suit)[] hand)
        {
            ulong card1 = (0x1UL << (int)hand[0].Item1) << (13 * ((int)hand[0].Item2 -1));
            ulong card2 = (0x1UL << (int)hand[1].Item1) << (13 * ((int)hand[1].Item2 - 1));

            return card1 + card2;
        }

        double GetHandEquity((Rank, Suit)[] handCards)
        {
            ulong key = HandToKey(handCards);

            return key == 0 ? 0 : _handEquityIndex[key];
        }


        (Rank, Suit)[] hand1 = [(Rank.Ace, Suit.Diamond), (Rank.Ace, Suit.Hart)];
        (Rank, Suit)[] hand2 = [(Rank.Ace, Suit.Diamond), (Rank.Two, Suit.Hart)];

        var val1 = GetHandEquity(hand1);
        var val2 = GetHandEquity(hand2);
    }

    [Fact]
    public void Blabla()
    {
        var x = new FastHandScorer();

        (Rank, Suit)[] heroCards =
        [
            (Rank.Five, Suit.Spade),
            (Rank.Ace, Suit.Spade)
        ];

        (Rank, Suit)[] villCards =
        [
            (Rank.King, Suit.Diamond),
            (Rank.Queen, Suit.Diamond)
        ];

        (Rank, Suit)[] communityCards =
        [
            (Rank.Ten, Suit.Diamond),
            (Rank.Jack, Suit.Club),
            (Rank.King, Suit.Club),
            (Rank.Queen, Suit.Spade),
        ];

        long hero = x.ScoreHand(heroCards, communityCards);
        long vill = x.ScoreHand(villCards, communityCards);

        var heroHand = Hand.DescriptionFromHandValueInternal((uint)hero);
        var villHand = Hand.DescriptionFromHandValueInternal((uint)vill);


        Assert.True(hero > vill);
    }

    [Fact]
    public async Task bla()
    {
        (Rank, Suit)[] heroCards =
        [
            (Rank.King, Suit.Spade),
            (Rank.Ace, Suit.Spade)
        ];

        int[] villainPercentages = [100];

        (Rank, Suit)[] communityCards =
        [
            (Rank.Two, Suit.Diamond),
            (Rank.Eight, Suit.Club),
            (Rank.Queen, Suit.Spade),
        ];

        var simulator = new HandSimulator(new DeckBuilder(), new HandScorer());
        var stopWatch = Stopwatch.StartNew();
        var (win2, tie2, loss2) = await simulator.SimulateWinChanceOld(heroCards, villainPercentages, communityCards);
        stopWatch.Stop();
        var seconds2 = stopWatch.Elapsed.TotalSeconds;

        stopWatch = Stopwatch.StartNew();
        var (win, tie, loss) = await simulator.SimulateWinChance(heroCards, villainPercentages, communityCards);
        stopWatch.Stop();
        var seconds = stopWatch.Elapsed.TotalSeconds;

        var simulator2 = new HandSimulator(new FastDeckBuilder(), new FastHandScorer());
        stopWatch = Stopwatch.StartNew();
        var (win4, tie4, loss4) = await simulator2.SimulateWinChanceOld(heroCards, villainPercentages, communityCards);
        stopWatch.Stop();
        var seconds4 = stopWatch.Elapsed.TotalSeconds;

        stopWatch = Stopwatch.StartNew();
        var (win3, tie3, loss3) = await simulator2.SimulateWinChance(heroCards, villainPercentages, communityCards);
        stopWatch.Stop();
        var seconds3 = stopWatch.Elapsed.TotalSeconds;

    }

    [Fact]
    public async Task bla2()
    {
        int i = 0;
        int count = 0;
        var taskFactory = new TaskFactory();
        const int startingSeed = 123;
        int seed = startingSeed;

        const int size = 10;
        var threads = new Task<int>[size];
        while (count < size)
        {
            threads[i] = taskFactory
                .StartNew(() =>
                {
                    var random = new Random(++seed);

                    return random.Next(100);
                });

            count++;
            i++;
        }

        var results = await Task.WhenAll(threads);

        i = 0;
        seed = startingSeed;
        count = 0;
        var nums = new int[size];
        while (count < size)
        {
            var deck = new Random(++seed);
            nums[i] = deck.Next(100);

            count++;
            i++;
        }

        Assert.True(results.Sum() == nums.Sum());
    }
}