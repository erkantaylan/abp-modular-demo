using LayeredDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace LayeredDemo.Blazor;

public abstract class LayeredDemoComponentBase : AbpComponentBase
{
    protected LayeredDemoComponentBase()
    {
        LocalizationResource = typeof(LayeredDemoResource);
    }
}
