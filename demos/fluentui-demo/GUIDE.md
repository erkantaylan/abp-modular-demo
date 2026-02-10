# FluentUI + ABP Framework Integration Guide

A step-by-step walkthrough of building a Blazor Server application with Microsoft FluentUI components on top of ABP Framework's modular architecture, replacing ABP's default Blazorise-based UI while keeping its MVC Account pages for authentication flows.

## Table of Contents

- [1. Starting from the ABP Layered Template](#1-starting-from-the-abp-layered-template)
- [2. Removing Blazorise and Adding FluentUI](#2-removing-blazorise-and-adding-fluentui)
- [3. Adding Bulma CSS for Layout Utilities](#3-adding-bulma-css-for-layout-utilities)
- [4. Setting Up the FluentUI Layout](#4-setting-up-the-fluentui-layout)
- [5. Localization (TR/EN)](#5-localization-tren)
- [6. Creating Feature Modules](#6-creating-feature-modules)
- [7. Aspire AppHost Configuration](#7-aspire-apphost-configuration)
- [8. Custom Account UI Replacement](#8-custom-account-ui-replacement)
- [9. External Login Setup (Google & GitHub)](#9-external-login-setup-google--github)
- [10. Common Issues & Solutions](#10-common-issues--solutions)

---

## 1. Starting from the ABP Layered Template

This project begins with ABP Framework v10's [Application (Layered) Solution Template](https://abp.io/docs/latest/get-started/layered-web-application) using Blazor Server as the UI framework and PostgreSQL as the database.

Generate the template:

```bash
abp new FluentUiDemo -t app -u blazor-server -d postgresql --version 10.0.2
```

This gives you the standard layered structure:

```
src/
├── FluentUiDemo.Domain.Shared/       # Shared constants, enums, localization
├── FluentUiDemo.Domain/              # Domain entities and aggregate roots
├── FluentUiDemo.Application.Contracts/ # DTOs and service interfaces
├── FluentUiDemo.Application/         # Application service implementations
├── FluentUiDemo.EntityFrameworkCore/  # EF Core DbContext and migrations
├── FluentUiDemo.HttpApi/             # REST API controllers
├── FluentUiDemo.HttpApi.Client/      # HTTP API client proxies
├── FluentUiDemo.Blazor/              # Main Blazor Server web application
└── FluentUiDemo.DbMigrator/          # Database migration console app
```

The default template ships with **Blazorise** for Blazor component rendering and the **Basic Theme** for both MVC and Blazor pages.

## 2. Removing Blazorise and Adding FluentUI

### The key insight: keep Basic Theme for MVC, replace Blazorise for Blazor

ABP's authentication pages (Login, Register, Forgot Password, etc.) are **MVC Razor Pages**, not Blazor components. They use ABP's MVC Basic Theme and require its CSS/JS bundles. The main application UI (Dashboard, Todo, Chat, Shopping) is Blazor and uses FluentUI components.

This means we keep both themes in the dependency list:

```xml
<!-- FluentUiDemo.Blazor.csproj -->

<!-- Keep: Required for MVC Account pages -->
<PackageReference Include="Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic" Version="10.0.2" />
<PackageReference Include="Volo.Abp.AspNetCore.Components.Server.BasicTheme" Version="10.0.2" />

<!-- Add: FluentUI Blazor components -->
<PackageReference Include="Microsoft.FluentUI.AspNetCore.Components" Version="4.13.2" />
<PackageReference Include="Microsoft.FluentUI.AspNetCore.Components.Icons" Version="4.13.2" />
```

### Register FluentUI services

In `FluentUiDemoBlazorModule.cs`, register FluentUI in `ConfigureServices`:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    // ...existing ABP configuration...

    // Register FluentUI services
    context.Services.AddFluentUIComponents();

    // ...rest of configuration...
}
```

### Add FluentUI CSS to App.razor

In `Components/App.razor`, add the FluentUI reboot stylesheet alongside ABP's bundled styles:

```html
<head>
    <!-- ABP bundled styles (needed for MVC pages) -->
    <AbpStyles BundleName="@BlazorBasicThemeBundles.Styles.Global" />

    <!-- Bulma CSS for layout utilities -->
    <link href="lib/bulma/bulma.min.css" rel="stylesheet" />

    <!-- FluentUI base reset -->
    <link href="_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css" rel="stylesheet" />

    <!-- Blazor component scoped styles -->
    <link href="FluentUiDemo.Blazor.styles.css" rel="stylesheet"/>

    <HeadOutlet @rendermode="InteractiveServer" />
</head>
```

The order matters: ABP styles load first (for MVC pages), then Bulma for layout, then FluentUI's reboot which normalizes elements for FluentUI components.

## 3. Adding Bulma CSS for Layout Utilities

FluentUI provides components (buttons, data grids, dialogs) but limited CSS layout utilities. [Bulma](https://bulma.io/) fills this gap with its column system and utility classes.

### Set up with yarn

```json
// package.json
{
  "name": "fluentui-demo",
  "version": "1.0.0",
  "private": true,
  "scripts": {
    "build:css": "cp node_modules/bulma/css/bulma.min.css src/FluentUiDemo.Blazor/wwwroot/lib/bulma/bulma.min.css"
  },
  "dependencies": {
    "bulma": "^1.0.4"
  }
}
```

Install and build:

```bash
cd demos/fluentui-demo
yarn install
yarn build:css
```

This copies `bulma.min.css` into the Blazor project's static assets at `wwwroot/lib/bulma/`.

### Usage in Blazor components

Use Bulma's column system for responsive layouts:

```razor
<!-- Product grid with Bulma columns -->
<div class="columns is-multiline">
    @foreach (var product in filteredProducts)
    {
        <div class="column is-one-third">
            <FluentCard>...</FluentCard>
        </div>
    }
</div>

<!-- Side-by-side layout -->
<div class="columns">
    <div class="column is-one-third">Sidebar</div>
    <div class="column">Main content</div>
</div>
```

## 4. Setting Up the FluentUI Layout

Replace ABP's default Blazor layout with a FluentUI-based one in `Components/Layout/MainLayout.razor`:

```razor
@inherits LayoutComponentBase
@using Microsoft.FluentUI.AspNetCore.Components

<FluentToastProvider />
<FluentDialogProvider />
<FluentTooltipProvider />

<FluentLayout>
    <FluentHeader>
        <FluentStack Orientation="Orientation.Horizontal"
                     HorizontalAlignment="HorizontalAlignment.Left"
                     VerticalAlignment="VerticalAlignment.Center"
                     Style="width: 100%;">
            <FluentButton Appearance="Appearance.Stealth" OnClick="@ToggleNavMenu"
                          IconStart="@(new Icons.Regular.Size24.Navigation())" />
            <FluentLabel Typo="Typography.H5" Style="margin-left: 12px; color: white;">
                @L["AppName"]
            </FluentLabel>
            <FluentSpacer />

            <!-- Language selector -->
            <FluentSelect TOption="LanguageInfo"
                          Items="@_languages"
                          SelectedOption="@_selectedLanguage"
                          SelectedOptionChanged="@OnLanguageChanged"
                          OptionText="@(l => l.DisplayName)"
                          Width="140px" />

            <!-- Auth-aware user menu -->
            <AuthorizeView>
                <Authorized>
                    <FluentLabel Style="color: white;">@context.User.Identity?.Name</FluentLabel>
                    <FluentAnchor Href="/Account/Logout" Appearance="Appearance.Stealth">
                        <FluentIcon Value="@(new Icons.Regular.Size20.SignOut())" Color="Color.Fill" />
                    </FluentAnchor>
                </Authorized>
                <NotAuthorized>
                    <FluentAnchor Href="/Account/Login" Appearance="Appearance.Accent">
                        @L["Login"]
                    </FluentAnchor>
                </NotAuthorized>
            </AuthorizeView>
        </FluentStack>
    </FluentHeader>

    <FluentStack Orientation="Orientation.Horizontal"
                 Style="height: calc(100vh - 50px); width: 100%;">
        @if (_navMenuExpanded)
        {
            <FluentNavMenu Width="250" Collapsible="true" @bind-Expanded="_navMenuExpanded"
                           Style="height: 100%; border-right: 1px solid var(--neutral-stroke-divider-rest);">
                <NavMenu />
            </FluentNavMenu>
        }
        <FluentBodyContent Style="padding: 24px; overflow-y: auto; flex: 1;">
            @Body
        </FluentBodyContent>
    </FluentStack>
</FluentLayout>
```

Key decisions:
- **FluentHeader** provides the top bar with navigation toggle, app name, language selector, and auth controls
- **FluentNavMenu** is collapsible (250px) with `NavMenu` component for navigation links
- **FluentBodyContent** fills the remaining space with scrolling
- Login/Logout links point to the MVC Account pages (`/Account/Login`, `/Account/Logout`)
- Language switching uses ABP's built-in `/culture/switch` endpoint

## 5. Localization (TR/EN)

### Structure

Each module maintains its own localization resources:

```
FluentUiDemo.Domain.Shared/Localization/FluentUiDemo/
├── en.json    # English (default)
└── tr.json    # Turkish

features/FluentUiDemo.Features.Todo/Localization/Todo/
├── en.json
└── tr.json

features/FluentUiDemo.Features.Chat/Localization/Chat/
├── en.json
└── tr.json

features/FluentUiDemo.Features.Shopping/Localization/Shopping/
├── en.json
└── tr.json
```

### Registration

Each module registers its localization resources via the virtual file system:

```csharp
// In TodoFeatureModule.cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpVirtualFileSystemOptions>(options =>
    {
        options.FileSets.AddEmbedded<TodoFeatureModule>();
    });

    Configure<AbpLocalizationOptions>(options =>
    {
        options.Resources
            .Add<TodoResource>("en")          // Default language
            .AddVirtualJson("/Localization/Todo");  // Path to JSON files
    });
}
```

### Usage in Blazor components

```razor
@inject IStringLocalizer<TodoResource> L

<FluentLabel>@L["Todos"]</FluentLabel>
<FluentButton>@L["CreateNewTodo"]</FluentButton>
```

### Language switching

The `MainLayout.razor` integrates ABP's culture switching via a `FluentSelect` dropdown. When a user selects a language, the app navigates to ABP's culture switch endpoint:

```csharp
NavigationManager.NavigateTo(
    $"/culture/switch?culture={cultureEscaped}&returnUrl={uriEscaped}",
    forceLoad: true);
```

### Adding a new language

1. Copy `en.json` to `<code>.json` (e.g., `de.json`) in each module's `Localization/` directory
2. Translate the string values
3. ABP discovers new JSON resources at runtime automatically

## 6. Creating Feature Modules

Feature modules are self-contained ABP modules that bundle their own domain entities, application services, EF Core configuration, UI components, localization, and permissions.

### Module structure

```
features/FluentUiDemo.Features.Todo/
├── Domain/
│   ├── Todo.cs                      # Entity
│   └── TodoStatus.cs                # Enum
├── Application/
│   ├── TodoAppService.cs            # CRUD operations
│   ├── ITodoAppService.cs           # Interface
│   ├── TodoDto.cs                   # DTO
│   ├── CreateUpdateTodoDto.cs       # Input DTO
│   └── TodoApplicationMappers.cs    # Mapperly source-generated mapper
├── Infrastructure/
│   └── TodoEfCoreConfiguration.cs   # EF Core entity configuration
├── Pages/
│   └── Todos.razor                  # FluentUI page
├── Permissions/
│   ├── TodoPermissions.cs
│   └── TodoPermissionDefinitionProvider.cs
├── Menus/
│   └── TodoMenuContributor.cs       # Adds menu item
├── Localization/Todo/
│   ├── en.json
│   └── tr.json
└── TodoFeatureModule.cs             # Module definition
```

### Module definition

```csharp
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
        // Embedded resources (localization JSON, etc.)
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TodoFeatureModule>();
        });

        // Localization
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<TodoResource>("en")
                .AddVirtualJson("/Localization/Todo");
        });

        // Menu
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new TodoMenuContributor());
        });

        // Router - register this assembly for Blazor page discovery
        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(TodoFeatureModule).Assembly);
        });
    }
}
```

### Wiring into the main app

Add the feature module as a dependency in `FluentUiDemoBlazorModule.cs`:

```csharp
[DependsOn(
    // ...other dependencies...
    typeof(TodoFeatureModule),
    typeof(ChatFeatureModule),
    typeof(ShoppingFeatureModule)
)]
public class FluentUiDemoBlazorModule : AbpModule { }
```

And add the project reference:

```xml
<!-- FluentUiDemo.Blazor.csproj -->
<ProjectReference Include="..\features\FluentUiDemo.Features.Todo\FluentUiDemo.Features.Todo.csproj" />
```

### Mapperly for object mapping

Instead of AutoMapper, this project uses [Mapperly](https://mapperly.riok.app/) for source-generated, zero-reflection mapping:

```csharp
[Mapper]
public static partial class TodoApplicationMappers
{
    public static partial TodoDto ToDto(this Todo entity);
    public static partial Todo ToEntity(this CreateUpdateTodoDto dto);
}
```

## 7. Aspire AppHost Configuration

.NET Aspire orchestrates the entire stack: PostgreSQL database, EF Core migrations, and the Blazor application.

### AppHost Program.cs

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL with pgAdmin UI and persistent data volume
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume("fluentui-demo-postgres-data")
    .PublishAsContainer();

var database = postgres.AddDatabase("Default");

// DbMigrator runs EF Core migrations and seeds data
var dbMigrator = builder.AddProject<Projects.FluentUiDemo_DbMigrator>("dbmigrator")
    .WithReference(database)
    .WaitFor(database)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);

// Blazor app starts AFTER migrations complete
builder.AddProject<Projects.FluentUiDemo_Blazor>("blazor")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitForCompletion(dbMigrator)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);

builder.Build().Run();
```

