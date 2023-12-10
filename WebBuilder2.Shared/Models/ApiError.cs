using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class ApiError
{
    public string? Message { get; } = string.Empty;
    public string? Code { get;} = string.Empty;
    public string? Resource { get; } = string.Empty;
    public string? Field { get; } = string.Empty; 
    public Exception? Exception { get; }
    public ApiErrorSeverity? Severity { get; }

    public ApiError(string? message = null, ApiErrorSeverity? severity = null, string? code = null, string? resource = null, string? field = null, Exception? exception = null)
    {
        Message = message;
        Severity = severity;
        Code = code;
        Resource = resource;
        Field = field;
        Exception = exception;
    }
}
