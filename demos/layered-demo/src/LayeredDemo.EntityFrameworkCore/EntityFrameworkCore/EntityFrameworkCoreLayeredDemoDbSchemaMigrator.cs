using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LayeredDemo.Data;
using Volo.Abp.DependencyInjection;

namespace LayeredDemo.EntityFrameworkCore;

public class EntityFrameworkCoreLayeredDemoDbSchemaMigrator
    : ILayeredDemoDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreLayeredDemoDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the LayeredDemoDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<LayeredDemoDbContext>()
            .Database
            .MigrateAsync();
    }
}
