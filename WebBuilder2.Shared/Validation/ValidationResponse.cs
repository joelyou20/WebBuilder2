using WebBuilder2.Shared.Models;

namespace WebBuilder2.Shared.Validation;

public class ValidationResponse<T> where T : class
{
    public bool IsSuccessful { get; set; } = false;
    public string? Message { get; set; }
    public IEnumerable<T>? Values { get; set; } = default!;
    public IEnumerable<ApiError> Errors { get; set; } = Enumerable.Empty<ApiError>();

    public static ValidationResponse<T> Default() => new();

    public static ValidationResponse<T> Success(IEnumerable<T>? values) => new()
    {
        IsSuccessful = true,
        Message = "Success",
        Values = values
    };

    public static ValidationResponse<T> Success(T value) => Success(new List<T> { value });

    public static ValidationResponse<T> Failure(IEnumerable<T>? values) => new()
    {
        IsSuccessful = false,
        Message = "Failure",
        Values = values
    };
    public static ValidationResponse<T> Failure(T value) => Failure(new List<T>() { value });

    public static ValidationResponse<T> CouldNotLocateEntity(IEnumerable<T>? values) => new()
    {
        IsSuccessful = false,
        Message = "Could not locate entity in database",
        Values = values
    };

    public static ValidationResponse<T> CouldNotLocateEntity(T value) => CouldNotLocateEntity(new List<T>() { value });

    public static ValidationResponse<T> EntityAlreadyExists(IEnumerable<T>? values) => new()
    {
        IsSuccessful = false,
        Message = "Entity already exists in database",
        Values = values
    };

    public static ValidationResponse<T> EntityAlreadyExists(T value) => EntityAlreadyExists(new List<T>() { value });

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
}
