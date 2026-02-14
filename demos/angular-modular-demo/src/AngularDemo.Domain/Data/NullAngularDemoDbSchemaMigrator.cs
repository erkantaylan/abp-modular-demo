using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AngularDemo.Data;

/* This is used if database provider does't define
 * IAngularDemoDbSchemaMigrator implementation.
 */
public class NullAngularDemoDbSchemaMigrator : IAngularDemoDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
