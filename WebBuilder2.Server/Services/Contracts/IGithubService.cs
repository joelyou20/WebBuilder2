﻿using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IGithubService
    {
        Task<GithubRespositoryResponse> GetRespositoriesAsync();
    }
}