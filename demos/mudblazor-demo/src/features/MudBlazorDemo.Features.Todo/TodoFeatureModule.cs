using MudBlazorDemo.Menus;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace MudBlazorDemo;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpAspNetCoreComponentsWebModule)
)]
public class TodoFeatureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TodoFeatureModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<TodoResource>("en")
                .AddVirtualJson("/Localization/Todo");
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new TodoMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(TodoFeatureModule).Assembly);
        });
    }
}
