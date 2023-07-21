using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Shared.Models.Projections;


public class GithubCreateRepoRequest
{
    [Required]
    public string RepoName { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    public RepoVisibility Visibility { get; set; } = RepoVisibility.Public;
    public bool IsPrivate { get; set; } = false;
    public bool IsTemplate { get; set; } = false;
    public bool AllowAutoMerge { get; set; } = false;
    public bool AllowMergeCommit { get; set; } = false;
    public bool AllowRebaseMerge { get; set; } = false;
    public bool AllowSquashMerge { get; set; } = false;
    public bool AutoInit { get; set; } = false;
    public bool DeleteBranchOnMerge { get; set; }
    public string GitignoreTemplate { get; set; } = string.Empty;
    public bool HasDownloads { get; set; } = false;
    public bool HasIssues { get; set; } = false;
    public bool HasProjects { get; set; } = false;
    public bool HasWiki { get; set; } = false;
    public string Homepage { get; set; } = string.Empty;
    public string LicenseTemplate { get; set; } = string.Empty;
    public int? TeamId { get; set; }
    public bool UseSquashPrTitleAsDefault { get; set; } = false;
}
