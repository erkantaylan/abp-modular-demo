using System;
using FluentUiDemo.Domain;
using Volo.Abp.Application.Dtos;

namespace FluentUiDemo.Application;

public class TodoDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TodoStatus Status { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid? CompleterId { get; set; }

    public DateTime? CompletionTime { get; set; }

    public string? CreatorUserName { get; set; }

    public string? CompleterUserName { get; set; }
}
