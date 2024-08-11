using Amazon.Route53Domains.Model;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface IAwsRoute53DomainsService
{
    Task<string> CheckDomainAvailabilityAsync(string domain);
    Task<IEnumerable<DomainInquiry>> GetDomainSuggestionsAsync(string domain, bool onlyAvailable, int suggestionCount = 50);
    Task<IEnumerable<Domain>> GetRegisteredDomainsAsync();
    Task<RegisterDomainResponse> RegisterDomainAsync(string domainName);
}
