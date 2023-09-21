using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Projections;

public class GithubCreateSecretRequest
{
    [JsonPropertyName("encrypted_value")]
    public string EncryptedValue { get; set; } = string.Empty;
    [JsonPropertyName("key_id")]
    public string PublicKeyId { get; set; } = string.Empty;

    public GithubCreateSecretRequest() { }

    public GithubCreateSecretRequest(string encryptedValue, string publicKeyId)
    {
        EncryptedValue = encryptedValue;
        PublicKeyId = publicKeyId;
    }
}
