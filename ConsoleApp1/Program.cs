using Poker.GameReader.Reporters;
using System.Diagnostics;

internal class Program
{
    private const string WindowName = "NLH";

    public static void Main(string[] args)
    {
        const int Seconds = 2;
        System.Timers.Timer timer = StartAnalysisOnTimer(Seconds);

        do
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine($"Quiting...");
                break;
            }
            GameStateReporter gameStateReporter = new();
            StrategyReporter strategyReporter = new StrategyReporter();
            StartGrabingSreenCards(gameStateReporter, strategyReporter);

        } while (true);
    }

    private static System.Timers.Timer StartAnalysisOnTimer(int seconds)
    {
        var timer = new System.Timers.Timer();
        timer.Interval = seconds * 1000;
        timer.AutoReset = false;

        GameStateReporter gameStateReporter = new();
        StrategyReporter strategyReporter = new StrategyReporter();

        timer.Elapsed += (o, e) =>
        {
            var callingTimer = (o as System.Timers.Timer);
            if (callingTimer != null)
            {
                
                StartGrabingSreenCards(gameStateReporter, strategyReporter);
                callingTimer.Start();
            }
        };

        timer.Start();

        return timer;
    }

    private static string GetCardString((int cardSymbol, int cardSuit) middleCard)
    {
        CardSymbol symbol = (CardSymbol)middleCard.cardSymbol;
        CardSuit suit = (CardSuit)middleCard.cardSuit;

        return symbol == CardSymbol.None
            ? string.Empty
            : $"{symbol} {suit}";
    }

    private static void StartGrabingSreenCards(GameStateReporter gameStateReporter, StrategyReporter strategyReporter)
    {
        Stopwatch stopwatch = new();
        stopwatch.Restart();

        if (!gameStateReporter.ConnectToGame(WindowName))
        {
            Console.WriteLine($"No window found starting with; {WindowName}");
            return;
        }

        GameData gameData = gameStateReporter.GetGameState();
        StrategyData strategyData = strategyReporter.GetStrategy(gameData);

        Console.Clear();
                
        Console.WriteLine($"{Environment.NewLine}=== Bet amounts ===");
        Console.WriteLine(string.Join(' ',gameData.Bets));

        Console.WriteLine($"{Environment.NewLine}================");

        WritePercentageLine(strategyData);
        Console.WriteLine($"been raised: {gameData.HasBeenRaised}");
        Console.ForegroundColor = ConsoleColor.Magenta;
        string leftCard = GetCardString(gameData.HandCards[0]);
        string rightCard = GetCardString(gameData.HandCards[1]);
        Console.WriteLine($"{leftCard} {Environment.NewLine}{rightCard}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{strategyData.SugestedAction}");
        Console.ResetColor();
        Console.WriteLine($"{Environment.NewLine}================");

        stopwatch.Stop();
        Console.WriteLine($"--Time: {stopwatch.Elapsed.TotalSeconds}--");
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
        if (strategyData.Fold > strategyData.Raise && strategyData.Fold> strategyData.Call)
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