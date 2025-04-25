using Poker.Common;

namespace Poker.GtoBuilder;

public class HandSimulator : IHandSimulator
{
    private const int BatchSize = 1000;
    private const int HandSize = 2;
    private const int MaxCommunityCards = 5;
    private const int MaxSimulations = 100000;
    private const int Precission = 2;
    private readonly IDeckBuilder _deckBuilder;
    private readonly IHandScorer _handScorer;
    private readonly (Rank, Suit)[][] _startingHandList = [];
    //private readonly (Rank, Suit)[][] _startingHandList = StartingHand
    //    .ReadStartingHands()
    //    .OrderByDescending(x => x.Item2)
    //    .Select(x => x.Item1)
    //    .ToArray();

    public int? Seed { get; set; }

    public HandSimulator(IDeckBuilder deckBuilder, IHandScorer handScorer)
    {
        _deckBuilder = deckBuilder;
        _handScorer = handScorer;
    }

    public async Task<(double win, double draw, double loss)> SimulateWinChance(
        (Rank, Suit)[] heroCards,
        int[] villainHandRangePercentages,
        (Rank, Suit)[] communityCards)
    {
        int count = 0;
        int index = 0;
        int seed = Seed ?? 0;
        TaskFactory taskFactory = new();
        Task<(int win, int draw, int loss)>[] threads = new Task<(int win, int draw, int loss)>[MaxSimulations / BatchSize];

        (Rank, Suit)[][][] villainHandRanges = new (Rank, Suit)[villainHandRangePercentages.Length][][];
        for (int i = 0; i < villainHandRanges.Length; i++)
        {
            villainHandRanges[i] = [.. _startingHandList.Take(_startingHandList.Length * villainHandRangePercentages[i] / 100 + 1)];
        }

        while (count < MaxSimulations)
        {
            threads[index] = taskFactory.StartNew(()
                => RunSimulation(_handScorer, heroCards, villainHandRanges, communityCards, ++seed, BatchSize));

            count += BatchSize;
            index++;
        }

        (int win, int draw, int loss)[] results = await Task.WhenAll(threads);

        await Task.WhenAll(threads);

        double wins = 0;
        double draws = 0;
        double loss = 0;

        foreach ((int win, int draw, int loss) result in results)
        {
            wins += result.win;
            draws += result.draw;
            loss += result.loss;
        }

        return (
            Math.Round(wins / MaxSimulations * 100, Precission),
            Math.Round(draws / MaxSimulations * 100, Precission),
            Math.Round(loss / MaxSimulations * 100, Precission));
    }

    public async Task<(double win, double draw, double loss)> SimulateWinChanceOld(
        (Rank, Suit)[] heroCards,
        int[] villainHandRanges,
        (Rank, Suit)[] communityCards)
    {
        if (villainHandRanges.Length == 0)
        {
            return (100, 0, 0);
        }

        double wins = 0;
        double draws = 0;
        double loss = 0;

        Task[] threads = new Task[MaxSimulations / BatchSize];

        int count = 0;
        int i = 0;

        int seed = Seed ?? 0;

        TaskFactory taskFactory = new();
        while (count < MaxSimulations)
        {
            IDeck deck = _deckBuilder.Build(++seed);
            threads[i] = taskFactory.StartNew(()
                => RunSimulationOld(_handScorer, heroCards, villainHandRanges, communityCards, deck, ref wins, ref draws, ref loss, BatchSize));

            count += BatchSize;
            i++;
        }

        await Task.WhenAll(threads);

        return (
            Math.Round(wins / MaxSimulations * 100, Precission),
            Math.Round(draws / MaxSimulations * 100, Precission),
            Math.Round(loss / MaxSimulations * 100, Precission));
    }