Key points:
- **`WaitFor(database)`**: DbMigrator waits for PostgreSQL to be healthy before running migrations
- **`WaitForCompletion(dbMigrator)`**: Blazor app waits for DbMigrator to finish (exit) before starting
- **`WithEnvironment`**: Explicitly propagates the environment name to child projects (see [Common Issues](#aspire-environment-propagation))
- **`WithDataVolume`**: PostgreSQL data persists across container restarts
- **`WithExternalHttpEndpoints`**: Blazor app is accessible from outside the Aspire network

### ServiceDefaults

The `FluentUiDemo.ServiceDefaults` project provides shared Aspire configuration:
- OpenTelemetry (metrics, tracing, logging)
- Health check endpoints (`/health-status`, `/health-ui`)
- HTTP client resilience with Polly
- Service discovery

## 8. Custom Account UI Replacement

### Why replace Account pages?

ABP's default Account module uses Blazorise components and the Basic Theme's MVC layout. Since we're using FluentUI for the main app, the Account pages would look visually inconsistent. We replace them with custom-styled MVC Razor Pages that match our design.

### How ABP page overriding works

ABP's Account pages (`Login.cshtml`, `Register.cshtml`, etc.) are delivered via NuGet packages using ABP's Virtual File System. To override them, create pages at the **same path** in your project:

```
src/FluentUiDemo.Blazor/
└── Pages/
    └── Account/
        ├── Login.cshtml          # Overrides ABP's Login page
        ├── Login.cshtml.cs
        ├── Register.cshtml       # Overrides ABP's Register page
        ├── Register.cshtml.cs
        ├── ForgotPassword.cshtml
        ├── ForgotPassword.cshtml.cs
        ├── ResetPassword.cshtml
        ├── ResetPassword.cshtml.cs
        ├── Logout.cshtml
        └── Logout.cshtml.cs
```

ASP.NET Core's Razor Pages routing finds these **before** ABP's virtual file system versions, so your custom pages take priority.

### Inheriting ABP's page logic

The code-behind classes inherit from ABP's base models to reuse all authentication logic:

```csharp
// Login.cshtml.cs
public class FluentLoginModel(
    IAuthenticationSchemeProvider schemeProvider,
    IOptions<AbpAccountOptions> accountOptions,
    IOptions<IdentityOptions> identityOptions,
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
    IWebHostEnvironment environment)
    : LoginModel(schemeProvider, accountOptions, identityOptions,
        identityDynamicClaimsPrincipalContributorCache, environment)
{
}
```

```csharp
// Register.cshtml.cs
public class FluentRegisterModel(
    IAccountAppService accountAppService,
    IAuthenticationSchemeProvider schemeProvider,
    IOptions<AbpAccountOptions> accountOptions,
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    : RegisterModel(accountAppService, schemeProvider, accountOptions,
        identityDynamicClaimsPrincipalContributorCache)
{
}
```

This gives you full access to `Model.LoginInput`, `Model.VisibleExternalProviders`, `Model.EnableLocalLogin`, etc. without reimplementing any authentication logic.

### Standalone HTML layout

Each Account page sets `Layout = null` and provides its own complete HTML document. This means Account pages are completely independent from the Blazor layout:

```html
@page
@model FluentUiDemo.Blazor.Pages.Account.FluentLoginModel
@{
    Layout = null;
}
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@L["Login"] - FluentUI Demo</title>
    <base href="/" />
    <!-- FluentUI reboot for consistent base styling -->
    <link href="_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css" rel="stylesheet" />
    <!-- Custom account page styles -->
    <link href="account-styles.css" rel="stylesheet" />
</head>
<body>
    <!-- Custom login form here -->
</body>
</html>
```

The `account-styles.css` file contains all styling for login/register pages (form layout, buttons, input fields, responsive design). No ABP theme dependencies are needed.

### External provider buttons on Login/Register

The login page dynamically renders buttons for configured external providers:

```html
@if (Model.VisibleExternalProviders.Any())
{
    <div class="divider">@L["OrLoginWith"]</div>
    <form asp-page="./Login" asp-page-handler="ExternalLogin"
          asp-route-returnUrl="@Model.ReturnUrl"
          asp-route-returnUrlHash="@Model.ReturnUrlHash"
          method="post">
        <div class="external-providers">
            @foreach (var provider in Model.VisibleExternalProviders)
            {
                <button type="submit" class="btn-external"
                        name="provider"
                        value="@provider.AuthenticationScheme">
                    @provider.DisplayName
                </button>
            }
        </div>
    </form>
}
```

Providers only appear when their `ClientId` is configured (non-empty). The `ExternalLogin` page handler is inherited from ABP's `LoginModel` and handles the entire OAuth redirect flow.

## 9. External Login Setup (Google & GitHub)

### Overview

External login providers are configured **conditionally** in `FluentUiDemoBlazorModule.cs`. When a provider's `ClientId` is empty, it is not registered and won't appear on the login page.

### NuGet packages

```xml
<!-- FluentUiDemo.Blazor.csproj -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.Google" Version="10.0.2" />
<PackageReference Include="AspNet.Security.OAuth.GitHub" Version="10.0.0" />
```

### Configuration in appsettings.json

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "",
      "ClientSecret": ""
    },
    "GitHub": {
      "ClientId": "",
      "ClientSecret": ""
    }
  }
}
```

Leave `ClientId` empty to disable a provider. Fill in credentials to enable it.

### Registration code

```csharp
// FluentUiDemoBlazorModule.cs - ConfigureAuthentication method
private void ConfigureAuthentication(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();

    context.Services.ForwardIdentityAuthenticationForBearer(
        OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
    {
        options.IsDynamicClaimsEnabled = true;
    });

    // Google OAuth - only registered when ClientId is non-empty
    if (!configuration["Authentication:Google:ClientId"].IsNullOrWhiteSpace())
    {
        context.Services.AddAuthentication()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
            });
    }

    // GitHub OAuth - only registered when ClientId is non-empty
    if (!configuration["Authentication:GitHub:ClientId"].IsNullOrWhiteSpace())
    {
        context.Services.AddAuthentication()
            .AddGitHub(GitHubAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = configuration["Authentication:GitHub:ClientId"]!;
                options.ClientSecret = configuration["Authentication:GitHub:ClientSecret"]!;
                options.Scope.Add("user:email");
            });
    }
}
```

### Setting up Google OAuth

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project (or select existing)
3. Navigate to **APIs & Services > Credentials**
4. Click **Create Credentials > OAuth client ID**
5. Select **Web application** as application type
6. Add authorized redirect URI: `https://localhost:44379/signin-google`
   - For production, use your actual domain: `https://yourdomain.com/signin-google`
