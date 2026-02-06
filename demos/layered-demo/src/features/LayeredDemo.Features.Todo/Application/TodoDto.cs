using System;
using Volo.Abp.Application.Dtos;

namespace LayeredDemo.Todos;

public class TodoDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TodoStatus Status { get; set; }

    public DateTime? DueDate { get; set; }

    public string? CreatorUserName { get; set; }
}
