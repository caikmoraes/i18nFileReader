using I18nApi.Contracts.Common;

namespace I18nApi.Contracts.Data;

public class GetDataRequest : PaginatedRequest
{
    public string? Property { get; set; }
    public string? Value { get; set; }
}