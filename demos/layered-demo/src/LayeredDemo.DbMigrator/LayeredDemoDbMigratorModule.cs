using LayeredDemo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LayeredDemo.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(LayeredDemoEntityFrameworkCoreModule),
    typeof(LayeredDemoApplicationContractsModule)
)]
public class LayeredDemoDbMigratorModule : AbpModule
{
}
