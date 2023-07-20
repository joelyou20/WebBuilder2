using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GithubCreateRepoRequest
{
    public string RepoName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RepoVisibility Visibility { get; set; } = RepoVisibility.Public;
    public bool? IsPrivate { get; set; }
    public bool? IsTemplate { get; set; }
    public bool? AllowAutoMerge { get; set; }
    public bool? AllowMergeCommit { get; set; }
    public bool? AllowRebaseMerge { get; set; }
    public bool? AllowSquashMerge { get; set; }
    public bool? AutoInit { get; set; }
    public bool? DeleteBranchOnMerge { get; set; }
    public string GitignoreTemplate { get; set; } = string.Empty;
    public bool? HasDownloads { get; set; }
    public bool? HasIssues { get; set; }
    public bool? HasProjects { get; set; }
    public bool? HasWiki { get; set; }
    public string Homepage { get; set; } = string.Empty;
    public string LicenseTemplate { get; set; } = string.Empty;
    public int? TeamId { get; set; }
    public bool? UseSquashPrTitleAsDefault { get; set; }
}
