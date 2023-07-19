using WebBuilder2.Server.Data.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface IDbService<T> where T : class
{
    Task<ValidationResponse<T>> GetAllAsync();
    Task<ValidationResponse<T>> InsertAsync(T value);
    Task<ValidationResponse<T>> GetSingleAsync(long id);
    Task<ValidationResponse<T>> UpdateAsync(T value);
    Task<ValidationResponse<T>> UpsertAsync(T value);
    Task<ValidationResponse<T>> DeleteAsync(T value);
    Task<ValidationResponse<T>> SoftDeleteAsync(T value);
}
