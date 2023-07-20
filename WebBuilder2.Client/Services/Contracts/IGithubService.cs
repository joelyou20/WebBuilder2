﻿using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Services.Contracts;

public interface IGithubService
{
    Task<RespositoryResponse> GetRepositoriesAsync();
    Task<GithubTemplateResponse> GetTemplatesAsync();
    Task<IEnumerable<string>> GetGitIgnoreTemplatesAsync();
    Task<GithubAuthenticationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request);
    Task<GithubCreateRepoResponse> PostCreateRepoAsync(GithubCreateRepoRequest request);
}
