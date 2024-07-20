using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface IAwsAmplifyService
{
    Task<ValidationResponse> CreateAppFromRepoAsync(RepositoryModel repo);
}
