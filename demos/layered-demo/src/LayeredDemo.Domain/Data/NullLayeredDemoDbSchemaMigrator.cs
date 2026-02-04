using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LayeredDemo.Data;

/* This is used if database provider does't define
 * ILayeredDemoDbSchemaMigrator implementation.
 */
public class NullLayeredDemoDbSchemaMigrator : ILayeredDemoDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
