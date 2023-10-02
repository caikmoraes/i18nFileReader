using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using I18nApi.Application.Common.Interfaces.Persistence;
using I18nApi.Domain.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace I18nApi.Infrastructure.Persistence;

public class DataRepository: BaseRepository, IDataRepository
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