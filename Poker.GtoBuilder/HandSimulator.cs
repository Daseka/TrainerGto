using Poker.Common;

namespace Poker.GtoBuilder;

public class HandSimulator(int? seed = null)
{
    private const int HandSize = 2;
    private const int MaxCommunityCards = 5;
    private const int MaxSimulations = 100000;
    private const int Precission = 2;
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
                => RunSimulation(heroCards, villainHandRanges, communityCards, new Deck(++seed), ref wins, ref draws, ref loss, batchSize));

            count += batchSize;
            i++;
        }

        await Task.WhenAll(threads);

        return (
            Math.Round(wins / MaxSimulations * 100, Precission),
            Math.Round(draws / MaxSimulations * 100, Precission),
            Math.Round(loss / MaxSimulations * 100, Precission));
    }

    private static Task RunSimulation(
        (Rank, Suit)[] heroCards,
        int[] villainHandRanges,
        (Rank, Suit)[] communityCards,
        Deck deck,
        ref double wins, ref double draws, ref double loss, int batchSize)
    {
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

            int villainCardCount = (villainHandRanges.Length * HandSize);
            int missingCommunityCardCount = (MaxCommunityCards - communityCards.Length);

            var peekedCards = deck.Peek(villainCardCount + missingCommunityCardCount);
            (Rank, Suit)[] fullCommunityCards = [.. communityCards, .. peekedCards.Take(missingCommunityCardCount)];
            long heroScore = HandScorer.ScoreHand(hand, [.. fullCommunityCards]);

            int villainCardIndex = missingCommunityCardCount;
            int scoresIndex = 0;
            long maxVillain = 0;

            while (scoresIndex < villainHandRanges.Length)
            {
                var villainScore = HandScorer.ScoreHand(
                    [peekedCards[villainCardIndex], peekedCards[++villainCardIndex]],
                    [.. fullCommunityCards]);

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
}