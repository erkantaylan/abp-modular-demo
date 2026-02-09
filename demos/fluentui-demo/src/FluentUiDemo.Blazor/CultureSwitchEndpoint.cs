using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;

namespace FluentUiDemo.Blazor;

public static class CultureSwitchEndpoint
{
    public static IEndpointRouteBuilder MapCultureSwitch(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/culture/switch", (HttpContext httpContext, string culture, string? returnUrl) =>
        {
            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Results.LocalRedirect(returnUrl ?? "/");
        });

        return endpoints;
    }
}
