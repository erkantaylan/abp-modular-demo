using LayeredDemo.Todos;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LayeredDemo.Features.Todo.Infrastructure;

public static class TodoEfCoreConfiguration
{
    public static void ConfigureTodoFeature(this ModelBuilder builder)
    {
        builder.Entity<Todos.Todo>(b =>
        {
            b.ToTable("AppTodos");
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(1024);
        });
    }
}
