using I18nApi.Application.Common.Interfaces.Services;
using I18nApi.Domain.Resources;
using Microsoft.Extensions.Localization;

namespace I18nApi.Infrastructure.Services;

public class GlobalizationService : IGlobalizationService
{
    private IStringLocalizer<Messages> _localizer;

    public GlobalizationService(IStringLocalizer<Messages> localizer)
    {
        _localizer = localizer;
    }

    public string this[string key]
    {
        get
        {
            return _localizer[key].Value;
        }
    }
}