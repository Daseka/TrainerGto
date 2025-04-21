using GtoTrainer;
using GtoTrainer.Trainers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    public static async Task Main(string[] args)
    {
        await Start();
    }

    private static async Task Start()
    {
        var host = new HostBuilder()
                    .AddStartup<Startup>()
                    .Build();

        var consoleTrainer = host.Services.GetRequiredService<IConsoleTrainer>();

        await consoleTrainer.RunTrainer();
    }
}