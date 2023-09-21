using Amazon.CostExplorer;
using Amazon.Route53Domains;
using Amazon.Route53Domains.Model;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class AwsRoute53DomainsService : IAwsRoute53DomainsService
{
    private AmazonRoute53DomainsClient _client;

    public AwsRoute53DomainsService(AmazonRoute53DomainsClient client)
    {
        _client = client;
    }

    public async Task<string> CheckDomainAvailabilityAsync(string domain)
    {
        var result = await _client.CheckDomainAvailabilityAsync(
            new CheckDomainAvailabilityRequest
            {
                DomainName = domain
            }
        );
        return result.Availability.Value;
    }

    public async Task<ValidationResponse<DomainInquiry>> GetDomainSuggestionsAsync(string domain, bool onlyAvailable, int suggestionCount = 10)
    {
        var suggestionResult = await _client.GetDomainSuggestionsAsync(
            new GetDomainSuggestionsRequest
            {
                DomainName = domain,
                OnlyAvailable = onlyAvailable,
                SuggestionCount = suggestionCount
            }
        );

        var domainSuggestions = suggestionResult.SuggestionsList;
        var domainTypes = domainSuggestions.Select(x => x.DomainName.Split('.').Last()).Distinct().ToList();
        List<DomainPrice>? priceResult = await ListPrices(domainTypes);

        if (priceResult == null) return ValidationResponse<DomainInquiry>.Failure(new List<DomainInquiry>(), "Could not determine prices.");

        IEnumerable<DomainInquiry> domains = domainSuggestions.Select(x => {
            DomainPrice domainPrice = priceResult.Single(y => x.DomainName.EndsWith(y.Name));
            var price = new Shared.Models.PriceWithCurrency
            {
                Price = Convert.ToDecimal(domainPrice.RegistrationPrice.Price),
                Currency = domainPrice.RegistrationPrice.Currency
            };

            var domain = new DomainInquiry
            {
                Name = x.DomainName,
                Availability = x.Availability switch
                {
                    "AVAILABLE" => Shared.Models.DomainAvailability.Available,
                    "AVAILABLE_RESERVED" => Shared.Models.DomainAvailability.AvailableReserved,
                    "AVAILABLE_PREORDER" => Shared.Models.DomainAvailability.AvailablePreOrder,
                    "DONT_KNOW" => Shared.Models.DomainAvailability.DontKnow,
                    "PENDING" => Shared.Models.DomainAvailability.Pending,
                    "RESERVED" => Shared.Models.DomainAvailability.Reserved,
                    "UNAVAILABLE" => Shared.Models.DomainAvailability.Unavailable,
                    "UNAVAILABLE_PREMIUM" => Shared.Models.DomainAvailability.UnavailablePremium,
                    "UNAVAILABLE_RESTRICTED" => Shared.Models.DomainAvailability.UnavailableRestricted,
                    _ => throw new ArgumentException("Availability type does not exist.")
                },
                Price = price
            };

            return domain;
        });

        return ValidationResponse<DomainInquiry>.Success(domains, $"Successfully retrieved {domains.Count()} suggestions.");
    }

    public async Task<List<DomainPrice>> ListPrices(List<string> domainTypes)
    {
        var results = new List<DomainPrice>();
        var paginatePrices = _client.Paginators.ListPrices(new ListPricesRequest());
        // Get the entire list using the paginator.
        await foreach (var prices in paginatePrices.Prices)
        {
            results.Add(prices);
        }

        return results.Where(p => domainTypes.Contains(p.Name)).ToList();
    }

    public async Task<ValidationResponse<Domain>> GetRegisteredDomainsAsync()
    {
        ListDomainsResponse result = await _client.ListDomainsAsync();

        IEnumerable<Domain> domainNames = result.Domains.Select(x => new Domain {
            Name = x.DomainName 
        });

        if (result == null) return ValidationResponse<Domain>.Failure(message: "Failed to get list of registered domains.");

        return ValidationResponse<Domain>.Success(domainNames, $"Successfully retrieved {domainNames.Count()} registered domains");
    }

    public async Task<ValidationResponse> RegisterDomainAsync(string domainName)
    {
        var request = new RegisterDomainRequest
        {
            DomainName = domainName
        };

        var result = await _client.RegisterDomainAsync(request);

        if (result == null) return ValidationResponse.Failure(message: $"Failed to register domain {domainName}.");

        return ValidationResponse.Success();
    }
}
