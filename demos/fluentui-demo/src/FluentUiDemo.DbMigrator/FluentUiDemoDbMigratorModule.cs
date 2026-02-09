using FluentUiDemo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace FluentUiDemo.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(FluentUiDemoEntityFrameworkCoreModule),
    typeof(FluentUiDemoApplicationContractsModule)
)]
public class FluentUiDemoDbMigratorModule : AbpModule
{
}
