using System.ComponentModel.DataAnnotations;
using FluentUiDemo.Domain;

namespace FluentUiDemo.Application;

public class CreateUpdateProductDto
{
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = null!;

    [StringLength(1024)]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [StringLength(512)]
    public string? ImageUrl { get; set; }

    public ProductCategory Category { get; set; }

    public bool IsAvailable { get; set; } = true;
}
