using GtoTrainer;
using GtoTrainer.Trainers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poker.Common;
using Poker.GtoBuilder;
using Poker.GtoBuilder.GameSims;

internal class Program
{
    public static void AltStart()  
    {
        //GenerateAndSaveStartingHandFile().GetAwaiter().GetResult();

        var handData = StartingHand
            .StartingHandWinPercentages
            .OrderByDescending(x => x.Value);

        foreach (var group in handData)
        {
            Console.WriteLine($" {group.Value}% ={group.Key}");
        }
    }

    public static async Task Main(string[] args)
    {
        AltStart();

        //await Start();
    }

    private static async Task GenerateAndSaveStartingHandFile()
    {
        var something = new StartingHand(new HandSimulator(new FastDeckBuilder(), new FastHandScorer()));

        _ = await something.SaveStartingHands();
    }

    private static async Task Start()
    {
        var host = new HostBuilder().AddStartup<Startup>().Build();
        var consoleTrainer = host.Services.GetRequiredService<IConsoleTrainer>();

        await consoleTrainer.RunTrainer();
    }

    private static string ToString(Rank rank, Suit suit)
    {
        var rankChar = rank switch
        {
            Rank.Ace => 'A',
            Rank.King => 'K',
            Rank.Queen => 'Q',
            Rank.Jack => 'J',
            Rank.Ten => 'T',
            _ => ((int)rank).ToString()[0]
        };

        var suitChar = suit switch
        {
            Suit.Spade => '♠',
            Suit.Hart => '♥',
            Suit.Diamond => '♦',
            _ => '♣',
        };

        return $"{rankChar}{suitChar}";
    }
}