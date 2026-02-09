using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace FluentUiDemo.Data;

public class NullFluentUiDemoDbSchemaMigrator : IFluentUiDemoDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
