using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Data.Models
{
    public class SiteDTO : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DeletedDateTime { get; set; }

        public Site FromDto() => new() 
        { 
            Id = Id, 
            Name = Name,
            CreatedDateTime = CreatedDateTime,
            ModifiedDateTime = ModifiedDateTime,
            DeletedDateTime = DeletedDateTime,
        };
    }
}
