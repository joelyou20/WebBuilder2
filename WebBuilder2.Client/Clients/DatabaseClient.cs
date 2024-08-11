using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class DatabaseClient(HttpClient httpClient) : ClientBase(httpClient, "database"), IDatabaseClient
{
    public async Task PostCreateDatabaseAsync(string databaseName) => await PostAsync("create", JsonContent.Create(databaseName));
}
