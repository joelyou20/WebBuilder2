using Amazon.CostExplorer;
using Amazon.CostExplorer.Model;
using System.Net;
using WebBuilder2.Server.Services.Contracts;

namespace WebBuilder2.Server.Services
{
    public class AwsCostExplorerService : IAwsCostExplorerService
    {
        private AmazonCostExplorerClient _client;

        public AwsCostExplorerService(AmazonCostExplorerClient client)
        {
            _client = client;
        }

        public async Task<string> GetForecastedCostAsync()
        {
            var response = await _client.GetCostForecastAsync(new GetCostForecastRequest());
            
            if(response == null || response.HttpStatusCode != HttpStatusCode.OK)
            {
                // Handle error
                return string.Empty;
            }

            return response.Total.Amount;
        }
    }
}
