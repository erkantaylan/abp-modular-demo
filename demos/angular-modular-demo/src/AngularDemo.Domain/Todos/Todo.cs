using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace AngularDemo.Todos;

public class Todo : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public Guid? CompletedBy { get; set; }
    public DateTime? CompletionTime { get; set; }

    protected Todo() { }

    public Todo(Guid id, string title, string? description = null)
        : base(id)
    {
        Title = title;
        Description = description;
    }

    public void Complete(Guid userId)
    {
        IsCompleted = true;
        CompletedBy = userId;
        CompletionTime = DateTime.Now;
    }
}
