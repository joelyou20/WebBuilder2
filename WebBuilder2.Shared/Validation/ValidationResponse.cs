namespace WebBuilder2.Shared.Validation;

public class ValidationResponse<T> where T : class
{
    public bool IsSuccessful { get; set; } = false;
    public string? Message { get; set; }
    public IEnumerable<T>? Values { get; set; } = default!;

    public static ValidationResponse<T> Default() => new();

    public static ValidationResponse<T> Success(IEnumerable<T>? values) => new()
    {
        IsSuccessful = true,
        Message = "Success",
        Values = values
    };

    public static ValidationResponse<T> Failure(IEnumerable<T>? values) => new()
    {
        IsSuccessful = false,
        Message = "Failure",
        Values = values
    };

    public static ValidationResponse<T> CouldNotLocateEntity(IEnumerable<T>? values) => new()
    {
        IsSuccessful = false,
        Message = "Could not locate entity in database",
        Values = values
    };

    public static ValidationResponse<T> EntityAlreadyExists(IEnumerable<T>? values) => new()
    {
        IsSuccessful = false,
        Message = "Entity already exists in database",
        Values = values
    };
}
