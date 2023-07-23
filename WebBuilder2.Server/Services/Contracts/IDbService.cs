using WebBuilder2.Server.Data.Models.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface IDbService<T1, T2> 
    where T1 : AuditableEntity 
    where T2 : IDto<T1>
{
    Task<ValidationResponse<T1>> GetAllAsync();
    Task<ValidationResponse<T1>> InsertAsync(T1 value);
    Task<ValidationResponse<T1>> GetSingleAsync(long id);
    Task<ValidationResponse<T1>> UpdateAsync(T1 value);
    Task<ValidationResponse<T1>> UpsertAsync(T1 value);
    Task<ValidationResponse<T1>> DeleteAsync(T1 value);
    Task<ValidationResponse<T1>> SoftDeleteAsync(T1 value);
    T2 ToDto(T1 site);
}
