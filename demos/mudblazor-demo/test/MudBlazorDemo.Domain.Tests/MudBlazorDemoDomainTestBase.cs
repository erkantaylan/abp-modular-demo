using Volo.Abp.Modularity;

namespace MudBlazorDemo;

/* Inherit from this class for your domain layer tests. */
public abstract class MudBlazorDemoDomainTestBase<TStartupModule> : MudBlazorDemoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
