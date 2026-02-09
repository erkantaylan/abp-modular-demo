using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentUiDemo.Data;
using Volo.Abp.DependencyInjection;

namespace FluentUiDemo.EntityFrameworkCore;

public class EntityFrameworkCoreFluentUiDemoDbSchemaMigrator
    : IFluentUiDemoDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreFluentUiDemoDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        await _serviceProvider
            .GetRequiredService<FluentUiDemoDbContext>()
            .Database
            .MigrateAsync();
    }
}
