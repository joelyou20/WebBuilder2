using WebBuilder2.Shared.Models;

namespace WebBuilder2.Shared.Validation;

[Serializable]
public class ValidationResponse<T> where T : class
{
    public bool IsSuccessful { get; set; } = false;
    public string? Message { get; set; }
    public IEnumerable<T>? Values { get; set; } = default!;
    public IEnumerable<ApiError> Errors { get; set; } = Enumerable.Empty<ApiError>();
    public bool HasValues => IsSuccessful && Values != null && Values.Any();

    public static ValidationResponse<T> Default() => new();

    public static ValidationResponse<T> Success(IEnumerable<T>? values = null, string? message = null) => new()
    {
        IsSuccessful = true,
        Message = message ?? "Success",
        Values = values
    };

    public static ValidationResponse<T> Success(T value, string? message = null) => Success(new List<T> { value }, message);

    public static ValidationResponse<T> Failure(IEnumerable<T>? values = null, string? message = null) => new()
    {
        IsSuccessful = false,
        Message = message ?? "Failure",
        Values = values
    };
    public static ValidationResponse<T> Failure(T value, string? message = null) => Failure(new List<T>() { value }, message);

    public static ValidationResponse<T> CouldNotLocateEntity(IEnumerable<T>? values = null, string? message = null) => new()
    {
        IsSuccessful = false,
        Message = message ?? "Could not locate entity in database",
        Values = values
    };

    public static ValidationResponse<T> CouldNotLocateEntity(T value, string? message = null) => CouldNotLocateEntity(new List<T>() { value }, message);

    public static ValidationResponse<T> EntityAlreadyExists(IEnumerable<T>? values = null, string? message = null) => new()
    {
        IsSuccessful = false,
        Message = message ?? "Entity already exists in database",
        Values = values
    };

    public static ValidationResponse<T> EntityAlreadyExists(T value, string? message = null) => EntityAlreadyExists(new List<T>() { value }, message);

    public static ValidationResponse<T> NotAuthenticated(IEnumerable<T>? values = null, string? message = null) => new()
    {
        IsSuccessful = false,
        Message = message ?? "User not Authenticated",
        Values = values
    };

    public static ValidationResponse<T> NotAuthenticated(T value, string? message = null) => NotAuthenticated(new List<T>() { value }, message);

    public List<T> GetValues() => Values?.ToList() ?? new List<T>();
}

public class ValidationResponse
{
    public bool IsSuccessful { get; set; } = false;
    public string? Message { get; set; }
    public IEnumerable<ApiError> Errors { get; set; } = Enumerable.Empty<ApiError>();

    public static ValidationResponse Default() => new();

    public static ValidationResponse Success() => new()
    {
        IsSuccessful = true,
        Message = "Success"
    };

    public static ValidationResponse Failure() => new()
    {
        IsSuccessful = false,
        Message = "Failure",
    };

    public static ValidationResponse CouldNotLocateEntity() => new()
    {
        IsSuccessful = false,
        Message = "Could not locate entity in database",
    };

    public static ValidationResponse EntityAlreadyExists() => new()
    {
        IsSuccessful = false,
        Message = "Entity already exists in database",
    };

    public static ValidationResponse NotAuthenticated() => new()
    {
        IsSuccessful = false,
        Message = "User not Authenticated",
    };
}
