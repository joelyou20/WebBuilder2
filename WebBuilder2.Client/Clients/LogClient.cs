using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class LogClient : ClientBase<LogModel>, ILogClient
{
    public LogClient(HttpClient client) : base(client, "log") { }
    public async Task<ValidationResponse<LogModel>> GetLogsAsync() => await GetAsync();
    public async Task<ValidationResponse<LogModel>> GetSingleLogAsync(long id) => await GetSingleAsync(id);
    public async Task<ValidationResponse<LogModel>> SoftDeleteLogAsync(LogModel log) => await SoftDeleteAsync(log);
    public async Task<ValidationResponse<LogModel>> AddLogAsync(LogModel log) => await AddAsync(log);
    public async Task<ValidationResponse<LogModel>> AddLogsAsync(IEnumerable<LogModel> logs) => await AddRangeAsync(logs);
    public async Task<ValidationResponse<LogModel>> UpdateLogAsync(LogModel log) => await UpdateAsync(log);
}
