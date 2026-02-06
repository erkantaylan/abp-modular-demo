using LayeredDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LayeredDemo.Permissions;

public class LayeredDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        context.AddGroup(LayeredDemoPermissions.GroupName);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LayeredDemoResource>(name);
    }
}
