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

        var todosPermission = myGroup.AddPermission(AngularDemoPermissions.Todos.Default, L("Permission:Todos"));
        todosPermission.AddChild(AngularDemoPermissions.Todos.Create, L("Permission:Todos.Create"));
        todosPermission.AddChild(AngularDemoPermissions.Todos.Complete, L("Permission:Todos.Complete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AngularDemoResource>(name);
    }
}
