using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Shared.Models;
using WebBuilder2.Server.Data.Models.Contracts;
using System.Text.Json.Serialization;

namespace WebBuilder2.Server.Data.Models
{
    public class RepositoryDTO : AuditableEntity, IDto<Repository>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RepoName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RepoVisibility Visibility { get; set; } = RepoVisibility.Public;
        public bool IsPrivate { get; set; } = false;
        public bool IsTemplate { get; set; } = false;
        public bool AllowAutoMerge { get; set; } = true;
        public bool AllowMergeCommit { get; set; } = true;
        public bool AllowRebaseMerge { get; set; } = true;
        public bool AllowSquashMerge { get; set; } = true;
        public bool AutoInit { get; set; } = true;
        public bool DeleteBranchOnMerge { get; set; }
        public string GitIgnoreTemplate { get; set; } = string.Empty;
        public bool HasDownloads { get; set; } = true;
        public bool HasIssues { get; set; } = true;
        public bool HasProjects { get; set; } = true;
        public bool HasWiki { get; set; } = true;
        public string Homepage { get; set; } = string.Empty;
        public string LicenseTemplate { get; set; } = string.Empty;
        public int? TeamId { get; set; }
        public bool UseSquashPrTitleAsDefault { get; set; } = false;
        public string HtmlUrl { get; set; } = string.Empty;
        public string GitUrl { get; set; } = string.Empty;

        public Repository FromDto() => new()
        {
            Id = Id,
            AllowAutoMerge = AllowAutoMerge,
            AllowMergeCommit = AllowMergeCommit,
            AllowRebaseMerge = AllowRebaseMerge,
            AllowSquashMerge = AllowSquashMerge,
            AutoInit = AutoInit,
            DeleteBranchOnMerge = DeleteBranchOnMerge,
            GitIgnoreTemplate = GitIgnoreTemplate,
            HasDownloads = HasDownloads,
            HasIssues = HasIssues,
            HasProjects = HasProjects,
            HasWiki = HasWiki,
            Homepage = Homepage,
            Description = Description,
            IsPrivate = IsPrivate,
            IsTemplate = IsTemplate,
            LicenseTemplate = LicenseTemplate,
            TeamId = TeamId,
            Name = Name,
            RepoName = RepoName,
            UseSquashPrTitleAsDefault = UseSquashPrTitleAsDefault,
            Visibility = Visibility,
            HtmlUrl = HtmlUrl,
            GitUrl = GitUrl,
            CreatedDateTime = CreatedDateTime,
            DeletedDateTime = DeletedDateTime,
            ModifiedDateTime = ModifiedDateTime
        };
    }
}
