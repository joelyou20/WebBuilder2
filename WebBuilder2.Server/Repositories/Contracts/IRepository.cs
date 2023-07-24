using WebBuilder2.Server.Data.Models.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Repositories.Contracts;

public interface IRepository<T1, T2>
    where T1 : AuditableEntity
    where T2 : IDto<T1>
{
    IQueryable<T1>? Get(IEnumerable<long>? exclude = null);
    void AddRange(IEnumerable<T1> values);
    void UpdateRange(IEnumerable<T1> values);
    void UpsertRange(IEnumerable<T1> values);
    void DeleteRange(IEnumerable<T1> values);
    void SoftDeleteRange(IEnumerable<T1> values);

    T2 ToDto(T1 site);
}
