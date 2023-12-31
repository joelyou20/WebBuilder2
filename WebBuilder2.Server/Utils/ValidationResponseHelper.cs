﻿using WebBuilder2.Shared.Models;
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
                    new ApiError(
                        message: ex.Message,
                        severity: ApiErrorSeverity.Error,
                        code: "",
                        resource: ex.Source ?? "",
                        field: "",
                        exception: ex)
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
                    new ApiError(
                        message: ex.Message,
                        severity: ApiErrorSeverity.Error,
                        code: "",
                        resource: ex.Source ?? "",
                        field: "",
                        exception: ex)
                }
        };
        return response;
    }

    public static ValidationResponse<T> BuildFailedResponse(IEnumerable<T> values, Exception ex)
    {
        var response = new ValidationResponse<T>
        {
            IsSuccessful = false,
            Values = values,
            Message = ex.Message,
            Errors = new List<ApiError>
                {
                    new ApiError(message : ex.Message, severity : ApiErrorSeverity.Error, code : "", resource : ex.Source ?? "", field : "", exception : ex)
                }
        };
        return response;
    }
}

public static class ValidationResponseHelper
{
    public static ValidationResponse BuildFailedResponse(Exception ex)
    {
        var response = new ValidationResponse
        {
            IsSuccessful = false,
            Message = ex.Message,
            Errors = new List<ApiError>
                {
                    new ApiError(message : ex.Message, severity : ApiErrorSeverity.Error, code : "", resource : ex.Source ?? "", field : "", exception : ex)
                }
        };
        return response;
    }
}
