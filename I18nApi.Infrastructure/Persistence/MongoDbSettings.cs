namespace I18nApi.Infrastructure.Persistence;

public class MongoDbSettings
{
    public const string SectionName = "MongoDB";
    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
    public required string Collection { get; init; }
}