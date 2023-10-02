namespace I18nApi.Application.Common.Interfaces.Services;

public interface IGlobalizationService
{
    string this[string key] { get; }
}