using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public class RepositoryModel : AuditableEntity
{
    [Key]
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonProperty("siteRepositoryId")]
    public long SiteRepositoryId { get; set; }
    public SiteRepositoryModel? SiteRepository { get; set; }
    [JsonPropertyName("externalId")]
    public long ExternalId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("repoName")]
    public string RepoName { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    [Required]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("visibility")]
    public RepoVisibility Visibility { get; set; } = RepoVisibility.Public;
    [JsonPropertyName("isPrivate")]
    public bool IsPrivate { get; set; } = false;
    [JsonPropertyName("isTemplate")]
    public bool IsTemplate { get; set; } = false;
    [JsonPropertyName("allowAutoMerge")]
    public bool AllowAutoMerge { get; set; } = true;
    [JsonPropertyName("allowMergeCommit")]
    public bool AllowMergeCommit { get; set; } = true;
    [JsonPropertyName("allowRebaseMerge")]
    public bool AllowRebaseMerge { get; set; } = true;
    [JsonPropertyName("allowSquashMerge")]
    public bool AllowSquashMerge { get; set; } = true;
    [JsonPropertyName("autoInit")]
    public bool AutoInit { get; set; } = true;
    [JsonPropertyName("deleteBranchOnMerge")]
    public bool DeleteBranchOnMerge { get; set; }
    [JsonPropertyName("gitIgnoreTemplate")]
    public string GitIgnoreTemplate { get; set; } = string.Empty;
    [JsonPropertyName("hasDownloads")]
    public bool HasDownloads { get; set; } = true;
    [JsonPropertyName("hasIssues")]
    public bool HasIssues { get; set; } = true;
    [JsonPropertyName("hasProjects")]
    public bool HasProjects { get; set; } = true;
    [JsonPropertyName("hasWiki")]
    public bool HasWiki { get; set; } = true;
    [JsonPropertyName("homepage")]
    [Required]
    public string Homepage { get; set; } = string.Empty;
    [JsonPropertyName("licenseTemplate")]
    public string LicenseTemplate { get; set; } = string.Empty;
    [JsonPropertyName("teamId")]
    public int? TeamId { get; set; }
    [JsonPropertyName("useSquashPrTitleAsDefault")]
    public bool UseSquashPrTitleAsDefault { get; set; } = false;
    [JsonPropertyName("htmlUrl")]
    public string HtmlUrl { get; set; } = string.Empty;
    [JsonPropertyName("gitUrl")]
    public string GitUrl { get; set; } = string.Empty;
}
