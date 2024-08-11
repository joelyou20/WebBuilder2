using System.Text;
using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Utils.Providers;

// https://stackoverflow.com/questions/78326286/show-details-of-uncaught-exception-in-blazor-ui/78326500#78326500
public class UnhandledExceptionProvider : ILoggerProvider
{
    public event Action<LogLevel, Exception?>? Log;

    public UnhandledExceptionProvider() { }

    public void Setup(IErrorObserver errorObserver)
    {
        Log += (loglevel, exception) => HandleException(loglevel, exception, errorObserver);
    }

    private void HandleException(LogLevel logLevel, Exception? exception, IErrorObserver errorObserver)
    {
        ApiErrorSeverity ConvertLogLevelToSeverity(LogLevel ll) => ll switch
        {
            LogLevel.Error => ApiErrorSeverity.Error,
            LogLevel.Critical => ApiErrorSeverity.Error,
            LogLevel.Warning => ApiErrorSeverity.Warning,
            LogLevel.Information => ApiErrorSeverity.Info,
            _ => ApiErrorSeverity.Normal,
        };

        if (exception != null)
        {
            string stackTrace = exception.StackTrace != null ?
                Encoding.UTF8.GetString(Encoding.UTF32.GetBytes(exception.StackTrace)) :
                string.Empty;

            string errorDetail = exception.Message;

            errorObserver.AddError(new ApiError
            {
                Severity = ConvertLogLevelToSeverity(logLevel),
                Exception = exception,
                Message = errorDetail,
                StackTrace = stackTrace
            });
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new UnhandledExceptionLogger(this);
    }

    public void Dispose() { }

    private class UnhandledExceptionLogger(UnhandledExceptionProvider unhandledExceptionProvider) : ILogger
    {
        private readonly UnhandledExceptionProvider unhandledExceptionProvider = unhandledExceptionProvider;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Unhandled exceptions will call this method
            unhandledExceptionProvider.Log?.Invoke(logLevel, exception);
        }

        public IDisposable? BeginScope<TState>(TState state)
             where TState : notnull
        {
            return new EmptyDisposable();
        }

        private class EmptyDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
