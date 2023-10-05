using ErrorOr;
using I18nApi.Application.Common;
using I18nApi.Application.Common.Interfaces.Persistence;
using I18nApi.Application.Common.Interfaces.Services;
using I18nApi.Application.Upload.Common;
using Microsoft.Extensions.Logging;

namespace I18nApi.Application.Upload.Queries;

public class GetDataQueryHandler : BaseRequestHandler<GetDataQuery<ErrorOr<DataQueryResult<Dictionary<string, object>>>>,
    ErrorOr<DataQueryResult<Dictionary<string, object>>>>
{
    private readonly IDataRepository _dataRepository;

    public GetDataQueryHandler(IGlobalizationService i18N, ILogger<BaseRequestHandler<GetDataQuery<ErrorOr<DataQueryResult<Dictionary<string, object>>>>, ErrorOr<DataQueryResult<Dictionary<string, object>>>>> logger, IDataRepository dataRepository) : base(i18N, logger)
    {
        _dataRepository = dataRepository;
    }

    public override async Task<ErrorOr<DataQueryResult<Dictionary<string, object>>>> Handle(GetDataQuery<ErrorOr<DataQueryResult<Dictionary<string, object>>>> query, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Handling fetch data");

        IList<Dictionary<string, object>> data = _dataRepository.FindAll(query);

        return new DataQueryResult<Dictionary<string, object>>(
            Total: data.Count,
            Data: data,
            Page: query.Page
        );
    }
}