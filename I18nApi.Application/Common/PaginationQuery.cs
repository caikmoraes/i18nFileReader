using MediatR;

namespace I18nApi.Application.Common;

public abstract class PaginationQuery<TResult>: IRequest<TResult>
{
    public int Page { get; set; }
    public int Size { get; set; } = 20;
    public string? OrderBy { get; set; }
    public string? OrderByDesc { get; set; }
}