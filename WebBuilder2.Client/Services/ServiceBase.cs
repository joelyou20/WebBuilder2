using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services;

public class ServiceBase
{
    private IErrorObserver _errorObserver;
    private ILogService _logService;

    public ServiceBase(IErrorObserver errorObserver, ILogService logService)
    {
        _errorObserver = errorObserver;
        _logService = logService;
    }

    public async Task ExecuteAsync(Func<Task<ValidationResponse>> func)
    {
        var result = await func();
        if (!result.IsSuccessful)
        {
            _errorObserver.AddErrorRange(result.Errors);
            await AddLogsAsync(result.Errors);
        }
    }

    public async Task<IEnumerable<T>?> ExecuteAsync<T>(Func<Task<ValidationResponse<T>>> func) where T : class
    {
        var result = await func();
        if(!result.IsSuccessful)
        {
            _errorObserver.AddErrorRange(result.Errors);
            await AddLogsAsync(result.Errors);
            return null;
        }
        else
        {
            return result.GetValues();
        }
    }

    private async Task AddLogsAsync(IEnumerable<ApiError> errors)
    {
        IEnumerable<LogModel> logs = errors.Select(e => new LogModel
        {
            Message = e.Message,
            StackTrace = e.Exception?.StackTrace,
            Exception = e.Exception?.GetType().ToString() ?? "",
            Type = LogType.Error
        });

        //await _logService.AddLogsAsync(logs);
    }
}
