using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Observers;

public class ErrorObserver : IErrorObserver
{
    public event EventHandler<List<ApiError>> ErrorsChanged = default!;

    protected List<ApiError> Errors { get; private set; } = new();

    public void AddErrorRange(IEnumerable<ApiError> errors)
    {
        foreach(var error in errors)
        {
            AddError(error);
        }
    } 

    public void AddError(ApiError error)
    {
        Errors.Add(error);
        ErrorsChanged?.Invoke(this, Errors);
    }

    public void AddError(Exception ex)
    {
        Errors.Add(new ApiError(
            exception: ex,
            code: "",
            field: ex.Source ?? "",
            message: ex.Message,
            resource: "",
            severity: ApiErrorSeverity.Error
        ));
        ErrorsChanged?.Invoke(this, Errors);
    }
}
