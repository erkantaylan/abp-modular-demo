using LayeredDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LayeredDemo.Permissions;

public class LayeredDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(LayeredDemoPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(LayeredDemoPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LayeredDemoResource>(name);
    }
}
