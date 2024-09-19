using Poker.GameReader.Reporters;

namespace Poker.GameReader.Strategies;

public abstract class BaseStrategy : IStrategy
{
    private readonly Dictionary<char, int> _leterToSymbol = new()
    {
        {'A',CardSymbol.Ace},
        {'K',CardSymbol.King},
        {'Q',CardSymbol.Queen},
        {'J',CardSymbol.Jack},
        {'T',CardSymbol.Ten},
        {'9',CardSymbol.Nine},
        {'8',CardSymbol.Eight},
        {'7',CardSymbol.Seven},
        {'6',CardSymbol.Six},
        {'5',CardSymbol.Five},
        {'4',CardSymbol.Four},
        {'3',CardSymbol.Three},
        {'2',CardSymbol.Two},
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

                var left = _leterToSymbol[trimmed[0]];
                var righ = _leterToSymbol[trimmed[2]];
                var suited = trimmed[1] == trimmed[3]
                    ? Suited.Same
                    : Suited.Offs;

                raise[(left, righ, suited)] = roundedPercentage;
            }
        }

        return raise;
    }
}