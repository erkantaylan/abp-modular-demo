using Volo.Abp.Modularity;

namespace LayeredDemo;

[DependsOn(
    typeof(LayeredDemoDomainModule),
    typeof(LayeredDemoTestBaseModule)
)]
public class LayeredDemoDomainTestModule : AbpModule
{

}
