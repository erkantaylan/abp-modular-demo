using Volo.Abp.Settings;

namespace MudBlazorDemo.Settings;

public class MudBlazorDemoSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MudBlazorDemoSettings.MySetting1));
    }
}
