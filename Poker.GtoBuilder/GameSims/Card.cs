using Poker.Common;

namespace Poker.GtoBuilder.GameSims;

public class Card
{
    public Rank Rank { get; set; }
    public Suit Suit { get; set; }

    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    public override string ToString()
    {
        var rankChar = Rank switch
        {
            Rank.Ace => 'A',
            Rank.King => 'K',
            Rank.Queen => 'Q',
            Rank.Jack => 'J',
            Rank.Ten => 'T',
            _ => ((int)Rank).ToString()[0]
        };

        var suitChar = Suit switch
        {
            Suit.Spade => '♠',
            Suit.Hart => '♥',
            Suit.Diamond => '♦',
            _ => '♣',
        };

        return $"{rankChar}{suitChar}";
    }
}