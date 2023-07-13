using System.Text.Json.Serialization;

namespace WebBuilder2.Shared.Models;

[Serializable]
public class Site
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("bucketId")]
    public string BucketId { get; set; } = "";
    [JsonPropertyName("creationDate")]
    public DateTime CreationDate { get; set; }

    public Site() { }

    public Site(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
