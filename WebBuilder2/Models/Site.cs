using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebBuilder2.Models;

[Serializable]
public class Site
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    public Site() { }

    public Site(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
