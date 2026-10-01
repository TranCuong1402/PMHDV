namespace QlyDaoTao.Model.Entities
{
    public class AuditLog
    {
        public long Id { get; set; }

        public int? UserId { get; set; }

        public string Action { get; set; } = null!;

        public string EntityName { get; set; } = null!;

        public string? EntityId { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}