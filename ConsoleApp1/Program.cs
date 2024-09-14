using Poker.GameReader.Reporters;
using System.Diagnostics;

internal class Program
{
    private const string WindowName = "NLH";

    public static void Main(string[] args)
    {
        const int Seconds = 5;
        System.Timers.Timer timer = StartAnalysisOnTimer(Seconds);

        do
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine($"Quiting...");
                break;
            }
        } while (true);
    }

    private static System.Timers.Timer StartAnalysisOnTimer(int seconds)
    {
        var timer = new System.Timers.Timer();
        timer.Interval = seconds * 1000;
        timer.AutoReset = false;

        timer.Elapsed += (o, e) =>
        {
            var callingTimer = (o as System.Timers.Timer);
            if (callingTimer != null)
            {
                StartGrabingSreenCards(new GameStateReporter());
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

        return $"{symbol} {suit}";
    }

    private static void StartGrabingSreenCards(GameStateReporter gameStateReporter)
    {
        Console.WriteLine($"Grabbing screens for window starting with: {WindowName}");
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Restart();

        if (!gameStateReporter.ConnectToGame(WindowName))
        {
            Console.WriteLine($"No window found starting with; {WindowName}");
            return;
        }

        GameData data = gameStateReporter.GetGameState();

        Console.Clear();
        //Take screenshot of middle cards
        Console.WriteLine($"{Environment.NewLine}=== Middle Cards ===");

        foreach (var middleCard in data.MiddleCards)
        {
            var value = GetCardString(middleCard);
            Console.WriteLine(value);
        }

        Console.WriteLine($"{Environment.NewLine}=== Left & Right Cards ===");
        var leftCard = GetCardString(data.HandCards[0]);
        Console.WriteLine(leftCard);
        var rightCard = GetCardString(data.HandCards[1]);
        Console.WriteLine(rightCard);

        Console.WriteLine($"{Environment.NewLine}=== Pot Total ===");
        Console.WriteLine(data.PotTotal);

        Console.WriteLine($"{Environment.NewLine}=== Call amount ===");
        Console.WriteLine(data.CallAmount);

        Console.WriteLine($"{Environment.NewLine}=== Position ===");
        Console.WriteLine(data.Position);

        stopwatch.Stop();
        Console.WriteLine($"--- calculation time: {stopwatch.Elapsed.TotalSeconds} ---");
    }
}