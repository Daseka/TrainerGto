using Poker.Common;

namespace Poker.GtoBuilder.GameSims;

public class Hand
{
    public Card FirstCard { get; set; }
    public bool IsSuited { get; set; }
    public Card SecondCard { get; set; }
    public double WinPercentage { get; set; }

    public Hand(Card first, Card second, double winPercentage = 0.0)
    {
        WinPercentage = winPercentage;

        if (first.Suit == second.Suit)
        {
            IsSuited = true;
        }

        if (first.Rank == Rank.Ace || first.Rank > second.Rank && second.Rank != Rank.Ace)
        {
            FirstCard = first;
            SecondCard = second;
        }
        else
        {
            FirstCard = second;
            SecondCard = first;
        }
    }

    override public string ToString()
    {
        var firstCard = FirstCard?.ToString() ?? "No Card";
        var secondCard = SecondCard?.ToString() ?? "No Card";
        return $"{firstCard}{secondCard}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Hand hand
            && hand.FirstCard.Rank == FirstCard.Rank
            && hand.SecondCard.Rank == SecondCard.Rank
            && hand.IsSuited == IsSuited;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstCard?.Rank, IsSuited);
    }
}