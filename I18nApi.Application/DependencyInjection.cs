﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace I18nApi.Application;

/// <summary>
/// Configure dependency injection of Application Layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        return services;
    }
}
