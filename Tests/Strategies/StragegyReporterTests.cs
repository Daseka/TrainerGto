using Poker.Common;
using Poker.GameReader.Reporters;
using System.Diagnostics;

namespace Tests.Strategies
{
    public class StragegyReporterTests
    {
        [Fact]
        public void bla()
        {
            var reporter = new StrategyReporter();

            var gameData = new GameData
            {
                Bets = [1, 2, 3, 0, 0, 0],
                BigBlind = 2,
                SmallBlind = 1,
                HandCards = [((int)Rank.Ace, (int)Suit.Spade), ((int)Rank.King, (int)Suit.Spade)],
                CommunityCards = [
                    ((int)Rank.Queen, (int)Suit.Hart),
                    ((int)Rank.Five, (int)Suit.Hart),
                    ((int)Rank.Nine, (int)Suit.Diamond), 
                    ((int)Rank.None, (int)Suit.None), 
                    ((int)Rank.None, (int)Suit.None)]
            };

            var stopWatch = Stopwatch.StartNew();

            var strategy = reporter.GetStrategy(gameData);

            Debug.WriteLine($"{strategy.SugestedAction}");

            stopWatch.Stop();

            Debug.WriteLine($"{stopWatch.Elapsed.TotalSeconds}");

        }
    }
}