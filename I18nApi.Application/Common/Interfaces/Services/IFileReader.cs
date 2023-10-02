using Microsoft.AspNetCore.Http;

namespace I18nApi.Application.Common.Interfaces.Services;

public interface IFileReader
{
    IList<Dictionary<string, string?>> ReadFile(IFormFile file);
}