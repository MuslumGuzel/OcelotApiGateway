using ApiGateway.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiGateway.Services;

public class LogsService : ILogsService
{
    private readonly IMongoCollection<Logs> logsCollection;

    public LogsService(IOptions<LogDatabaseSettings> logDatabaseSettings) 
    {
        var mongoClient = new MongoClient(logDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(logDatabaseSettings.Value.DatabaseName);
        logsCollection = mongoDatabase.GetCollection<Logs>(logDatabaseSettings.Value.LogsCollectionName);
    }

    public async Task<IList<Logs>> GetAsync() => await logsCollection.Find(_ => true).ToListAsync();
    public async Task<Logs?> GetAsync(string id) => await logsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    public async Task CreateAsync(Logs newLog) => await logsCollection.InsertOneAsync(newLog);
    public async Task UpdateAsync(string id, Logs updatedLog) => await logsCollection.ReplaceOneAsync(x => x.Id == id, updatedLog);
    public async Task RemoveAsync(string id) => await logsCollection.DeleteOneAsync(x => x.Id == id);
}