    private static (Rank, Suit)[] GetVillainCards(IDeck deck, int seed, (Rank, Suit)[][] startingHandList)
    {
        if (startingHandList is null)
        {
            throw new ArgumentException("Starting Hand List can't be Null");
        }

        Random random = new(seed);
        int topPercentage = startingHandList.Length;

        (Rank, Suit)[] hand = new (Rank, Suit)[2];
        bool succesfullCard1;
        bool succesfullCard2;
        
        while (topPercentage > 0)
        {
            int index = random.Next(topPercentage);
            succesfullCard1 = deck.CanDeal(startingHandList[index][0]);
            succesfullCard2 = deck.CanDeal(startingHandList[index][1]);

            if (succesfullCard1 && succesfullCard2)
            {
                _ = deck.TryDeal(startingHandList[index][0], out hand[0]);
                _ = deck.TryDeal(startingHandList[index][1], out hand[1]);

                return hand;
            }

            if (!succesfullCard1)
            {
                (startingHandList[index], startingHandList[topPercentage - 1]) = (startingHandList[topPercentage - 1], startingHandList[index]);
                --topPercentage;
            }

            if (!succesfullCard2)
            {
                (startingHandList[index], startingHandList[topPercentage - 1]) = (startingHandList[topPercentage - 1], startingHandList[index]);
                --topPercentage;
            }
        }

        throw new ArgumentException("Could not get vilain hand");
    }

    private static Task RunSimulationOld(
        IHandScorer handScorer,
        (Rank, Suit)[] heroCards,
        int[] villainHandRanges,
        (Rank, Suit)[] communityCards,
        IDeck deck,
        ref double wins, ref double draws, ref double loss, int batchSize)
    {
        int i = 0;
        while (i < batchSize)
        {
            deck.Reset();
            deck.Shuffle();

            (Rank, Suit)[] hand = new (Rank, Suit)[2];
            _ = deck.TryDeal(heroCards[0], out hand[0]);
            _ = deck.TryDeal(heroCards[1], out hand[1]);

            (Rank, Suit)[] community = new (Rank, Suit)[communityCards.Length];
            for (int j = 0; j < communityCards.Length; j++)
            {
                _ = deck.TryDeal(communityCards[j], out community[j]);
            }

            int villainCardCount = villainHandRanges.Length * HandSize;
            int missingCommunityCardCount = MaxCommunityCards - communityCards.Length;

            (Rank, Suit)[] peekedCards = deck.Peek(villainCardCount + missingCommunityCardCount);
            (Rank, Suit)[] fullCommunityCards = [.. communityCards, .. peekedCards.Take(missingCommunityCardCount)];
            long heroScore = handScorer.ScoreHand(hand, [.. fullCommunityCards]);

            int villainCardIndex = missingCommunityCardCount;
            int scoresIndex = 0;
            long maxVillain = 0;

            while (scoresIndex < villainHandRanges.Length)
            {
                long villainScore = handScorer
                    .ScoreHand([peekedCards[villainCardIndex], peekedCards[++villainCardIndex]],[.. fullCommunityCards]);

                if (villainScore > maxVillain)
                {
                    maxVillain = villainScore;
                }

                villainCardIndex++;
                scoresIndex++;
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

    private (int wins, int draws, int loss) RunSimulation(
        IHandScorer handScorer,
        (Rank, Suit)[] heroCards,
        (Rank, Suit)[][][] villainHandRanges,
        (Rank, Suit)[] communityCards,
        int seed,
        int batchSize)
    {
        int wins = 0;
        int draws = 0;
        int loss = 0;

        IDeck deck = _deckBuilder.Build(seed);
        int i = 0;

        while (i < batchSize)
        {
            deck.Reset();
            deck.Shuffle();

            (Rank, Suit)[] hand = new (Rank, Suit)[2];
            _ = deck.TryDeal(heroCards[0], out hand[0]);
            _ = deck.TryDeal(heroCards[1], out hand[1]);

            (Rank, Suit)[] community = new (Rank, Suit)[communityCards.Length];
            for (int j = 0; j < communityCards.Length; j++)
            {
                _ = deck.TryDeal(communityCards[j], out community[j]);
            }

            List<(Rank, Suit)[]> villainHands = new(villainHandRanges.Length);
            foreach ((Rank, Suit)[][] villainHandRange in villainHandRanges)
            {
                villainHands.Add(GetVillainCards(deck, ++seed, villainHandRange));
            }

            int missingCommunityCardCount = MaxCommunityCards - communityCards.Length;
            (Rank, Suit)[] peekedCards = deck.Peek(missingCommunityCardCount);
            (Rank, Suit)[] fullCommunityCards = [.. communityCards, .. peekedCards];
            long heroScore = handScorer.ScoreHand(hand, [.. fullCommunityCards]);

            long maxVillain = 0;
            foreach ((Rank, Suit)[] villainHand in villainHands)
            {
                long villainScore = handScorer.ScoreHand(villainHand, fullCommunityCards);

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

        return (wins, draws, loss);
    }
}