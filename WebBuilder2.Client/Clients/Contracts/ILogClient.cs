using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface ILogClient
{
    Task<IEnumerable<LogModel>> GetLogsAsync();
    Task<LogModel> GetSingleLogAsync(long id);
    Task<LogModel> SoftDeleteLogAsync(LogModel log);
    Task<LogModel> AddLogAsync(LogModel log);
    Task<IEnumerable<LogModel>> AddLogsAsync(IEnumerable<LogModel> logs);
    Task<LogModel> UpdateLogAsync(LogModel log);
}
