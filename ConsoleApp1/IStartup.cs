using Microsoft.Extensions.DependencyInjection;

namespace GtoTrainer;

internal interface IStartup
{
    void ConfigureServices(IServiceCollection services);
}