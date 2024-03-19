using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Shared.Models;
using WebBuilder2.Server.Data.Models.Contracts;

namespace WebBuilder2.Server.Data.Models
{
    // Top-level entity
    public class Site : AuditableEntity, IEntity<SiteModel>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long SiteRepositoryId { get; set; }
        public SiteRepository? SiteRepository { get; set; }
        public DateTime? SSLCertificateIssueDate { get; set; }
        public string? SSLARN { get; set; }
        public Region Region { get; set; } = Region.USEast1;

        public SiteModel FromDto() => new() 
        { 
            Id = Id, 
            Name = Name,
            Description = Description,
            SiteRepository = SiteRepository?.FromDto(),
            CreatedDateTime = CreatedDateTime,
            ModifiedDateTime = ModifiedDateTime,
            DeletedDateTime = DeletedDateTime,
            SSLCertificateIssueDate = SSLCertificateIssueDate,
            SSLARN = SSLARN,
            Region = Region,
            SiteRepositoryId = SiteRepositoryId
        };
    }
}
