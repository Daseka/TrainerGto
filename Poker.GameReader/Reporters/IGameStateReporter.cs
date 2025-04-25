
namespace Poker.GameReader.Reporters
{
    public interface IGameStateReporter
    {
        bool ConnectToGame(string[] gameName);
        Task<GameData> GetGameState();
    }
}