using MudBlazorDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace MudBlazorDemo.Blazor;

public abstract class MudBlazorDemoComponentBase : AbpComponentBase
{
    protected MudBlazorDemoComponentBase()
    {
        LocalizationResource = typeof(MudBlazorDemoResource);
    }
}
