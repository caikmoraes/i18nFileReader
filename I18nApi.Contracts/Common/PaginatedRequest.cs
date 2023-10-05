namespace I18nApi.Contracts.Common;

public abstract class PaginatedRequest
{
    public int Page { get; set; }
    public int Size { get; set; } = 20;
    public string? OrderBy { get; set; }
    public string? OrderByDesc { get; set; }
}