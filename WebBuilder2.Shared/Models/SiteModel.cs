using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    public RepositoryModel? Repository { get; set; }
    [JsonProperty("sslCertificateIssueDate")]
    public DateTime? SSLCertificateIssueDate { get; set; }

    public SiteModel() { }

    public SiteModel(string name)
    {
        Name = name;
    }
}
