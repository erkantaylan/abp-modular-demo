using System.Threading.Tasks;
using FluentUiDemo.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;

namespace FluentUiDemo.Menus;

public class ShoppingMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var l = context.GetLocalizer<ShoppingResource>();

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "FluentUiDemo.Shopping",
                l["Menu:Shopping"],
                "/shopping",
                icon: "fas fa-shopping-cart",
                order: 3
            ).RequirePermissions(ShoppingPermissions.Products.Default)
        );

        return Task.CompletedTask;
    }
}
