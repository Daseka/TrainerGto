using Poker.GameReader.Reporters;

namespace Poker.GameReader.Strategies;

public abstract class BaseStrategy : IStrategy
{
    private readonly Dictionary<char, int> _leterToRank = new()
    {
        {'A',CardRank.Ace},
        {'K',CardRank.King},
        {'Q',CardRank.Queen},
        {'J',CardRank.Jack},
        {'T',CardRank.Ten},
        {'9',CardRank.Nine},
        {'8',CardRank.Eight},
        {'7',CardRank.Seven},
        {'6',CardRank.Six},
        {'5',CardRank.Five},
        {'4',CardRank.Four},
        {'3',CardRank.Three},
        {'2',CardRank.Two},
    };

    public abstract StrategySolution Solve(GameData gameData);

    protected static Suited GetSuitedState(GameData gameData)
    {
        Suited suited = gameData.HandCards[0].cardSuit == gameData.HandCards[1].cardSuit
            ? Suited.Same
            : Suited.Offs;

        return suited;
    }

    protected Dictionary<(int, int, Suited), double> BuildRange(string[] data)
    {
        var raise = new Dictionary<(int, int, Suited), double>();
        foreach (string item in data)
        {
            var percentageSymbol = item[1..].Split("]");
            double.TryParse(percentageSymbol[0], out double percentage);

            double roundedPercentage = Math.Round(percentage / 100, 2);

            var hands = percentageSymbol[1].Split(",");
            foreach (var hand in hands)
            {
                string trimmed = hand.Trim();

                var left = _leterToRank[trimmed[0]];
                var righ = _leterToRank[trimmed[2]];
                var suited = trimmed[1] == trimmed[3]
                    ? Suited.Same
                    : Suited.Offs;

                raise[(left, righ, suited)] = roundedPercentage;
            }
        }

        return raise;
    }
}