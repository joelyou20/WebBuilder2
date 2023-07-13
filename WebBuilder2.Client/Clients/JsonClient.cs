using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Clients;

// INFO: The plan is eventually to use the Database as the source of truth for populating the data on the site. For the time being, we will be pulling data from two places:
//  1. The JSON file (i.e. temporary DB of static data)
//  2. AWS -> This will be to serve real data for testing, and later any data pulled from here will be updated in the DB? Maybe...

public class JsonClient : IDatabaseClient
{
    private HttpClient _httpClient;

    public JsonClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Site?> GetSingleSiteAsync(int id)
    {
        return (await GetSitesAsync())?.SingleOrDefault(site => site.Id == id);
    }

    public async Task<IEnumerable<Site>?> GetSitesAsync()
    {
        var response = await _httpClient.GetAsync("./test.json");
        var message = (await response.Content.ReadAsStringAsync()).Replace("\r\n", "");
        IEnumerable<Site> sites = JsonConvert.DeserializeObject<IEnumerable<Site>>(message);
        return sites;
    }
}
