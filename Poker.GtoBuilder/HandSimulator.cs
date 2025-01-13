using Poker.Common;

namespace Poker.GtoBuilder;

public class HandSimulator(int? seed = null)
{
    private const int MaxSimulations = 100000;
    private const int HandSize = 2;
    private const int Precission = 2;
    private const int MaxCommunityCards = 5;

    private readonly Deck _deck = new(seed);

    public (double win, double draw, double loss) SimulateWinChance(
        (Rank, Suit)[] heroCards,
        int[] villainHandRanges,
        (Rank, Suit)[] communityCards)
    {
        _deck.Reset();
        _deck.Shuffle();

        var hand = new (Rank, Suit)[2];
        _deck.TryDeal(heroCards[0], out hand[0]);
        _deck.TryDeal(heroCards[1], out hand[1]);

        var community = new (Rank, Suit)[communityCards.Length];
        for (int i = 0; i < communityCards.Length; i++)
        {
            _deck.TryDeal(communityCards[i], out community[i]);
        }

        int villainCardCount = (villainHandRanges.Length * HandSize);
        int missingCommunityCardCount = (MaxCommunityCards - communityCards.Length);

        double wins = 0;
        double draws = 0;
        double loss = 0;

        for (int i = 0; i < MaxSimulations; i++)
        {
            var peekedCards = _deck.Peek(villainCardCount + missingCommunityCardCount);
            (Rank, Suit)[] fullCommunityCards = [.. communityCards, .. peekedCards.Take(missingCommunityCardCount)];
            long heroScore = HandScorer.ScoreHand(hand, [.. fullCommunityCards]);

            var villainScores = new long[villainHandRanges.Length];
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

            _deck.Reset();
            _deck.Shuffle();
        }

        return (
            Math.Round(wins / MaxSimulations * 100, Precission), 
            Math.Round(draws / MaxSimulations * 100, Precission), 
            Math.Round(loss / MaxSimulations * 100, Precission));
    }
}