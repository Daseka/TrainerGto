using Poker.Common;

namespace Poker.GtoBuilder;

public class Deck
{
    private const int CardQuantity = 52;
    private const int HighestRank = 13;
    private int[][] _cards;
    private Random _randomizer;
    private int _startIndexOfCardsDealt;

    public Deck(int? seed = null)
    {
        _randomizer = seed is null
            ? new Random()
            : new Random(seed.Value);

        _startIndexOfCardsDealt = CardQuantity - 1;
        _cards = new int[CardQuantity][];

        InitializeCards(_cards);
    }

    public (Rank, Suit)[] GetRemaining()
    {
        return Peek(_startIndexOfCardsDealt + 1);
    }

    public (Rank, Suit)[] Peek(int count)
    {
        count = Math.Min(CardQuantity, count);

        var cardsLeftInDeck = new (Rank, Suit)[count];
        for (int i = 0; i < count; i++)
        {
            cardsLeftInDeck[i] = ((Rank)_cards[i][0], (Suit)_cards[i][1]);
        }

        return cardsLeftInDeck;
    }

    public void Reset()
    {
        _startIndexOfCardsDealt = CardQuantity - 1;
    }

    public void Shuffle()
    {
        var span = new Span<int[]>(_cards, 0, _startIndexOfCardsDealt);
        _randomizer.Shuffle(span);
    }

    public bool TryDeal((Rank, Suit) cardToDeal, out (Rank, Suit) cardDealt)
    {
        for (int i = 0; i <= _startIndexOfCardsDealt; i++)
        {
            var currentCard = ((Rank)_cards[i][0], (Suit)_cards[i][1]);
            if (currentCard == cardToDeal)
            {
                cardDealt = currentCard;
                
                (_cards[_startIndexOfCardsDealt], _cards[i]) = (_cards[i], _cards[_startIndexOfCardsDealt]);
                _startIndexOfCardsDealt--;

                return true;
            }
        }

        cardDealt = (Rank.None, Suit.None);

        return false;
    }

    public bool TryDeal(out (Rank, Suit)? cardDealt)
    {
        cardDealt = null;
        if (_startIndexOfCardsDealt < 1)
        {
            return false;
        }

        cardDealt = ((Rank)_cards[_startIndexOfCardsDealt][0], (Suit)_cards[_startIndexOfCardsDealt][1]);
        _startIndexOfCardsDealt--;

        return true;
    }

    private static void InitializeCards(int[][] cards)
    {
        for (int i = 0; i < CardQuantity; i++)
        {
            int rank = i % HighestRank + 1;
            int suit = i / HighestRank + 1;

            cards[i] = [rank, suit];
        }
    }
}