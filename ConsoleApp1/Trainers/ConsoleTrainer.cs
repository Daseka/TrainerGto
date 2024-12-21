using Poker.GameReader.Reporters;
using System.Diagnostics;

namespace GtoTrainer.Trainers;

internal class ConsoleTrainer
{
    const int Seconds = 2;
    private static readonly string[] WindowNames = ["NLH", "Rush"];

    public static void RunConsoleTrainer()
    {
        StartAnalysisOnTimer(Seconds);

        do
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine($"Quiting...");
                break;
            }
            GameStateReporter gameStateReporter = new();
            var strategyReporter = new StrategyReporter();
            PrintGameStateReport(gameStateReporter, strategyReporter);
        } while (true);
    }

    private static string GetCardString((int cardRank, int cardSuit) middleCard)
    {
        CardRank rank = (CardRank)middleCard.cardRank;
        CardSuit suit = (CardSuit)middleCard.cardSuit;

        return rank == CardRank.None
            ? string.Empty
            : $"{rank} {suit}";
    }

    private static void PrintGameStateReport(GameStateReporter gameStateReporter, StrategyReporter strategyReporter)
    {
        Stopwatch stopwatch = new();
        stopwatch.Restart();

        if (!gameStateReporter.ConnectToGame(WindowNames))
        {
            Console.WriteLine($"No window found starting with: {string.Join(",", WindowNames)}");
            return;
        }

        GameData gameData = gameStateReporter.GetGameState();
        StrategyData strategyData = strategyReporter.GetStrategy(gameData);

        Console.Clear();

        Console.WriteLine($"{Environment.NewLine}=== Bet amounts ===");
        Console.WriteLine(string.Join(' ', gameData.Bets));

        Console.WriteLine($"{Environment.NewLine}================");

        WritePercentageLine(strategyData);
        Console.WriteLine($"been raised: {gameData.HasBeenRaised}");

        Console.ForegroundColor = ConsoleColor.Magenta;
        string leftCard = GetCardString(gameData.HandCards[0]);
        string rightCard = GetCardString(gameData.HandCards[1]);
        Console.WriteLine($"{leftCard} {Environment.NewLine}{rightCard}");

        if (gameData.PotOdds > 0)
        {
            Console.ForegroundColor = gameData.PotOdds <= 0.32
                ? ConsoleColor.Green
                : ConsoleColor.DarkRed;
            Console.WriteLine($"{Environment.NewLine}Pot Odds: {gameData.PotOdds * 100}%");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{strategyData.SugestedAction}");
        Console.ResetColor();

        Console.WriteLine($"{Environment.NewLine}================");

        stopwatch.Stop();
        Console.WriteLine($"--Time: {stopwatch.Elapsed.TotalSeconds}--");
    }

    private static System.Timers.Timer StartAnalysisOnTimer(int seconds)
    {
        var timer = new System.Timers.Timer
        {
            Interval = seconds * 1000,
            AutoReset = false
        };

        var gameStateReporter = new GameStateReporter();
        var strategyReporter = new StrategyReporter();

        timer.Elapsed += (o, e) =>
        {
            if (o is System.Timers.Timer callingTimer)
            {
                PrintGameStateReport(gameStateReporter, strategyReporter);
                callingTimer.Start();
            }
        };

        timer.Start();

        return timer;
    }

    private static void WritePercentageLine(StrategyData strategyData)
    {
        Console.Write("C:");
        if (strategyData.Call > strategyData.Raise && strategyData.Call > strategyData.Fold)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else if (strategyData.Call > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        Console.Write($"{strategyData.Call}% ");
        Console.ResetColor();

        Console.Write("R:");
        if (strategyData.Raise > strategyData.Call && strategyData.Raise > strategyData.Fold)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else if (strategyData.Raise > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        Console.Write($"{strategyData.Raise}% ");
        Console.ResetColor();

        Console.Write("F:");
        if (strategyData.Fold > strategyData.Raise && strategyData.Fold > strategyData.Call)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else if (strategyData.Fold > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        Console.WriteLine($"{strategyData.Fold}% ");
        Console.ResetColor();
    }
}