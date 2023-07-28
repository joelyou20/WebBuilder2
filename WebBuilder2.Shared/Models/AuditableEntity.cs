namespace WebBuilder2.Shared.Models
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
