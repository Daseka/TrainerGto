namespace Poker.GtoBuilder;

public class DeckBuilder : IDeckBuilder
{
    public IDeck Build(int seed)
    {
        return new Deck(seed);
    }
}
