using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Managers.Contracts;

public interface ISiteManager
{
    Task<ValidationResponse> CreateSiteAsync(CreateSiteRequest createSiteRequest);
}
