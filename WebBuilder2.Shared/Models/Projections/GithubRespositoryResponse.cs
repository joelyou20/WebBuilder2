using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GithubRespositoryResponse
{
    public List<GithubRepository> Repositories { get; set; } = new List<GithubRepository>();
}
