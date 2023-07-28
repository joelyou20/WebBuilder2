namespace WebBuilder2.Server.Services.Contracts
{
    public interface IAwsSecretsManagerService
    {
        Task<string> GetSecretAsync(string name);
    }
}
