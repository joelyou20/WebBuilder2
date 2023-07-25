using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebBuilder2.Shared.Models;

[Serializable]
public class Site : AuditableEntity
{
    [Key]
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("repositoryId")]
    public long RepoId { get; set; }
    [JsonIgnore]
    public Repository Repository { get; set; } = default!;

    public Site() { }

    public Site(string name)
    {
        Name = name;
    }
}
