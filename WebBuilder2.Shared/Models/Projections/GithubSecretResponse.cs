using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;
public class GithubSecretResponse
{
    [JsonProperty("total_count")]
    public int TotalCount { get; set; }

    [JsonProperty("secrets")]
    public List<GithubSecret> GithubSecrets { get; set; } = new();
}

