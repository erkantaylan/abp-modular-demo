using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace FluentUiDemo.Permissions;

public class ShoppingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var shoppingGroup = context.AddGroup(ShoppingPermissions.GroupName, L("Permission:Shopping"));

        var productsPermission = shoppingGroup.AddPermission(ShoppingPermissions.Products.Default, L("Permission:Shopping"));
        productsPermission.AddChild(ShoppingPermissions.Products.Create, L("Permission:Shopping.Create"));
        productsPermission.AddChild(ShoppingPermissions.Products.Edit, L("Permission:Shopping.Edit"));
        productsPermission.AddChild(ShoppingPermissions.Products.Delete, L("Permission:Shopping.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ShoppingResource>(name);
    }
}
