using I18nApi.Application.Common.Interfaces.Services;
using I18nApi.Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace I18nApi.Infrastructure.Services;

public class FileReader : IFileReader
{
    private readonly ILogger<FileReader> _logger;
    private readonly IFileReaderHandler _readerHandler;

    public FileReader(ILogger<FileReader> logger)
    {
        _logger = logger;
        IFileReaderHandler readerHandler = new ParallelExcelFileReaderHandler();
        readerHandler.SetNext(new CsvFileReader());
        _readerHandler = readerHandler;
    }

    public IList<Dictionary<string, string?>> ReadFile(IFormFile file)
    {
        _logger.LogInformation("Start reading file");
        IList<Dictionary<string, string?>> result = _readerHandler.Handle(file);
        _logger.LogInformation("End reading file");
        return result;
    }

}