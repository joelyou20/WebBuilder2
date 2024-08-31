using Amazon.Route53Domains;
using Amazon.Route53Domains.Model;
using Amazon.S3;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Services;

public class AwsRoute53DomainsService(AmazonRoute53DomainsClient client) : IAwsRoute53DomainsService
{
    private readonly AmazonRoute53DomainsClient _client = client;

    public async Task<string> CheckDomainAvailabilityAsync(string domain)
    {
        CheckDomainAvailabilityResponse response = await _client.CheckDomainAvailabilityAsync(
            new CheckDomainAvailabilityRequest
            {
                DomainName = domain
            }
        );

        AmazonServiceResponseValidator<AmazonRoute53DomainsException>.Validate(response, $"Failed to get domain availability for domain {domain}");

        return response.Availability.Value;
    }

    public async Task<IEnumerable<DomainInquiry>> GetDomainSuggestionsAsync(string domain, bool onlyAvailable, int suggestionCount = 10)
    {
        GetDomainSuggestionsResponse response = await _client.GetDomainSuggestionsAsync(
            new GetDomainSuggestionsRequest
            {
                DomainName = domain,
                OnlyAvailable = onlyAvailable,
                SuggestionCount = suggestionCount
            }
        );

        AmazonServiceResponseValidator<AmazonRoute53DomainsException>.Validate(response);

        var domainSuggestions = response.SuggestionsList;
        var domainTypes = domainSuggestions.Select(x => x.DomainName.Split('.').Last()).Distinct().ToList();
        IEnumerable<DomainPrice>? priceResult = await ListPrices(domainTypes);

        if (priceResult == null || !priceResult.Any()) throw new AmazonRoute53DomainsException("Could not determine prices.");

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

        return domains;
    }

    public async Task<IEnumerable<DomainPrice>> ListPrices(List<string> domainTypes)
    {
        var results = new List<DomainPrice>();
        IListPricesPaginator paginatePrices = _client.Paginators.ListPrices(new ListPricesRequest());
        // Get the entire list using the paginator.
        await foreach (var prices in paginatePrices.Prices)
        {
            results.Add(prices);
        }

        return results.Where(p => domainTypes.Contains(p.Name));
    }

    public async Task<IEnumerable<Domain>> GetRegisteredDomainsAsync()
    {
        ListDomainsResponse response = await _client.ListDomainsAsync();

        AmazonServiceResponseValidator<AmazonRoute53DomainsException>.Validate(response);

        IEnumerable<Domain> domainNames = response.Domains.Select(x => new Domain {
            Name = x.DomainName 
        });


        return domainNames;
    }

    public async Task<RegisterDomainResponse> RegisterDomainAsync(string domainName)
    {
        var request = new RegisterDomainRequest
        {
            DomainName = domainName
        };

        RegisterDomainResponse response = await _client.RegisterDomainAsync(request);

        AmazonServiceResponseValidator<AmazonRoute53DomainsException>.Validate(response);

        return response;
    }
}