7. Copy the **Client ID** and **Client Secret**
8. Add to `appsettings.json`:

```json
"Google": {
  "ClientId": "your-client-id.apps.googleusercontent.com",
  "ClientSecret": "your-client-secret"
}
```

> **Note:** You may also need to configure the OAuth consent screen under **APIs & Services > OAuth consent screen**.

### Setting up GitHub OAuth

1. Go to [GitHub Developer Settings](https://github.com/settings/developers)
2. Click **New OAuth App**
3. Fill in:
   - **Application name**: FluentUI Demo (or your app name)
   - **Homepage URL**: `https://localhost:44379`
   - **Authorization callback URL**: `https://localhost:44379/signin-github`
4. Click **Register application**
5. Copy the **Client ID**
6. Generate a **Client Secret** and copy it
7. Add to `appsettings.json`:

```json
"GitHub": {
  "ClientId": "your-github-client-id",
  "ClientSecret": "your-github-client-secret"
}
```

### How account linking works with ABP Identity

When a user logs in with an external provider for the first time:

1. ABP redirects to the provider's OAuth page
2. After consent, the provider redirects back to `/signin-google` or `/signin-github`
3. ABP's `ExternalLogin` handler processes the callback
4. If no local account exists with the external email, ABP redirects to the Register page with the external login info pre-filled
5. The user completes registration (username, email)
6. ABP creates a local `IdentityUser` and links the external login to it
7. On subsequent logins, ABP finds the linked account and signs in directly

Existing users can link external providers from their account settings. ABP stores the provider link in the `AbpUserLogins` table.

### Testing the external login flow

1. Start the application
2. Go to the Login page
3. You should see "Google" and/or "GitHub" buttons below the login form
4. Click a provider button
5. Complete the OAuth flow on the provider's site
6. You'll be redirected back to register (first time) or logged in (returning user)

## 10. Common Issues & Solutions

### OpenIddict certificate error (dev vs production)

**Symptom**: `InvalidOperationException: No signing certificate was found` or `No encryption certificate was found` in production.

**Cause**: In development, ABP auto-generates temporary signing/encryption certificates. In production, you must provide a real certificate.

**Solution**: The module handles this with environment-based configuration:

```csharp
// FluentUiDemoBlazorModule.cs
if (!hostingEnvironment.IsDevelopment())
{
    PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
    {
        options.AddDevelopmentEncryptionAndSigningCertificate = false;
    });

    PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
    {
        serverBuilder.AddProductionEncryptionAndSigningCertificate(
            "openiddict.pfx",
            configuration["AuthServer:CertificatePassPhrase"]!);
        serverBuilder.SetIssuer(new Uri(configuration["AuthServer:Authority"]!));
    });
}
```

For production deployment:
1. Generate a PFX certificate: `dotnet dev-certs https -ep openiddict.pfx -p YourPassPhrase`
2. Place `openiddict.pfx` in the Blazor project root
3. Set `AuthServer:CertificatePassPhrase` in your production configuration
4. The `.csproj` conditionally embeds it: `<ItemGroup Condition="Exists('./openiddict.pfx')">`

### ABP client libraries (`abp install-libs`)

**Symptom**: Missing JavaScript/CSS files for ABP MVC pages, or build warnings about client libraries.

**Cause**: ABP MVC pages (Account module) need client-side libraries (jQuery, Bootstrap for validation, etc.) that are managed by ABP's `abp install-libs` CLI command.

**Solution**: Run `abp install-libs` from the Blazor project directory:

```bash
cd src/FluentUiDemo.Blazor
abp install-libs
```

This populates `wwwroot/libs/` with the required client libraries. These are only needed for MVC pages (Account, Tenant Management). The FluentUI Blazor pages don't use them.

> **Note:** Our custom Account pages use `Layout = null` and their own `account-styles.css`, so they don't depend on the full ABP JavaScript bundles. However, `abp install-libs` is still needed for ABP's internal MVC middleware to function correctly.

### Aspire environment propagation

**Symptom**: Application behaves as if in Production mode when running via Aspire, even though you launched with `ASPNETCORE_ENVIRONMENT=Development`.

**Cause**: Aspire-launched projects don't automatically inherit the parent's environment. Without explicit propagation, child projects default to Production.

**Solution**: Explicitly pass the environment in the AppHost:

```csharp
// AppHost Program.cs
var dbMigrator = builder.AddProject<Projects.FluentUiDemo_DbMigrator>("dbmigrator")
    .WithReference(database)
    .WaitFor(database)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);

builder.AddProject<Projects.FluentUiDemo_Blazor>("blazor")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitForCompletion(dbMigrator)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);
```

Without this, you'll see issues like:
- OpenIddict requiring a production certificate in development
- Detailed error pages not appearing
- Developer exception page not loading

### FileProviders.Embedded version mismatch after ABP update

**Symptom**: `MissingMethodException` or `TypeLoadException` related to `Microsoft.Extensions.FileProviders.Embedded` after upgrading ABP packages.

**Cause**: ABP packages may depend on a specific version of `Microsoft.Extensions.FileProviders.Embedded` that conflicts with what your .NET SDK provides.

**Solution**: Add an explicit package reference to match the .NET SDK version:

```xml
<!-- If needed, pin the version to match your SDK -->
<PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="10.0.0" />
```

Or update all ABP packages to the latest patch version that resolves the conflict:

```bash
dotnet list package --outdated
# Update all Volo.Abp.* packages to the same version
```

### HTTPS and development certificates

**Symptom**: OpenIddict validation fails with transport security errors when running locally.

**Cause**: Development environments may not have trusted HTTPS certificates.

**Solution**: The module supports disabling HTTPS requirement via configuration:

```json
// appsettings.Development.json
{
  "AuthServer": {
    "RequireHttpsMetadata": false
  }
}
```

```csharp
// FluentUiDemoBlazorModule.cs
if (!configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata"))
{
    Configure<OpenIddictServerAspNetCoreOptions>(options =>
    {
        options.DisableTransportSecurityRequirement = true;
    });
}
```

> **Warning:** Never disable HTTPS in production. This setting is for local development only.

Alternatively, trust the .NET development certificate:

```bash
dotnet dev-certs https --trust
```
