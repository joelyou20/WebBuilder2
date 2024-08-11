using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;

namespace WebBuilder2.Client.Services;

public class LogService : ILogService
{
    private ILogClient _logClient;

    public LogService(ILogClient logClient) 
    {
        _logClient = logClient;
    }

    public async Task<LogModel?> AddLogAsync(Exception ex)
    {
        var log = new LogModel
        {
            Message = ex.Message,
            StackTrace = ex?.StackTrace,
            Exception = ex?.ToString() ?? "",
            Type = LogType.Error,
        };

        return await AddLogAsync(log);
    }

    public async Task<LogModel?> AddLogAsync(LogModel log) => await _logClient.AddLogAsync(log);

    public async Task<IEnumerable<LogModel>?> AddLogsAsync(IEnumerable<LogModel> logs) => await _logClient.AddLogsAsync(logs);

    public async Task<IEnumerable<LogModel>?> GetLogsAsync() => await _logClient.GetLogsAsync();

    public async Task<LogModel?> GetSingleLogAsync(long id) => await _logClient.GetSingleLogAsync(id);

    public async Task<LogModel?> SoftDeleteLogAsync(LogModel log) => await _logClient.SoftDeleteLogAsync(log);

    public async Task<LogModel?> UpdateLogAsync(LogModel log) => await _logClient.UpdateLogAsync(log);
}
