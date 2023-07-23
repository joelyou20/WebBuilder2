using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Utils;

public static class ValidationResponseHelper<T> where T : class
{
    public static ValidationResponse<T> BuildFailedResponse(Exception ex)
    {
        var response = new ValidationResponse<T>
        {
            IsSuccessful = false,
            Values = new List<T>(),
            Message = ex.Message,
            Errors = new List<ApiError>
                {
                    new ApiError
                    {
                        Exception = ex,
                        Message = ex.Message
                    }
                }
        };
        return response;
    }

    public static ValidationResponse<T> BuildFailedResponse(T value, Exception ex)
    {
        var response = new ValidationResponse<T>
        {
            IsSuccessful = false,
            Values = new List<T> { value },
            Message = ex.Message,
            Errors = new List<ApiError>
                {
                    new ApiError
                    {
                        Exception = ex,
                        Message = ex.Message
                    }
                }
        };
        return response;
    }
}
