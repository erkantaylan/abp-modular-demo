using Volo.Abp.Modularity;

namespace AngularDemo;

[DependsOn(
    typeof(AngularDemoApplicationModule),
    typeof(AngularDemoDomainTestModule)
)]
public class AngularDemoApplicationTestModule : AbpModule
{

}
