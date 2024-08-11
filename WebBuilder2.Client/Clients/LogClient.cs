using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class LogClient(HttpClient client) : ClientBase<LogModel>(client, "log"), ILogClient
{
    public async Task<IEnumerable<LogModel>> GetLogsAsync() => await GetAsync();
    public async Task<LogModel> GetSingleLogAsync(long id) => await GetSingleAsync(id);
    public async Task<LogModel> SoftDeleteLogAsync(LogModel log) => await SoftDeleteAsync(log);
    public async Task<LogModel> AddLogAsync(LogModel log) => await AddAsync(log);
    public async Task<IEnumerable<LogModel>> AddLogsAsync(IEnumerable<LogModel> logs) => await AddRangeAsync(logs);
    public async Task<LogModel> UpdateLogAsync(LogModel log) => await UpdateAsync(log);
}
