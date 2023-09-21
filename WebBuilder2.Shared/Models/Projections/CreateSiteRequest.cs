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
    public ProjectTemplateType ProjectTemplateType { get; set; }
    public Dictionary<BucketType, Bucket> Buckets => new()
    {
        { 
            BucketType.Domain, 
            new Bucket(
                name: Domain.Name,
                region: Region,
                configureForWebSiteHosting: true) 
        },
        {
            BucketType.Subdomain,
            new Bucket(
                name: $"www.{Domain.Name}",
                region: Region,
                configureForWebSiteHosting: true,
                redirectTarget: Domain.Name)
        },
        {
            BucketType.Logging,
            new Bucket(
                name: $"logs.{Domain.Name}",
                region: Region,
                configureForLogging: true)
        }
    };
}
