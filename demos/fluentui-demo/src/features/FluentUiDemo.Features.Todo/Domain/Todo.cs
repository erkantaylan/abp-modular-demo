using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace FluentUiDemo.Domain;

public class Todo : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TodoStatus Status { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid? CompleterId { get; set; }

    public DateTime? CompletionTime { get; set; }
}
