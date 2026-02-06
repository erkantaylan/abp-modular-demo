using System;
using System.ComponentModel.DataAnnotations;

namespace LayeredDemo.Todos;

public class CreateUpdateTodoDto
{
    [Required]
    [StringLength(256)]
    public string Title { get; set; } = null!;

    [StringLength(1024)]
    public string? Description { get; set; }

    public TodoStatus Status { get; set; }

    public DateTime? DueDate { get; set; }
}
