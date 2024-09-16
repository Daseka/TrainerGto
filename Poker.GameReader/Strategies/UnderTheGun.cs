using Poker.GameReader.Reporters;
using static Poker.GameReader.Reporters.CardSymbol;

namespace Poker.GameReader.Strategies;

public static class UnderTheGun
{
    public static Dictionary<(int, int, Suited), (double fold, double raise, double call)> Strategy = new()
    {
        //Pairs
        {(Ace,Ace, Suited.Pair), (0.0,1.0,0.0)}, {(King,King, Suited.Pair), (0.0,1.0,0.0)}, {(Queen,Queen, Suited.Pair), (0.0,1.0,0.0)},
        {(Jack,Jack, Suited.Pair), (0.0,1.0,0.0)}, {(Ten,Ten, Suited.Pair), (0.0,1.0,0.0)}, {(Nine,Nine, Suited.Pair), (0.0,1.0,0.0)},
        {(Eight,Eight, Suited.Pair), (0.0,1.0,0.0)}, {(Seven,Seven, Suited.Pair), (0.0,1.0,0.0)}, {(Six,Six, Suited.Pair), (0.0,1.0,0.0)},
        {(Five,Five, Suited.Pair), (0.15,0.85,0.0)}, {(Four,Four, Suited.Pair), (0.81,0.19,0.0)}, {(Three,Three, Suited.Pair), (0.91,0.09,0.0)},   


        //Suited 
        {(Ace,King, Suited.Same), (0.0,1.0,0.0)},       {(Ace,Queen, Suited.Same), (0.0,1.0,0.0)},      {(Ace,Jack, Suited.Same), (0.0,1.0,0.0)},
        {(Ace,Ten, Suited.Same), (0.0,1.0,0.0)},        {(Ace,Nine, Suited.Same), (0.0,1.0,0.0)},       {(Ace,Eight, Suited.Same), (0.0,1.0,0.0)},
        {(Ace,Seven, Suited.Same), (0.0,1.0,0.0)},      {(Ace,Six, Suited.Same), (0.0,1.0,0.0)},        {(Ace,Five, Suited.Same), (0.0,1.0,0.0)},
        {(Ace,Four, Suited.Same), (0.0,1.0,0.0)},       {(Ace,Three, Suited.Same), (0.0,1.0,0.0)},      {(Ace,Two, Suited.Same), (0.0,1.0,0.0)},

        {(King,Queen, Suited.Same), (0.0,1.0,0.0)},     {(King,Jack, Suited.Same), (0.0,1.0,0.0)},      {(King,Ten, Suited.Same), (0.0,1.0,0.0)},
        {(King,Nine, Suited.Same), (0.0,1.0,0.0)},      {(King,Eight, Suited.Same), (0.0,1.0,0.0)},     {(King,Seven, Suited.Same), (0.0,1.0,0.0)},
        {(King,Six, Suited.Same), (0.68,0.32,0.0)},     {(King,Five, Suited.Same), (0.46,0.54,0.0)},

        {(Queen,Jack, Suited.Same), (0.0,1.0,0.0)},     {(Queen,Ten, Suited.Same), (0.0,1.0,0.0)},      {(Queen,Nine, Suited.Same), (0.0,1.0,0.0)},

        {(Jack,Ten, Suited.Same), (0.0,1.0,0.0)},       {(Jack,Nine, Suited.Same), (0.46,0.53,0.0)},

        {(Ten,Nine, Suited.Same), (0.0,1.0,0.0)},       {(Ten,Eight, Suited.Same), (0.69,0.31,0.0)},

        {(Nine,Eight, Suited.Same), (0.53,0.47,0.0)},

        {(Eight,Seven, Suited.Same), (0.78,0.22,0.0)},

        {(Seven,Six, Suited.Same), (0.75,0.25,0.0)},

        {(Six,Five, Suited.Same), (0.74,0.26,0.0)},

        {(Five,Four, Suited.Same), (0.75,0.25,0.0)},    


        //OffSuit
        {(Ace,King, Suited.Offs), (0.0,1.0,0.0)},       {(Ace,Queen, Suited.Offs), (0.0,1.0,0.0)},      {(Ace,Jack, Suited.Offs), (0.0,1.0,0.0)},
        {(Ace,Ten, Suited.Offs), (0.0,1.0,0.0)},

        {(King,Queen, Suited.Offs), (0.0,1.0,0.0)},     {(King,Jack, Suited.Offs), (0.03,0.97,0.0)},    {(King,Ten, Suited.Offs), (0.69,0.31,0.0)},

        {(Queen,Jack, Suited.Offs), (0.71,0.29,0.0)},
    };

    public static StrategySolution GetSolution(GameData gameData)
    {
        Suited suited = GetSuitedState(gameData);

        (int, int, Suited) result = (gameData.HandCards[0].cardSymbol, gameData.HandCards[1].cardSymbol, suited);

        if (Strategy.TryGetValue(result, out var value))
        {
            return new StrategySolution
            {
                Raise = value.raise,
                Call = value.call,
                Fold = value.fold,
            };
        }

        return new StrategySolution
        {
            Raise = 0,
            Call = 0,
            Fold = 1,
        };
    }

    private static Suited GetSuitedState(GameData gameData)
    {
        Suited suited;
        if (gameData.HandCards[0].cardSymbol == gameData.HandCards[1].cardSuit)
        {
            suited = Suited.Pair;
        }
        else if (gameData.HandCards[0].cardSuit == gameData.HandCards[1].cardSuit)
        {
            suited = Suited.Same;
        }
        else
        {
            suited = Suited.Offs;
        }

        return suited;
    }
}