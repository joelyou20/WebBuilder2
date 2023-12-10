using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class LogModel : AuditableEntity
{
    [Key]
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("type")]
    public LogType Type { get; set; } = LogType.Information;
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    [JsonPropertyName("stackTrace")]
    public string? StackTrace { get; set; }
    [JsonPropertyName("exception")]
    public string Exception { get; set; } = string.Empty;
}
