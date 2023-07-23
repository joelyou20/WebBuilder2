using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GitIgnoreTemplateResponse
{
    public IEnumerable<string> Templates { get; set; } = Enumerable.Empty<string>();

    public GitIgnoreTemplateResponse() { }
    public GitIgnoreTemplateResponse(IEnumerable<string> templates)
    {
        Templates = templates;
    }
}
