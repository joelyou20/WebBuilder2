using System.Collections.ObjectModel;
using WebBuilder2.Client.Models;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Managers.Contracts;

public interface ISiteManager
{
    ObservableCollection<Job> BuildCreateSiteJobList();
    Task CreateSiteAsync(CreateSiteRequest createSiteRequest, ObservableCollection<Job> jobList);
}
