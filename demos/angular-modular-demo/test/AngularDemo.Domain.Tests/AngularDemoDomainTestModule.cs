using Volo.Abp.Modularity;

namespace AngularDemo;

[DependsOn(
    typeof(AngularDemoDomainModule),
    typeof(AngularDemoTestBaseModule)
)]
public class AngularDemoDomainTestModule : AbpModule
{

}
