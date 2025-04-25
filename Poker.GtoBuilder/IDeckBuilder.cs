namespace Poker.GtoBuilder
{
    public interface IDeckBuilder
    {
        IDeck Build(int seed);
    }
}