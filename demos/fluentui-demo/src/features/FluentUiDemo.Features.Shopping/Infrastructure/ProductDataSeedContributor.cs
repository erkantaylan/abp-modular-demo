using System;
using System.Threading.Tasks;
using FluentUiDemo.Domain;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace FluentUiDemo.Infrastructure;

public class ProductDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductDataSeedContributor(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _productRepository.GetCountAsync() > 0)
        {
            return;
        }

        await _productRepository.InsertManyAsync(new[]
        {
            new Product
            {
                Name = "Wireless Headphones",
                Description = "Premium noise-cancelling wireless headphones with 30-hour battery life.",
                Price = 149.99m,
                Category = ProductCategory.Electronics,
                IsAvailable = true
            },
            new Product
            {
                Name = "Mechanical Keyboard",
                Description = "RGB mechanical keyboard with Cherry MX switches and aluminum frame.",
                Price = 89.99m,
                Category = ProductCategory.Electronics,
                IsAvailable = true
            },
            new Product
            {
                Name = "Running Shoes",
                Description = "Lightweight running shoes with responsive cushioning for everyday training.",
                Price = 119.99m,
                Category = ProductCategory.Sports,
                IsAvailable = true
            },
            new Product
            {
                Name = "Cotton T-Shirt",
                Description = "Soft organic cotton t-shirt, available in multiple colors.",
                Price = 24.99m,
                Category = ProductCategory.Clothing,
                IsAvailable = true
            },
            new Product
            {
                Name = "Design Patterns",
                Description = "Classic software engineering book on reusable object-oriented design patterns.",
                Price = 44.99m,
                Category = ProductCategory.Books,
                IsAvailable = true
            },
            new Product
            {
                Name = "Desk Lamp",
                Description = "LED desk lamp with adjustable brightness and color temperature.",
                Price = 39.99m,
                Category = ProductCategory.Home,
                IsAvailable = true
            },
            new Product
            {
                Name = "Yoga Mat",
                Description = "Non-slip exercise yoga mat with carrying strap, 6mm thick.",
                Price = 29.99m,
                Category = ProductCategory.Sports,
                IsAvailable = true
            },
            new Product
            {
                Name = "Coffee Beans",
                Description = "Premium Arabica coffee beans, medium roast, 1kg bag.",
                Price = 18.99m,
                Category = ProductCategory.Food,
                IsAvailable = true
            }
        });
    }
}
