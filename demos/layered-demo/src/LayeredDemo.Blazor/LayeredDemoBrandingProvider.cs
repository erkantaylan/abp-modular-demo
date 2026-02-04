using Microsoft.Extensions.Localization;
using LayeredDemo.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LayeredDemo.Blazor;

[Dependency(ReplaceServices = true)]
public class LayeredDemoBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<LayeredDemoResource> _localizer;

    public LayeredDemoBrandingProvider(IStringLocalizer<LayeredDemoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
