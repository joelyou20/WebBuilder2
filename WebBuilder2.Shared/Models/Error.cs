using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class Error
{
    public string? Message { get; set; } = string.Empty;
    public string? Code { get; set; } = string.Empty;
    public string? Resource { get; set; } = string.Empty;
    public string? Field { get; set; } = string.Empty; 
    public Exception? Exception { get; set; }
    public ApiErrorSeverity? Severity { get; set; }

    public Error(string? message = null, ApiErrorSeverity? severity = null, string? code = null, string? resource = null, string? field = null, Exception? exception = null)
    {
        Message = message;
        Severity = severity;
        Code = code;
        Resource = resource;
        Field = field;
        Exception = exception;
    }

    public Error() { }
}
