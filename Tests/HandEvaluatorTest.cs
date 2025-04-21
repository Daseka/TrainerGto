using HandEvaluator;
using Poker.Common;
using Poker.GtoBuilder;
using System.Diagnostics;

namespace Tests;

public class HandEvaluatorTest
{
    [Fact]
    public void Test()
    {
        string[] pocketCards =
        [
            "3s Qc",
            "Qh 4h",
        ];

        string board = "Th 5h Qd";

        var wins = new long[ pocketCards.Length ];
        var ties = new long[ pocketCards.Length ];
        var loss = new long[ pocketCards.Length ];

        long total = 0;

        Hand.HandOdds(pocketCards, board, string.Empty, wins, ties, loss, ref total);

        for (int i = 0; i < pocketCards.Length; i++)
        {
            double winPercent = ((double)wins[i] / total) * 100;
            double tiePercent = ((double)ties[i] / total) * 100;
            double lossPercent = ((double)loss[i] / total) * 100;
        }
    }
}