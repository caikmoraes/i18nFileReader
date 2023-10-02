using ErrorOr;
using I18nApi.Application.Common;
using I18nApi.Application.Common.Interfaces.Persistence;
using I18nApi.Application.Common.Interfaces.Services;
using I18nApi.Application.Upload.Common;
using I18nApi.Domain.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace I18nApi.Application.Upload.Commands;

public class UploadFileCommandHandler : BaseRequestHandler<UploadFileCommand, ErrorOr<UploadResult>>
{
    private readonly IFileReader _fileReader;
    private readonly IDataRepository _repository;
    public UploadFileCommandHandler(IGlobalizationService i18N, ILogger<UploadFileCommandHandler> logger, IFileReader fileReader, IDataRepository repository) : base(i18N, logger)
    {
        _fileReader = fileReader;
        _repository = repository;
    }

    public override async Task<ErrorOr<UploadResult>> Handle(UploadFileCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (command.File is null)
            return Error(typeof(Errors.UploadErrors), nameof(Errors.UploadErrors.NoFile));
        
        
        Logger.LogInformation("Handling file upload: {fileName}", command.File.FileName);

        IList<Dictionary<string, string?>> dataList;
        try
        {
            dataList = _fileReader.ReadFile(command.File);
        }
        catch (ArgumentException e)
        {
            Logger.LogError("An error occurred reading file: {error}", e);
            return Error(typeof(Errors.UploadErrors), nameof(Errors.UploadErrors.UnsupportedType));
        }
        try
        {
            _repository.BulkInsert(dataList.ToList());
        }
        catch (Exception e)
        {
            Logger.LogError("An error occurred persisting data: {error}", e);
            return Error(typeof(Errors.PersistenceErrors), nameof(Errors.PersistenceErrors.PersistenceFailure));
        }
        int dataCount = dataList.Count;
        Logger.LogInformation("Total of {total} lines were read", dataCount);

        return new UploadResult(I18n["UploadSuccess"]);
    }

    
}