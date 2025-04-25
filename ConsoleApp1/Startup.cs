using GtoTrainer;
using GtoTrainer.Trainers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poker.GameReader;
using Poker.GtoBuilder;

internal class Startup : IStartup
{
    public static IConfiguration? Configuration { get; private set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IConsoleTrainer, ConsoleTrainer>();

        services
            .AddGameReaderServices()
            .AddGTOBuilderServices();
    }
}