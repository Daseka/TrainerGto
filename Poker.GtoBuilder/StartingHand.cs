using Poker.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Poker.GtoBuilder;

public class StartingHand
{
    private const int MaxRank = (int)Rank.King;
    private const int MinRank = (int)Rank.None;
    private const int MaxSuit = (int)Suit.Spade;
    private const int MinSuit = (int)Suit.None;
    private const string FilePath = "StartingHands.json";
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        IncludeFields = true,
        WriteIndented = true
    };

    public async Task<List<((Rank, Suit)[], double)>> SaveStartingHands()
    {
        List<((Rank, Suit)[], double)> list = await GetStartingHands();

        var json = JsonSerializer.Serialize(list, _jsonOptions);
        File.WriteAllText(FilePath, json);

        return list;
    }

    public static List<((Rank, Suit)[], double)>? ReadStartingHands()
    {
        var json = File.ReadAllText(FilePath);
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        var data = JsonSerializer.Deserialize<List<((Rank, Suit)[], double)>>(json, _jsonOptions);

        return data;
    }

    public async Task<List<((Rank, Suit)[], double)>> GetStartingHands()
    {
        var simulator = new HandSimulator();
        var threads = new List<Task<((Rank, Suit)[], double)>>();
        var taskFactory = new TaskFactory();

        for (int i = MaxRank; i > MinRank; i--)
        {
            for (int j = i; j > MinRank; j--)
            {
                for (int x = MaxSuit; x > MinSuit; x--)
                {
                    for (int y = x; y > MinSuit; y--)
                    {
                        if (i == j && x == y)
                        {
                            continue;
                        }

                        (Rank, Suit)[] hand =
                        [
                            ((Rank)i, (Suit)x),
                            ((Rank)j, (Suit)y)
                        ];
                        
                        threads.Add(taskFactory.StartNew(() =>
                        {
                            var (win, draw, _) = simulator.SimulateWinChance(hand, [100], [])
                                .GetAwaiter()
                                .GetResult();

                            return (hand, Math.Round(win + draw, 0));
                        }));
                    }
                }
            }
        }

        ((Rank, Suit)[], double)[] result = await Task.WhenAll(threads);

        return [.. result];
    }
}
