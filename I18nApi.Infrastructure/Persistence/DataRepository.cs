using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using I18nApi.Application.Common.Interfaces.Persistence;
using I18nApi.Application.Upload.Queries;
using I18nApi.Domain.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace I18nApi.Infrastructure.Persistence;

public class DataRepository : BaseRepository, IDataRepository
{
    private readonly IMongoCollection<BsonDocument> _collection;

    public DataRepository(IOptions<MongoDbSettings> options, ILogger<DataRepository> logger) : base(logger)
    {
        MongoClient client = new(options.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<BsonDocument>(options.Value.Collection);
    }

    public List<Dictionary<string, string?>> BulkInsert(List<Dictionary<string, string?>> data)
    {
        Logger.LogInformation("Persisting data using bulk insert");
        List<BsonDocument> documents = DataToDocuments(data);

        List<BsonDocument>[] batchList = ListExtensions.SplitList<BsonDocument>(documents, 500).ToArray();

        Logger.LogInformation("Initializing threads");

        Parallel.For(0, batchList.Length, index =>
        {
            List<BsonDocument> batch = batchList[index];
            _collection.InsertMany(batch);
        });

        Logger.LogInformation("Data persisted");

        return data;
    }

    public List<Dictionary<string, object>> FindAll<TResult>(GetDataQuery<TResult> query)
    {
        FilterDefinition<BsonDocument>? filter = Builders<BsonDocument>.Filter.Empty;
        if (query.Property is not null)
        {
            filter |= Builders<BsonDocument>.Filter.Eq(query.Property, query.Value);
        }

        List<BsonDocument> data = new();
        if (query.OrderByDesc is not null)
        {
            data = _collection.Find(filter)
                .SortByDescending(x => x[query.OrderByDesc!])
                .Skip((query.Page - 1) * query.Size)
                .Limit(query.Size)
                .ToList();
        }
        else
        {
            data = _collection.Find(filter)
                .SortBy(x => String.IsNullOrEmpty(query.OrderBy) ? x[query.OrderBy] : x[0])
                .Skip((query.Page - 1) * query.Size)
                .Limit(query.Size)
                .ToList();
        }

        return data.ConvertAll(x => x.ToDictionary()).ToList();
    }

    private List<BsonDocument> DataToDocuments(List<Dictionary<string, string?>> data)
    {
        List<BsonDocument> documents = new();

        ref Dictionary<string, string?> start = ref MemoryMarshal.GetArrayDataReference(data.ToArray());
        ref Dictionary<string, string> end = ref Unsafe.Add(ref start, data.Count())!;
        while (Unsafe.IsAddressLessThan(ref start!, ref end))
        {
            BsonDocument doc = new(start);

            documents.Add(doc);

            start = ref Unsafe.Add(ref start, 1)!;
        }

        return documents;
    }
}