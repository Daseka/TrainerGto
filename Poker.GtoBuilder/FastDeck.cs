using Poker.Common;

namespace Poker.GtoBuilder;

public class FastDeck : IDeck
{
    private const long AllCardsInDeck = 0x00_0F_FF_FF_FF_FF_FF_FF;

    private const int Ace = 0x00_00_00_00_00_00_00_01;
    private const int Eight = 0x00_00_00_00_00_00_00_80;
    private const int Five = 0x00_00_00_00_00_00_00_10;
    private const int Four = 0x00_00_00_00_00_00_00_08;
    private const int Jack = 0x00_00_00_00_00_00_04_00;
    private const int King = 0x00_00_00_00_00_00_10_00;
    private const int Nine = 0x00_00_00_00_00_00_01_00;
    private const int Queen = 0x00_00_00_00_00_00_08_00;
    private const int Seven = 0x00_00_00_00_00_00_00_40;
    private const int Six = 0x00_00_00_00_00_00_00_20;
    private const int Ten = 0x00_00_00_00_00_00_02_00;
    private const int Three = 0x00_00_00_00_00_00_00_04;
    private const int Two = 0x00_00_00_00_00_00_00_02;

    private const int CardCount = 52;

    /// <summary>
    /// This table is equivalent to 1UL left shifted by the index.
    /// The lookup is faster than the left shift operator.
    /// </summary>
    private static readonly ulong[] _cardMasksTable =
    {
            0x1,
            0x2,
            0x4,
            0x8,
            0x10,
            0x20,
            0x40,
            0x80,
            0x100,
            0x200,
            0x400,
            0x800,
            0x1000,
            0x2000,
            0x4000,
            0x8000,
            0x10000,
            0x20000,
            0x40000,
            0x80000,
            0x100000,
            0x200000,
            0x400000,
            0x800000,
            0x1000000,
            0x2000000,
            0x4000000,
            0x8000000,
            0x10000000,
            0x20000000,
            0x40000000,
            0x80000000,
            0x100000000,
            0x200000000,
            0x400000000,
            0x800000000,
            0x1000000000,
            0x2000000000,
            0x4000000000,
            0x8000000000,
            0x10000000000,
            0x20000000000,
            0x40000000000,
            0x80000000000,
            0x100000000000,
            0x200000000000,
            0x400000000000,
            0x800000000000,
            0x1000000000000,
            0x2000000000000,
            0x4000000000000,
            0x8000000000000,
        };

    private static readonly (Rank, Suit)[] _indexToCard =
    {
        (Rank.Ace, Suit.Diamond), (Rank.Two, Suit.Diamond), (Rank.Three, Suit.Diamond), (Rank.Four, Suit.Diamond),(Rank.Five, Suit.Diamond), (Rank.Six, Suit.Diamond), (Rank.Seven, Suit.Diamond), (Rank.Eight, Suit.Diamond),(Rank.Nine, Suit.Diamond), (Rank.Ten, Suit.Diamond), (Rank.Jack, Suit.Diamond), (Rank.Queen, Suit.Diamond), (Rank.King, Suit.Diamond),
        (Rank.Ace, Suit.Hart), (Rank.Two, Suit.Hart), (Rank.Three, Suit.Hart), (Rank.Four, Suit.Hart),(Rank.Five, Suit.Hart), (Rank.Six, Suit.Hart), (Rank.Seven, Suit.Hart), (Rank.Eight, Suit.Hart),(Rank.Nine, Suit.Hart), (Rank.Ten, Suit.Hart), (Rank.Jack, Suit.Hart), (Rank.Queen, Suit.Hart), (Rank.King, Suit.Hart),
        (Rank.Ace, Suit.Club), (Rank.Two, Suit.Club), (Rank.Three, Suit.Club), (Rank.Four, Suit.Club),(Rank.Five, Suit.Club), (Rank.Six, Suit.Club), (Rank.Seven, Suit.Club), (Rank.Eight, Suit.Club),(Rank.Nine, Suit.Club), (Rank.Ten, Suit.Club), (Rank.Jack, Suit.Club), (Rank.Queen, Suit.Club), (Rank.King, Suit.Club),
        (Rank.Ace, Suit.Spade), (Rank.Two, Suit.Spade), (Rank.Three, Suit.Spade), (Rank.Four, Suit.Spade),(Rank.Five, Suit.Spade), (Rank.Six, Suit.Spade), (Rank.Seven, Suit.Spade), (Rank.Eight, Suit.Spade),(Rank.Nine, Suit.Spade), (Rank.Ten, Suit.Spade), (Rank.Jack, Suit.Spade), (Rank.Queen, Suit.Spade), (Rank.King, Suit.Spade)
    };

    private static readonly ulong[] _rankToHex = [0x00, Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King,];

    private static readonly int[] _suitToShift = [0, 0, 13, 26, 39,];
    private readonly ulong[] _hexToRank = new ulong[4097];
    private readonly Random _randomizer;
    private ulong _cards = AllCardsInDeck;

    public FastDeck(int? seed = null)
    {
        _randomizer = seed is null
            ? new Random()
            : new Random(seed.Value);

        _hexToRank[Ace] = 1;
        _hexToRank[Two] = 2;
        _hexToRank[Three] = 3;
        _hexToRank[Four] = 4;
        _hexToRank[Five] = 5;
        _hexToRank[Six] = 6;
        _hexToRank[Seven] = 7;
        _hexToRank[Eight] = 8;
        _hexToRank[Nine] = 9;
        _hexToRank[Ten] = 10;
        _hexToRank[Jack] = 11;
        _hexToRank[Queen] = 12;
        _hexToRank[King] = 13;
    }

    public bool CanDeal((Rank, Suit) cardToDeal)
    {
        var cardHex = CardToHex(cardToDeal);

        return (_cards & cardHex) > 0;
    }

    public (Rank, Suit)[] GetRemaining()
    {
        throw new NotImplementedException();
    }

    public (Rank, Suit)[] Peek(int count)
    {
        var cards = new (Rank, Suit)[count];

        for (int i = 0; i < count; i++)
        {
            int index;
            do
            {
                index = _randomizer.Next(CardCount);
            } while ((_cardMasksTable[index] & _cards) == 0);

            cards[i] = _indexToCard[index];
        }

        return cards;
    }

    public void Reset()
    {
        _cards = AllCardsInDeck;
    }

    public void Shuffle()
    {
        //does nothing
    }

    public bool TryDeal((Rank, Suit) cardToDeal, out (Rank, Suit) cardDealt)
    {
        cardDealt = (Rank.None, Suit.None);

        if (!CanDeal(cardToDeal))
        {
            return false;
        }

        cardDealt = cardToDeal;
        // We are using XOR to remove the card from the deck
        _cards ^= CardToHex(cardToDeal);

        return true;
    }

    public bool TryDeal(out (Rank, Suit)? cardDealt)
    {
        throw new NotImplementedException();
    }

    private ulong CardToHex((Rank rank, Suit suit) cardToDeal)
    {
        return _rankToHex[(int)cardToDeal.rank] << _suitToShift[(int)cardToDeal.suit];
    }

    private (Rank, Suit) HexToCard(ulong mask)
    {
        for (int i = 51; i >= 0; i--)
        {
            if ((1UL << i & mask) != 0)
            {
                return _indexToCard[i];
            }
        }

        throw new ArgumentException("shouldnt reach here");
    }
}