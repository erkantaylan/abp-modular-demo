using FluentUiDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace FluentUiDemo.Blazor;

public abstract class FluentUiDemoComponentBase : AbpComponentBase
{
    protected FluentUiDemoComponentBase()
    {
        LocalizationResource = typeof(FluentUiDemoResource);
    }
}
