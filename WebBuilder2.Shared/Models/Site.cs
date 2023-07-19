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
    [JsonPropertyName("deletedDateTime")]
    public DateTime? DeletedDateTime { get; set; }

    public Site() { }

    public Site(string name)
    {
        Name = name;
    }
}
