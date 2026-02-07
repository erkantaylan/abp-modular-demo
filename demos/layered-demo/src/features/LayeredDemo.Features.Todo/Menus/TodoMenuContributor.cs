using System.Threading.Tasks;
using LayeredDemo.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;

namespace LayeredDemo.Menus;

public class TodoMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var l = context.GetLocalizer<TodoResource>();

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "LayeredDemo.Todos",
                l["Menu:Todos"],
                "/todos",
                icon: "fas fa-list-check",
                order: 2
            ).RequirePermissions(TodoPermissions.Todos.Default)
        );

        return Task.CompletedTask;
    }
}
