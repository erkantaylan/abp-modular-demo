using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;

namespace FluentUiDemo.Blazor.Pages.Account;

public class FluentRegisterModel(
    IAccountAppService accountAppService,
    IAuthenticationSchemeProvider schemeProvider,
    IOptions<AbpAccountOptions> accountOptions,
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    : RegisterModel(accountAppService, schemeProvider, accountOptions, identityDynamicClaimsPrincipalContributorCache)
{
}
