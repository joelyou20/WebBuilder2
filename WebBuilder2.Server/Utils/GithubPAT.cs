using System.Text.Json.Serialization;

namespace WebBuilder2.Server.Utils;

public class GithubPAT
{
    [JsonPropertyName("github-pat")]
    public string Value { get; set; } = string.Empty;
}
