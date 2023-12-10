using Serilog;
using WebBuilder2.Client.Clients;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services;

public class LogService : ILogService
{
    private ILogClient _logClient;
    private IErrorObserver _errorObserver;

    public LogService(ILogClient logClient, IErrorObserver errorObserver) 
    {
        _logClient = logClient;
        _errorObserver = errorObserver;
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

    public async Task<LogModel?> AddLogAsync(LogModel log)
    {
        ValidationResponse<LogModel> result = await _logClient.AddLogAsync(log);

        return GetValue(result)?.SingleOrDefault();
    }

    public async Task<List<LogModel>?> AddLogsAsync(IEnumerable<LogModel> logs)
    {
        ValidationResponse<LogModel> result = await _logClient.AddLogsAsync(logs);

        return GetValue(result)?.ToList();
    }

    public async Task<List<LogModel>?> GetLogsAsync()
    {
        ValidationResponse<LogModel> result = await _logClient.GetLogsAsync();

        return GetValue(result)?.ToList();
    }

    public async Task<LogModel?> GetSingleLogAsync(long id)
    {
        ValidationResponse<LogModel> result = await _logClient.GetSingleLogAsync(id);

        return GetValue(result)?.SingleOrDefault();
    }

    public async Task<LogModel?> SoftDeleteLogAsync(LogModel log)
    {
        ValidationResponse<LogModel> result = await _logClient.SoftDeleteLogAsync(log);

        return GetValue(result)?.SingleOrDefault();
    }

    public async Task<LogModel?> UpdateLogAsync(LogModel log)
    {
        ValidationResponse<LogModel> result = await _logClient.UpdateLogAsync(log);

        return result.GetValues()?.SingleOrDefault();
    }

    private IEnumerable<LogModel>? GetValue(ValidationResponse<LogModel> value)
    {
        if (!value.IsSuccessful)
        {
            _errorObserver.AddErrorRange(value.Errors);
            return null;
        }
        else
        {
            return value.GetValues();
        }
    }
}
