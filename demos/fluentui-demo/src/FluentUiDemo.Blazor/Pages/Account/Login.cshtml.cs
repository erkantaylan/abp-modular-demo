using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;

namespace FluentUiDemo.Blazor.Pages.Account;

public class FluentLoginModel(
    IAuthenticationSchemeProvider schemeProvider,
    IOptions<AbpAccountOptions> accountOptions,
    IOptions<IdentityOptions> identityOptions,
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
    IWebHostEnvironment environment)
    : LoginModel(schemeProvider, accountOptions, identityOptions, identityDynamicClaimsPrincipalContributorCache, environment)
{
}
