using Poker.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Poker.GtoBuilder;

public class StartingHand
{
    private const string FilePath = "StartingHands.json";
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        IncludeFields = true,
        WriteIndented = true
    };
    private readonly IHandSimulator _handSimulator;

    public int MaxRank { get; set; } = CardList.Cards.Length - 1;

    public StartingHand(IHandSimulator handSimulator)
    {
        _handSimulator = handSimulator;
    }

    public static List<((Rank, Suit)[], double)> ReadStartingHands()
    {
        var json = File.ReadAllText(FilePath);
        if (string.IsNullOrEmpty(json))
        {
            throw new ArgumentException($"File not found: {FilePath} ");
        }

        var data = JsonSerializer.Deserialize<List<((Rank, Suit)[], double)>>(json, _jsonOptions);

        return data is null
            ? throw new ArgumentException($"No Data found in file")
            : data;
    }

    public async Task<List<((Rank, Suit)[], double)>> GetStartingHands()
    {
        var threads = new List<Task<((Rank, Suit)[], double)>>();
        var taskFactory = new TaskFactory();

        int max = MaxRank;
        int min = 0;

        for (int i = max; i >= min; i--)
        {
            for (int j = i - 1; j >= min; j--)
            {
                (Rank, Suit)[] hand = [CardList.Cards[i], CardList.Cards[j]];

                threads.Add(taskFactory.StartNew(() =>
                {
                    var (win, draw, _) = _handSimulator.SimulateWinChanceOld(hand, [100], [])
                        .GetAwaiter()
                        .GetResult();

                    return (hand, Math.Round(win + draw, 0));
                }));
            }
        }

        ((Rank, Suit)[], double)[] result = await Task.WhenAll(threads);

        return [.. result];
    }

    public async Task<List<((Rank, Suit)[], double)>> SaveStartingHands()
    {
        List<((Rank, Suit)[], double)> list = await GetStartingHands();

        var json = JsonSerializer.Serialize(list, _jsonOptions);
        File.WriteAllText(FilePath, json);

        return list;
    }
}