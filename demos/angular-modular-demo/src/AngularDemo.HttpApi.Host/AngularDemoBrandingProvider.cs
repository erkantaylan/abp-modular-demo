using Microsoft.Extensions.Localization;
using AngularDemo.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AngularDemo;

[Dependency(ReplaceServices = true)]
public class AngularDemoBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AngularDemoResource> _localizer;

    public AngularDemoBrandingProvider(IStringLocalizer<AngularDemoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
