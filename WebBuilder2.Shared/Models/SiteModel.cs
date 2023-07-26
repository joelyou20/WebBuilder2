using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebBuilder2.Shared.Models;

[Serializable]
public class SiteModel : AuditableEntity
{
    [Key]
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    public RepositoryModel? Repository { get; set; }

    public SiteModel() { }

    public SiteModel(string name)
    {
        Name = name;
    }
}
