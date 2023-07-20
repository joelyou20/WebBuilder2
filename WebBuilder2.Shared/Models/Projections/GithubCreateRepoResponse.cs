using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GithubCreateRepoResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
