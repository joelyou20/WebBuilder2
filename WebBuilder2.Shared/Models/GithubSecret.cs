using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebBuilder2.Shared.Models;
 
public class GithubSecret
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    [JsonProperty("value")]
    public string? Value { get; set; }
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
