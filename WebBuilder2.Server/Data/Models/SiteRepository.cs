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

        public SiteRepositoryModel FromDto()
        {
            throw new NotImplementedException();
        }
    }
}
