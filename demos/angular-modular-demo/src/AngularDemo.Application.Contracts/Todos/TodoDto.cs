using System;
using Volo.Abp.Application.Dtos;

namespace AngularDemo.Todos;

public class TodoDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public Guid? CompletedBy { get; set; }
    public DateTime? CompletionTime { get; set; }
    public string? CreatorUserName { get; set; }
    public string? CompletedByUserName { get; set; }
}
