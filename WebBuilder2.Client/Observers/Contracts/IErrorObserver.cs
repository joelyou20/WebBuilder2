using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Observers.Contracts;

public interface IErrorObserver
{
    string? ErrorMessage { get; set; }
    bool HasError { get; }

    event Action? OnErrorChanged;
    void ReportError(string errorMessage);
    void ClearError();
}
