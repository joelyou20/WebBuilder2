using WebBuilder2.Server.Data.Models.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Repositories.Contracts;

public interface IRepository<T1, T2>
    where T1 : AuditableEntity
    where T2 : IEntity<T1>
{
    IQueryable<T1>? Get(IEnumerable<long>? exclude = null);
    IEnumerable<T1> AddRange(IEnumerable<T1> values);
    IEnumerable<T1> UpdateRange(IEnumerable<T1> values);
    IEnumerable<T1> UpsertRange(IEnumerable<T1> values);
    IEnumerable<T1> DeleteRange(IEnumerable<T1> values);
    IEnumerable<T1> SoftDeleteRange(IEnumerable<T1> values);

    T2 ToDto(T1 site);
}
