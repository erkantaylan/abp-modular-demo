using System;
using MudBlazorDemo.Domain;
using Volo.Abp.Application.Dtos;

namespace MudBlazorDemo.Application;

public class TodoDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TodoStatus Status { get; set; }

    public DateTime? DueDate { get; set; }

    public string? CreatorUserName { get; set; }
}
