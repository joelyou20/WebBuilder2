using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class CreateSiteRequest
{
    public string SiteName { get; set; } = string.Empty;
    public RepositoryModel TemplateRepository { get; set; } = new RepositoryModel();
}
