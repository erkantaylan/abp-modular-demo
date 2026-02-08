using Microsoft.Extensions.Localization;
using MudBlazorDemo.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace MudBlazorDemo.Blazor;

[Dependency(ReplaceServices = true)]
public class MudBlazorDemoBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<MudBlazorDemoResource> _localizer;

    public MudBlazorDemoBrandingProvider(IStringLocalizer<MudBlazorDemoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
