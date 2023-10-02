using I18nApi.Application.Common.Interfaces.Persistence;
using I18nApi.Application.Common.Interfaces.Services;
using I18nApi.Infrastructure.Persistence;
using I18nApi.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace I18nApi.Infrastructure;

/// <summary>
/// Configure dependency injection of Infrastructure Layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<MongoDbSettings>()
            .Bind(config.GetSection(MongoDbSettings.SectionName));

        services.AddSingleton<IGlobalizationService, GlobalizationService>();
        services.AddSingleton<IFileReader, FileReader>();
        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDataRepository, DataRepository>();

        return services;
    }
}
