using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class GithubPublicKey
{
    [JsonProperty("key_id")]
    public string Id { get; set; } = string.Empty;
    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;
}
