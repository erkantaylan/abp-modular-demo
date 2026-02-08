using MudBlazorDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace MudBlazorDemo.Permissions;

public class MudBlazorDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        context.AddGroup(MudBlazorDemoPermissions.GroupName);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MudBlazorDemoResource>(name);
    }
}
