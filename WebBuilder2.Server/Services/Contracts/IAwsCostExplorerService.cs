namespace WebBuilder2.Server.Services.Contracts
{
    public interface IAwsCostExplorerService
    {
        Task<string> GetForecastedCostAsync();
    }
}
