using Amazon.Route53Domains.Model;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface IAwsRoute53DomainsService
{
    Task<string> CheckDomainAvailabilityAsync(string domain);
    Task<ValidationResponse<DomainInquiry>> GetDomainSuggestionsAsync(string domain, bool onlyAvailable, int suggestionCount = 50);
    Task<ValidationResponse<Domain>> GetRegisteredDomainsAsync();
    Task<ValidationResponse> RegisterDomainAsync(string domainName);
}
