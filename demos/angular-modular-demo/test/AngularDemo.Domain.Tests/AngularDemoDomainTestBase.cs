using Volo.Abp.Modularity;

namespace AngularDemo;

/* Inherit from this class for your domain layer tests. */
public abstract class AngularDemoDomainTestBase<TStartupModule> : AngularDemoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
