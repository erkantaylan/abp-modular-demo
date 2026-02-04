using Volo.Abp.Settings;

namespace LayeredDemo.Settings;

public class LayeredDemoSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(LayeredDemoSettings.MySetting1));
    }
}
