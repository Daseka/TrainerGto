﻿using Poker.Common;
using Poker.GameReader.Reporters;
using System.Diagnostics;
using System.Text;

namespace GtoTrainer.Trainers;

internal class ConsoleTrainer
{
    const int Seconds = 2;
    private static readonly string[] WindowNames = ["NLH", "Rush"];

    public static async Task RunConsoleTrainer()
    {
        await StartAnalysisOnTimer(Seconds);

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
            await PrintGameStateReport(gameStateReporter, strategyReporter);
        } while (true);

        return;
    }

    private static string GetCardString((int cardRank, int cardSuit) middleCard)
    {
        CardRank rank = (CardRank)middleCard.cardRank;
        CardSuit suit = (CardSuit)middleCard.cardSuit;

        return rank == CardRank.None
            ? string.Empty
            : $"{rank} {suit}";
    }

    private static string GetFoldedList(GameData gameData)
    {
        bool[] playing = [gameData.HandCards[0].cardRank != (int)Rank.None, .. gameData.VillainsPlaying];
        var stringBuilder = new StringBuilder();
        foreach (bool item in playing)
        {
            stringBuilder.Append(item ? "1 " : "0 ");
        }

        return stringBuilder.ToString();
    }

    private static async Task PrintGameStateReport(GameStateReporter gameStateReporter, StrategyReporter strategyReporter)
    {
        Stopwatch stopwatch = new();
        stopwatch.Restart();

        if (!gameStateReporter.ConnectToGame(WindowNames))
        {
            Console.WriteLine($"No window found starting with: {string.Join(",", WindowNames)}");
            return;
        }

        GameData gameData = await gameStateReporter.GetGameState();
        StrategyData strategyData = await strategyReporter.GetStrategy(gameData);

        Console.Clear();

        Console.WriteLine($"{Environment.NewLine}=== Playing ===");
        string stillPlaying = GetFoldedList(gameData);
        Console.WriteLine(stillPlaying);

        Console.WriteLine($"{Environment.NewLine}=== Bet amounts ===");
        Console.WriteLine(string.Join(' ', gameData.Bets));

        Console.WriteLine($"{Environment.NewLine}=== Pot total ===");
        Console.WriteLine(string.Join(' ', gameData.PotTotal));

        Console.WriteLine($"{Environment.NewLine}================");

        WritePercentageLine(strategyData);
        Console.WriteLine($"Raised: {gameData.HasBeenRaised}");

        Console.ForegroundColor = ConsoleColor.Magenta;
        string leftCard = GetCardString(gameData.HandCards[0]);
        string rightCard = GetCardString(gameData.HandCards[1]);
        Console.WriteLine($"{leftCard} {Environment.NewLine}{rightCard}");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{strategyData.SugestedAction}");
        Console.ResetColor();

        Console.WriteLine($"{Environment.NewLine}=== Bet amount ===");
        WriteBetAmountLine(strategyData, gameData);

        Console.WriteLine($"{Environment.NewLine}================");

        stopwatch.Stop();
        Console.WriteLine($"--Time: {stopwatch.Elapsed.TotalSeconds}--");

        return;
    }

    private static Task<System.Timers.Timer> StartAnalysisOnTimer(int seconds)
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
                PrintGameStateReport(gameStateReporter, strategyReporter).GetAwaiter().GetResult();
                callingTimer.Start();
            }
        };

        timer.Start();

        return Task.FromResult(timer);
    }

    private static void WriteBetAmountLine(StrategyData strategyData, GameData gameData)
    {
        if (strategyData.MaxBet != 0)
        {
            Console.ForegroundColor = strategyData.Fold > strategyData.Raise && strategyData.Fold > strategyData.Call 
                ? ConsoleColor.DarkGreen
                : ConsoleColor.Green;
            Console.WriteLine($"{Environment.NewLine}Max bet amount {Math.Round(strategyData.MaxBet, 2):f2} ");

            string minBet = strategyData.MinBet > 0
                ? Math.Round(strategyData.MinBet, 2).ToString()
                : "Fold";

            Console.ForegroundColor = strategyData.Fold > strategyData.Raise && strategyData.Fold > strategyData.Call
                ? ConsoleColor.Green
                : ConsoleColor.DarkGreen;
            Console.WriteLine($"Min bet amout {minBet:f2} ");
        }
        else if (gameData.HandCards[0].cardRank != (int)Rank.None)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{Environment.NewLine} Check");
        }
        else  
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{Environment.NewLine}No bet amount");
        }

        Console.ResetColor();
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