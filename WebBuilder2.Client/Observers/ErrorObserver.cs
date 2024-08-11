using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Observers;

public class ErrorObserver : IErrorObserver
{
    public string? ErrorMessage { get; set; }
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public event Action? OnErrorChanged;

    public void ReportError(string errorMessage)
    {
        ErrorMessage = errorMessage;
        OnErrorChanged?.Invoke();
    }

    public void ClearError()
    {
        ErrorMessage = null;
        OnErrorChanged?.Invoke();
    }
}
