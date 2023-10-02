using I18nApi.Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace I18nApi.Infrastructure.Common.Abstractions;

public abstract class BaseFileReaderHandler : IFileReaderHandler
{
    private IFileReaderHandler? _nextHandler;


    public IFileReaderHandler SetNext(IFileReaderHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }
        
    public virtual IList<Dictionary<string, string?>> Handle(IFormFile file)
    {
        return _nextHandler?.Handle(file) ?? throw new ArgumentException("Unsupported file format");
    }

    protected string GetFileExtension(IFormFile file)
    {
        return Path.GetExtension(file.FileName);
    }
}