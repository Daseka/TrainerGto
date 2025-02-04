using Poker.Common;

namespace Poker.GtoBuilder;

public class HandSimulator(int? seed = null)
{
    private const int MaxCommunityCards = 5;
    private const int MaxSimulations = 100000;
    private const int Precission = 2;
    private static readonly (Rank, Suit)[][]? _startingHandList = StartingHand
        .ReadStartingHands()
        ?.OrderByDescending(x => x.Item2)
        .Select(x => x.Item1)
        .ToArray();
    private readonly int? _seed = seed;

    public async Task<(double win, double draw, double loss)> SimulateWinChance(
        (Rank, Suit)[] heroCards,
        int[] villainHandRanges,
        (Rank, Suit)[] communityCards)
    {
        double wins = 0;
        double draws = 0;
        double loss = 0;

        int batchSize = 1000;
        var threads = new Task[MaxSimulations / batchSize];

        int count = 0;
        int i = 0;
        int seed = _seed ?? 0;

        var taskFactory = new TaskFactory();
        while (count < MaxSimulations)
        {
            threads[i] = taskFactory.StartNew(()
                => RunSimulation(heroCards, villainHandRanges, communityCards, ++seed, ref wins, ref draws, ref loss, batchSize));

            count += batchSize;
            i++;
        }

        await Task.WhenAll(threads);

        return (
            Math.Round(wins / MaxSimulations * 100, Precission),
            Math.Round(draws / MaxSimulations * 100, Precission),
            Math.Round(loss / MaxSimulations * 100, Precission));
    }

    private (Rank, Suit)[] GetVillainCards(int playPercentage, Deck deck, int seed)
    {
        if (_startingHandList is null)
        {
            throw new ArgumentException("Starting Hand List can't be Null");
        }

        var random = new Random(seed);
        var topPercentage = (_startingHandList.Length * playPercentage / 100) + 1;

        var hand = new (Rank, Suit)[2];
        bool succesfullCard1 = false;
        bool succesfullCard2 = false;

        var errors = new List<string>();

        while (topPercentage > 0)
        {
            var index = random.Next(topPercentage);
            succesfullCard1 = deck.CanDeal(_startingHandList[index][0]);
            succesfullCard2 = deck.CanDeal(_startingHandList[index][1]);

            if (succesfullCard1 && succesfullCard2)
            {
                deck.TryDeal(_startingHandList[index][0], out hand[0]);
                deck.TryDeal(_startingHandList[index][1], out hand[1]);

                return hand;
            }

            if (!succesfullCard1)
            {
                errors.Add($"{_startingHandList[index][0].Item1} {_startingHandList[index][0].Item2}");
                (_startingHandList[index], _startingHandList[topPercentage]) = (_startingHandList[topPercentage], _startingHandList[index]);
                --topPercentage;
                continue;
            }

            if (!succesfullCard2)
            {
                errors.Add($"{_startingHandList[index][1].Item1} {_startingHandList[index][1].Item2}");
                (_startingHandList[index], _startingHandList[topPercentage]) = (_startingHandList[topPercentage], _startingHandList[index]);
                --topPercentage;
            }
        }
        
        throw new ArgumentException("Tried 5 times and failed");
    }

    private Task RunSimulation(
        (Rank, Suit)[] heroCards,
        int[] villainPlayPercentages,
        (Rank, Suit)[] communityCards,
        int seed,
        ref double wins, ref double draws, ref double loss, int batchSize)
    {
        var deck = new Deck(seed);
        int i = 0;

        while (i < batchSize)
        {
            deck.Reset();
            deck.Shuffle();

            var hand = new (Rank, Suit)[2];
            deck.TryDeal(heroCards[0], out hand[0]);
            deck.TryDeal(heroCards[1], out hand[1]);

            var community = new (Rank, Suit)[communityCards.Length];
            for (int j = 0; j < communityCards.Length; j++)
            {
                deck.TryDeal(communityCards[j], out community[j]);
            }

            var villainHands = new List<(Rank, Suit)[]>(villainPlayPercentages.Length);
            foreach (var percentage in villainPlayPercentages)
            {
                villainHands.Add(GetVillainCards(percentage, deck, seed));
            }

            int missingCommunityCardCount = (MaxCommunityCards - communityCards.Length);
            var peekedCards = deck.Peek(missingCommunityCardCount);
            (Rank, Suit)[] fullCommunityCards = [.. communityCards, .. peekedCards];
            long heroScore = HandScorer.ScoreHand(hand, [.. fullCommunityCards]);

            long maxVillain = 0;
            foreach (var villainHand in villainHands)
            {
                var villainScore = HandScorer.ScoreHand(villainHand, fullCommunityCards);

                if (villainScore > maxVillain)
                {
                    maxVillain = villainScore;
                }
            }

            //Determin if highest scoring villain beats hero
            if (heroScore > maxVillain)
            {
                wins++;
            }
            else if (heroScore == maxVillain)
            {
                draws++;
            }
            else
            {
                loss++;
            }

            i++;
        }

        return Task.CompletedTask;
    }
}