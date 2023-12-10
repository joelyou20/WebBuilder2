using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Server.Data.Models.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Data.Models;


public class Log : AuditableEntity, IEntity<LogModel>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public LogType Type { get; set; } = LogType.Information;
    public string Message { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string Exception { get; set; } = string.Empty;

    public LogModel FromDto() => new() 
    { 
        Id = Id, 
        Message = Message, 
        StackTrace = StackTrace,
        Exception = Exception
    };
}
