using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MudBlazorDemo.Data;
using Volo.Abp.DependencyInjection;

namespace MudBlazorDemo.EntityFrameworkCore;

public class EntityFrameworkCoreMudBlazorDemoDbSchemaMigrator
    : IMudBlazorDemoDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreMudBlazorDemoDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the MudBlazorDemoDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<MudBlazorDemoDbContext>()
            .Database
            .MigrateAsync();
    }
}
