using FluentUiDemo.Menus;
using FluentUiDemo.Services;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace FluentUiDemo;

[DependsOn(
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpAspNetCoreComponentsWebModule)
)]
public class ChatFeatureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<ChatService>();

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ChatFeatureModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ChatResource>("en")
                .AddVirtualJson("/Localization/Chat");
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ChatMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(ChatFeatureModule).Assembly);
        });
    }
}
