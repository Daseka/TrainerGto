using Microsoft.Extensions.DependencyInjection;
using Poker.GameReader.Reporters;
using Poker.GameReader.ScreenUtilities;
using Poker.GtoBuilder;

namespace Poker.GameReader;

public static class StartupExtension
{
    public static IServiceCollection AddGameReaderServices(this IServiceCollection services)
    {
        services
            .AddTransient<IScreenGrabber, ScreenGrabber>()
            .AddTransient<IGameStateReporter, GameStateReporter>()
            .AddTransient<IStrategyReporter, StrategyReporter>()
            .AddTransient<IHandSimulator, HandSimulator>();

        return services;
    }
}
