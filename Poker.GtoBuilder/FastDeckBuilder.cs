namespace Poker.GtoBuilder;

public class FastDeckBuilder : IDeckBuilder
{
    public IDeck Build(int seed)
    {
        return new FastDeck(seed);
    }
}
