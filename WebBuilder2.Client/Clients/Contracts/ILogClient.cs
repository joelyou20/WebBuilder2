using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface ILogClient
{
    Task<ValidationResponse<LogModel>> GetLogsAsync();
    Task<ValidationResponse<LogModel>> GetSingleLogAsync(long id);
    Task<ValidationResponse<LogModel>> SoftDeleteLogAsync(LogModel log);
    Task<ValidationResponse<LogModel>> AddLogAsync(LogModel log);
    Task<ValidationResponse<LogModel>> AddLogsAsync(IEnumerable<LogModel> logs);
    Task<ValidationResponse<LogModel>> UpdateLogAsync(LogModel log);
}
