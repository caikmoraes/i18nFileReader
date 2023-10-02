namespace I18nApi.Application.Upload.Common;

public record DataQueryResult<TData>(
    int Total,
    IList<TData> Data,
    int Page
);