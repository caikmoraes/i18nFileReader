using Microsoft.AspNetCore.Http;

namespace I18nApi.Infrastructure.Common.Interfaces;

public interface IFileReaderHandler
{
    IFileReaderHandler SetNext(IFileReaderHandler handler);
        
    IList<Dictionary<string, string?>> Handle(IFormFile file);
}