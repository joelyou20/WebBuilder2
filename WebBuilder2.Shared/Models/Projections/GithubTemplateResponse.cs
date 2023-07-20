using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GithubTemplateResponse
{
    public IEnumerable<GithubTemplate> Templates { get; set; } = Enumerable.Empty<GithubTemplate>();
}
