using Amazon.CostExplorer;
using Amazon.CostExplorer.Model;
using Amazon.S3.Model;
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
            var request = new GetCostForecastRequest
            {
                Granularity = Granularity.DAILY,
                Metric = Metric.BLENDED_COST,
                TimePeriod = new DateInterval
                {
                    Start = DateTime.UtcNow.ToShortDateString(),
                    End = DateTime.UtcNow.AddDays(14).ToShortDateString()
                }
            };

            var response = await _client.GetCostForecastAsync(request);
            
            if(response == null || response.HttpStatusCode != HttpStatusCode.OK)
            {
                // Handle error
                return string.Empty;
            }

            return response.Total.Amount;
        }
    }
}
