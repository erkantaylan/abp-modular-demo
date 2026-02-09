using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace FluentUiDemo.Domain;

public class Product : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public ProductCategory Category { get; set; }

    public bool IsAvailable { get; set; } = true;
}
