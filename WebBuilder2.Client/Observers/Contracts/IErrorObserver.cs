using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Observers.Contracts;

public interface IErrorObserver
{
    event EventHandler<List<ApiError>> ErrorsChanged;
    void AddErrorRange(IEnumerable<ApiError> errors);
    void AddError(ApiError error);
    void AddError(Exception error);

}
