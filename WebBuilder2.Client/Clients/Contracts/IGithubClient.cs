﻿using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IGithubClient
{
    Task<GithubRespositoryResponse> GetRepositoriesAsync();
}