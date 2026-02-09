using FluentUiDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace FluentUiDemo.Infrastructure;

public static class ShoppingEfCoreConfiguration
{
    public static void ConfigureShoppingFeature(this ModelBuilder builder)
    {
        builder.Entity<Product>(b =>
        {
            b.ToTable("AppProducts");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(1024);
            b.Property(x => x.Price).HasPrecision(18, 2);
            b.Property(x => x.ImageUrl).HasMaxLength(512);
        });
    }
}
