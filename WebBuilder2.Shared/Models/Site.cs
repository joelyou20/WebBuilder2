using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebBuilder2.Shared.Models;

[Serializable]
public class Site
{
    [Key]
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("creationDate")]
    public DateTime CreationDate { get; set; }
    public int Id => Name.GetHashCode();

    public Site() { }

    public Site(string name)
    {
        Name = name;
    }
}
