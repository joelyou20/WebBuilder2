using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Pages;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Clients;

// INFO: The plan is eventually to use the Database as the source of truth for populating the data on the site. For the time being, we will be pulling data from two places:
//  1. The JSON file (i.e. temporary DB of static data)
//  2. AWS -> This will be to serve real data for testing, and later any data pulled from here will be updated in the DB? Maybe...

public class JsonClient
{
    private HttpClient _httpClient;

    public JsonClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    //public async Task<IEnumerable<SiteRepository>?> GetSiteRepositoriesAsync()
    //{
        //var response = await _httpClient.GetAsync("./site-repositories.json");
        //var message = (await response.Content.ReadAsStringAsync()).Replace("\r\n", "");
        //IEnumerable<SiteConnection> siteConnections = JsonConvert.DeserializeObject<IEnumerable<SiteConnection>>(message);

        //var repos = await GetRepositoriesAsync();

        //if (repos == null) return null;

        //var sites = await GetSitesAsync();
        //var siteRepos = new List<SiteRepository>();

        //repos.Where(repo => siteConnections.Any(sc => repo.Id == sc.RepositoryId)).ToList();

        //repos.Foreach

    //    return null;
    //}
}
