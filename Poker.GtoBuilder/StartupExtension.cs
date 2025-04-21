using Microsoft.Extensions.DependencyInjection;

namespace Poker.GtoBuilder
{
    public static class StartupExtension
    {
        public static IServiceCollection AddGTOBuilderServices(this IServiceCollection services)
        {
            services
                .AddTransient<IDeckBuilder, FastDeckBuilder>()
                .AddTransient<IHandScorer, FastHandScorer>();

            return services;
        }
    }
}

