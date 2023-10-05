using I18nApi.Application.Upload.Queries;

namespace I18nApi.Application.Common.Interfaces.Persistence;

public interface IDataRepository
{
    List<Dictionary<string, string?>> BulkInsert(List<Dictionary<string, string?>> data);
    List<Dictionary<string, object>> FindAll<TResult>(GetDataQuery<TResult> query);
}