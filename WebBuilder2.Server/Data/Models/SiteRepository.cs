using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Server.Data.Models.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Data.Models
{
    public class SiteRepository : AuditableEntity, IEntity<SiteRepositoryModel>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SiteId { get; set; }
        [ForeignKey("SiteId")]
        public Site Site { get; set; } = default!;
        public long RepositoryId { get; set; }
        [ForeignKey("RepositoryId")]
        public Repository Repository { get; set; } = default!;

        public SiteRepositoryModel FromDto() => new()
        {
            Id = Id,
            SiteId = Id,
            RepositoryId = RepositoryId,
            CreatedDateTime = CreatedDateTime,
            DeletedDateTime = DeletedDateTime,
            ModifiedDateTime = ModifiedDateTime
        };
    }
}
