namespace I18nApi.Application.Common.Interfaces.Persistence;

public interface IDataRepository
{
    List<Dictionary<string, string?>> BulkInsert(List<Dictionary<string, string?>> data);
}