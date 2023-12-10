using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IAwsCostExplorerService
    {
        Task<ValidationResponse<string>> GetForecastedCostAsync();
    }
}
