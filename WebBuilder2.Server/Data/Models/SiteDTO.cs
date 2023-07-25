using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Shared.Models;
using WebBuilder2.Server.Data.Models.Contracts;

namespace WebBuilder2.Server.Data.Models
{
    // Top-level entity
    public class SiteDTO : AuditableEntity, IDto<Site>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Repository Repository { get; set; } = default!;

        public Site FromDto() => new() 
        { 
            Id = Id, 
            Name = Name,
            Repository = Repository,
            CreatedDateTime = CreatedDateTime,
            ModifiedDateTime = ModifiedDateTime,
            DeletedDateTime = DeletedDateTime,
        };
    }
}
