using GtoTrainer.Trainers;
using Poker.GameReader.Reporters;

internal class Program
{
    public async static Task Main(string[] args)
    {
        await ConsoleTrainer.RunConsoleTrainer();
    }
}