using Volo.Abp.Modularity;

namespace LayeredDemo;

/* Inherit from this class for your domain layer tests. */
public abstract class LayeredDemoDomainTestBase<TStartupModule> : LayeredDemoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
