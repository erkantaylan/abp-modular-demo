using System.ComponentModel.DataAnnotations;

namespace AngularDemo.Todos;

public class CreateTodoDto
{
    [Required]
    [StringLength(256)]
    public string Title { get; set; } = null!;

    [StringLength(1024)]
    public string? Description { get; set; }
}
