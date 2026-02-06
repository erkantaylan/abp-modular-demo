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

        var todosPermission = myGroup.AddPermission(LayeredDemoPermissions.Todos.Default, L("Permission:Todos"));
        todosPermission.AddChild(LayeredDemoPermissions.Todos.Create, L("Permission:Todos.Create"));
        todosPermission.AddChild(LayeredDemoPermissions.Todos.Edit, L("Permission:Todos.Edit"));
        todosPermission.AddChild(LayeredDemoPermissions.Todos.Delete, L("Permission:Todos.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LayeredDemoResource>(name);
    }
}
