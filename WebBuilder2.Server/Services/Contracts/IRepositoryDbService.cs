using WebBuilder2.Server.Data.Models;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IRepositoryDbService : IDbService<Repository, RepositoryDTO>
    {
    }
}
