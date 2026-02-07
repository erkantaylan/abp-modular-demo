using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace LayeredDemo.Domain;

public class Todo : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TodoStatus Status { get; set; }

    public DateTime? DueDate { get; set; }
}
