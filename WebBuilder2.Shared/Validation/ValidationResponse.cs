﻿using Newtonsoft.Json;
using System.Net;
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

    public static ValidationResponse<T> Failure(IEnumerable<T>? values = null, string? message = null, HttpStatusCode? code = null) => new()
    {
        IsSuccessful = false,
        Message = message ?? "Failure",
        Values = values,
        Errors = new List<ApiError> { 
            new(message, code: code.HasValue ? ((int)code!.Value).ToString() : null)
        }
    };
    public static ValidationResponse<T> Failure(T value, string? message = null) => Failure(new List<T>() { value }, message);

    public static ValidationResponse<T> Failure(Exception e) => new()
    {
        IsSuccessful = false,
        Message = e.Message ?? "Failure",
        Errors = new List<ApiError>() { new ApiError(
            message: e.Message ?? "Failure", 
            severity: ApiErrorSeverity.Error, 
            code: "",
            resource: e.Source ?? "",
            field: "",
            exception: e) }
    };

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

    public static ValidationResponse<T> ToResult(string message, JsonSerializerSettings? settings = null)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<ValidationResponse<T>>(message, settings);

            return result!;
        }
        catch
        {
            return ValidationResponse<T>.Failure(message: message);
        }
    }

    public List<T> GetValues() => Values?.ToList() ?? new List<T>();

    public static async Task<ValidationResponse<T>> ParseResponseAsync(HttpResponseMessage response)
    {
        var message = await response.Content.ReadAsStringAsync();
        var result = ValidationResponse<T>.ToResult(message)!;
        return result;
    }
}

public class ValidationResponse
{
    public bool IsSuccessful { get; set; } = false;
    public string? Message { get; set; }
    public IEnumerable<ApiError> Errors { get; set; } = Enumerable.Empty<ApiError>();

    public ValidationResponse() { }

    public ValidationResponse(bool isSuccessful, string? message, IEnumerable<ApiError>? errors = null)
    {
        IsSuccessful = isSuccessful;
        Message = message;
        Errors = errors ?? Enumerable.Empty<ApiError>();
    }

    public static ValidationResponse Default() => new();

    public static ValidationResponse Success(string? message = null) => new()
    {
        IsSuccessful = true,
        Message = message ?? "Success"
    };

    public static ValidationResponse Failure(string? message = null) => new()
    {
        IsSuccessful = false,
        Message = message ?? "Failure",
    };

    public static ValidationResponse Failure(Exception e) => new()
    {
        IsSuccessful = false,
        Message = e.Message ?? "Failure",
        Errors = new List<ApiError>() { new ApiError(
            message: e.Message ?? "Failure",
            severity: ApiErrorSeverity.Error,
            code: "",
            resource: e.Source ?? "",
            field: "",
            exception: e) }
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

    public static ValidationResponse ToResult(string message, JsonSerializerSettings? settings = null)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<ValidationResponse>(message, settings);

            return result!;
        }
        catch
        {
            return Failure(message: message);
        }
    }

    public static async Task<ValidationResponse> ParseResponseAsync(HttpResponseMessage response)
    {
        var message = await response.Content.ReadAsStringAsync();
        var result = ToResult(message)!;
        return result;
    }
}
