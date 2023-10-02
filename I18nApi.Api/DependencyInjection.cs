using System.Reflection;
using I18nApi.Api.Common.Errors;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace I18nApi.Api;

/// <summary>
/// Configure dependency injection of Presentation Layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddLocalization();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMappings();
        services.AddSingleton<ProblemDetailsFactory, I18NApiProblemsDetailsFactory>();
        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;

        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);

        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    public static IApplicationBuilder UseCultures(this IApplicationBuilder builder, WebApplication app)
    {
        string[] supportedCultures = new[] { "pt-BR", "en-US" };
        RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

        return builder;
    }
}
