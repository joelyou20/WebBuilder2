using WebBuilder2.Server.Data.Models;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models.Dtos;

namespace WebBuilder2.Server.Repositories.Contracts
{
    public interface IRepositoryRepository : IRepository<RepositoryModel, Repository>
    {
    }
}
