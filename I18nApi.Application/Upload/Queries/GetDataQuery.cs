using I18nApi.Application.Common;

namespace I18nApi.Application.Upload.Queries;

public class GetDataQuery<TResult>: PaginationQuery<TResult>
{
    public string? Property { get; set; }
    public string? Value { get; set; }
}