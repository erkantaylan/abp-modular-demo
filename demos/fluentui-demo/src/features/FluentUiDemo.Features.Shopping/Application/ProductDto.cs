using System;
using FluentUiDemo.Domain;
using Volo.Abp.Application.Dtos;

namespace FluentUiDemo.Application;

public class ProductDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public ProductCategory Category { get; set; }

    public bool IsAvailable { get; set; }
}
