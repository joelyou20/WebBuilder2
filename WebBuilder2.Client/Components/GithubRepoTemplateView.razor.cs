using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components;

public partial class GithubRepoTemplateView
{
    [Parameter] public List<RepositoryModel> TemplateRepositories { get; set; } = new();
}
