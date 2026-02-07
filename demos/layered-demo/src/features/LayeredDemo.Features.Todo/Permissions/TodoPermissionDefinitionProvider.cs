using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LayeredDemo.Permissions;

public class TodoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var todoGroup = context.AddGroup(TodoPermissions.GroupName, L("Permission:Todos"));

        var todosPermission = todoGroup.AddPermission(TodoPermissions.Todos.Default, L("Permission:Todos"));
        todosPermission.AddChild(TodoPermissions.Todos.Create, L("Permission:Todos.Create"));
        todosPermission.AddChild(TodoPermissions.Todos.Edit, L("Permission:Todos.Edit"));
        todosPermission.AddChild(TodoPermissions.Todos.Delete, L("Permission:Todos.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TodoResource>(name);
    }
}
