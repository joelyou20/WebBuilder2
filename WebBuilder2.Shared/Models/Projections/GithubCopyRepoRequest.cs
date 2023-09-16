using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GithubCopyRepoRequest
{
    public string ClonedRepoName { get; set; } = string.Empty;
    public string NewRepoName { get; set; } = string.Empty;
    public string? Path { get; set; }
}
