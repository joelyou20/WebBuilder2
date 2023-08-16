using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class CreateSiteRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    public Domain Domain { get; set; } = new();
    public Region Region { get; set; } = new();
    public RepositoryModel TemplateRepository { get; set; } = new();
    public IEnumerable<Bucket> Buckets => new Bucket[]
    {
        new Bucket(Domain.Name, Region),
        new Bucket($"www.{Domain.Name}", Region),
        new Bucket($"logs.{Domain.Name}", Region)
    };
}
