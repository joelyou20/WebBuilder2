using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class CreateSiteRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Domain Domain { get; set; } = new();
    public Region Region { get; set; } = new();
    public RepositoryModel TemplateRepository { get; set; } = new();
}
