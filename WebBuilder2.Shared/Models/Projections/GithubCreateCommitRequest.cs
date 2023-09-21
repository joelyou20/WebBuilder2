using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GithubCreateCommitRequest
{
    public string Message { get; set; } = string.Empty;
    public List<NewFile> Files { get; set; } = new List<NewFile>();
}
