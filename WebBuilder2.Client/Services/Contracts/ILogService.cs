using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services.Contracts;

public interface ILogService
{
    Task<List<LogModel>?> GetLogsAsync();
    Task<LogModel?> GetSingleLogAsync(long id);
    Task<LogModel?> AddLogAsync(LogModel log);
    Task<LogModel?> AddLogAsync(Exception ex);
    Task<List<LogModel>?> AddLogsAsync(IEnumerable<LogModel> logs);
    Task<LogModel?> SoftDeleteLogAsync(LogModel log);
    Task<LogModel?> UpdateLogAsync(LogModel log);
}
