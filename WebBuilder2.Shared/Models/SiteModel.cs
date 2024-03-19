using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebBuilder2.Shared.Models;

[Serializable]
public class SiteModel : AuditableEntity
{
    [Key]
    [JsonProperty("id")]
    public long Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } = "";
    [JsonProperty("description")]
    public string Description { get; set; } = "";
    [JsonProperty("siteRepositoryId")]
    public long SiteRepositoryId { get; set; }
    public SiteRepositoryModel? SiteRepository { get; set; }
    [JsonProperty("sslCertificateIssueDate")]
    public DateTime? SSLCertificateIssueDate { get; set; }
    [JsonProperty("sslArn")]
    public string? SSLARN { get; set; }
    [JsonIgnore]
    public string? CertificateID => SSLARN?.Split('/').Last() ?? null;
    public Region Region { get; set; } = Region.USEast1;

    public SiteModel() { }

    public SiteModel(string name)
    {
        Name = name;
    }
}
