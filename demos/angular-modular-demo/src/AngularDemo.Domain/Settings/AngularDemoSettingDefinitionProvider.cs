using Volo.Abp.Settings;

namespace AngularDemo.Settings;

public class AngularDemoSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AngularDemoSettings.MySetting1));
    }
}
