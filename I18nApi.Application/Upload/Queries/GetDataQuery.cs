using I18nApi.Application.Common;
using I18nApi.Application.Upload.Common;
using MediatR;

namespace I18nApi.Application.Upload.Queries;

public class GetDataQuery<TResult>: PaginationQuery<DataQueryResult<TResult>>
{
    
}