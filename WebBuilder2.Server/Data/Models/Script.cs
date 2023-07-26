using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBuilder2.Server.Data.Models.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Data.Models;

public class Script : AuditableEntity, IEntity<ScriptModel>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;

    public ScriptModel FromDto() => new()
    {
        Id = Id,
        Name = Name,
        Data = Data,
        CreatedDateTime = CreatedDateTime,
        DeletedDateTime = DeletedDateTime,
        ModifiedDateTime = ModifiedDateTime
    };
}
