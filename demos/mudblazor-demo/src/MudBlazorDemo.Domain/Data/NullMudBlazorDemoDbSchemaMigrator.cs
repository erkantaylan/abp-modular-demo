using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MudBlazorDemo.Data;

/* This is used if database provider does't define
 * IMudBlazorDemoDbSchemaMigrator implementation.
 */
public class NullMudBlazorDemoDbSchemaMigrator : IMudBlazorDemoDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
