namespace Poker.GameReader.Reporters;

public readonly struct GameData
{
    public double[] Bets { get; init; }
    public double CallAmount { get; init; }
    public (int cardSymbol, int cardSuit)[] HandCards { get; init; }
    public (int cardSymbol, int cardSuit)[] MiddleCards { get; init; }
    public Position Position { get; init; }
    public double PotTotal { get; init; }

    public bool HasBeenRaised
    {
        get
        {
            int increase = 0;
            double currentAmount = 0;
            var bets = Bets.ToList();
            bets.Sort();

            for (int i = 0; i < bets.Count; i++)
            {
                if (currentAmount < bets[i])
                {
                    currentAmount = bets[i];
                    increase++;
                }
            }

            return increase > 2;
        }
    }
}
