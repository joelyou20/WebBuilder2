using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Models;

public class SiteCreationState
{
    public SiteModel Site { get; set; } = default!;
    public Bucket DomainBucket { get; set; } = default!;
    public Bucket SubDomainBucket { get; set; } = default!;
    public Bucket LoggingBucket { get; set; } = default!;
    public RepositoryModel Repository { get; set; } = default!;
}
