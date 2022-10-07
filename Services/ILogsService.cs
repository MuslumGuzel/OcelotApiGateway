using ApiGateway.Models;

public interface ILogsService {
    Task<IList<Logs>> GetAsync();
    Task<Logs?> GetAsync(string id);
    Task CreateAsync(Logs newLog);
    Task UpdateAsync(string id, Logs updatedLog);
    Task RemoveAsync(string id);
}