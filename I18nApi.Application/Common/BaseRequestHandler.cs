using System.Reflection;
using ErrorOr;
using I18nApi.Application.Common.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace I18nApi.Application.Common;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    protected readonly IGlobalizationService I18n;
    protected readonly ILogger<BaseRequestHandler<TRequest, TResponse>> Logger;

    protected BaseRequestHandler(IGlobalizationService i18N, ILogger<BaseRequestHandler<TRequest, TResponse>> logger)
    {
        I18n = i18N;
        Logger = logger;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

    protected Error Error(Type errorType, string errorCode)
    {
        MethodInfo? methodInfo = errorType.GetMethod(errorCode, BindingFlags.Public | BindingFlags.Static);

        if (methodInfo is null) throw new ArgumentNullException("errorCode", I18n["NoError"]);

        return (Error)methodInfo?.Invoke(null, new object[] { I18n[errorCode] })!;
    }
}