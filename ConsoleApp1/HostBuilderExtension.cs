using Microsoft.Extensions.Hosting;

namespace GtoTrainer;

internal static class HostBuilderExtension
{
    public static IHostBuilder AddStartup<T>(this HostBuilder builder) where T : IStartup
    {
        builder.ConfigureServices((context, services) =>
        {
            var startup = Activator.CreateInstance(typeof(T), context.Configuration) as IStartup;
            startup?.ConfigureServices(services);
        });

        return builder;
    }
}
