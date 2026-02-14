using AngularDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace AngularDemo.Permissions;

public class AngularDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AngularDemoPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(AngularDemoPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AngularDemoResource>(name);
    }
}
