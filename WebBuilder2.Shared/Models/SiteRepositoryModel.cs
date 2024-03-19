using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebBuilder2.Shared.Models;

public class SiteRepositoryModel : AuditableEntity
{
    [Key]
    [JsonProperty("id")]
    public long Id { get; set; }
    [JsonProperty("siteId")]
    public long SiteId { get; set; }
    public SiteModel? Site { get; set; }
    [JsonProperty("repositoryId")]
    public long RepositoryId { get; set; }
    public RepositoryModel? Repository { get; set; }
}
