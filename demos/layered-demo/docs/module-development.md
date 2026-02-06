# Feature Module Development Guide

This guide explains how to create self-contained ABP feature modules following the pattern established by the Todo module.

## Why Feature Modules?

Instead of scattering a feature across 8+ layered projects, each business feature lives in a single project:

```
src/features/LayeredDemo.Features.Todo/
    TodoFeatureModule.cs           # ABP module registration
    Domain/
        Todo.cs                    # Entity
        TodoStatus.cs              # Enum
    Application/
        ITodoAppService.cs         # Service interface
        TodoAppService.cs          # Service implementation
        TodoDto.cs                 # DTO
        CreateUpdateTodoDto.cs     # Input DTO
        TodoGetListInput.cs        # List query input
        TodoApplicationMappers.cs  # Mapperly mappers
    Permissions/
        TodoPermissions.cs         # Permission constants
        TodoPermissionDefinitionProvider.cs
    Infrastructure/
        TodoEfCoreConfiguration.cs # EF Core entity config
    Pages/
        Todos.razor                # Blazor page
    Menus/
        TodoMenuContributor.cs     # Navigation menu
    Localization/
        Todo/
            en.json                # Localization strings
```

**Benefits:**
- Open one folder to see the entire feature
- New developers learn one module without understanding the full architecture
- Feature changes don't touch shared infrastructure projects
- Modules can be extracted to NuGet packages later

## Creating a New Feature Module

### 1. Create the Project

```bash
mkdir -p src/features/LayeredDemo.Features.YourFeature
```

Use `Microsoft.NET.Sdk.Razor` SDK (needed for Blazor pages):

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <Import Project="..\..\common.props" />
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <RootNamespace>LayeredDemo</RootNamespace>
    <GenerateEmbeddedFilesManifest>true</GenerateEmbeddedFilesManifest>
  </PropertyGroup>
  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Volo.Abp.Ddd.Application" Version="10.0.2" />
    <PackageReference Include="Volo.Abp.EntityFrameworkCore" Version="10.0.2" />
    <PackageReference Include="Volo.Abp.BlazoriseUI" Version="10.0.2" />
    <PackageReference Include="Volo.Abp.AspNetCore.Components.Web" Version="10.0.2" />
    <PackageReference Include="Volo.Abp.Mapperly" Version="10.0.2" />
    <PackageReference Include="Blazorise.Bootstrap5" Version="1.8.8" />
    <PackageReference Include="Blazorise.Icons.FontAwesome" Version="1.8.8" />
    <PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="10.0.0" />
    <!-- Add domain-specific packages as needed -->
  </ItemGroup>
  <ItemGroup>
    <EmbeddedResource Include="Localization\YourFeature\*.json" />
    <Content Remove="Localization\YourFeature\*.json" />
  </ItemGroup>
</Project>
```

### 2. Create the ABP Module Class

```csharp
[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpBlazoriseUIModule),
    typeof(AbpAspNetCoreComponentsWebModule)
)]
public class YourFeatureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Virtual file system for embedded localization
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<YourFeatureModule>();
        });

        // Localization resource
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<YourFeatureResource>("en")
                .AddVirtualJson("/Localization/YourFeature");
        });

        // Menu registration
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new YourFeatureMenuContributor());
        });

        // Blazor component routing
        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(YourFeatureModule).Assembly);
        });
    }
}
```

### 3. Define the Entity (Domain/)

Use ABP's base classes for built-in auditing:
- `FullAuditedAggregateRoot<Guid>` - tracks creator, modifier, deleter + soft delete
- `AuditedAggregateRoot<Guid>` - tracks creator, modifier (no soft delete)
- `CreationAuditedAggregateRoot<Guid>` - tracks creator only

### 4. Create EF Core Configuration (Infrastructure/)

Provide a `ModelBuilder` extension method:

```csharp
public static class YourFeatureEfCoreConfiguration
{
    public static void ConfigureYourFeature(this ModelBuilder builder)
    {
        builder.Entity<YourEntity>(b =>
        {
            b.ToTable("AppYourEntities");
            b.ConfigureByConvention();
            // Property configurations...
        });
    }
}
```

The host DbContext calls this extension in `OnModelCreating`.

### 5. Add Permissions (Permissions/)

Define permission constants with string values that won't change:

```csharp
public static class YourFeaturePermissions
{
    public const string GroupName = "LayeredDemo.YourFeature";

    public static class Items
    {
        public const string Default = GroupName;
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
```

### 6. Protect Blazor Pages

Always add `@attribute [Authorize]` to prevent anonymous access:

```razor
@page "/your-feature"
@attribute [Authorize]
@inherits AbpCrudPageBase<...>
```

### 7. Register in the Host

Add a project reference and `[DependsOn]` in the host module:

```csharp
// In LayeredDemoBlazorModule.cs
[DependsOn(
    typeof(YourFeatureModule),
    // ... existing dependencies
)]
```

Call the EF Core configuration in the host DbContext:

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    // ... existing configurations
    builder.ConfigureYourFeature();
}
```

### 8. Add to Solution

Update `LayeredDemo.slnx`:

```xml
<Folder Name="/5. Features/">
    <Folder Name="/5. Features/YourFeature/">
        <Project Path="src/features/LayeredDemo.Features.YourFeature/..." />
    </Folder>
</Folder>
```

## Creating Tests

Create a test project at `test/features/LayeredDemo.Features.YourFeature.Tests/`:

```csharp
[DependsOn(
    typeof(YourFeatureModule),
    typeof(LayeredDemoEntityFrameworkCoreTestModule)
)]
public class YourFeatureTestModule : AbpModule { }
```

Tests inherit from `LayeredDemoEntityFrameworkCoreTestBase` which provides:
- In-memory SQLite database
- Authenticated test user (admin)
- Dependency injection

```bash
dotnet test test/features/LayeredDemo.Features.YourFeature.Tests/
```

## Infrastructure vs Features

| Code Type | Location | Reason |
|-----------|----------|--------|
| Identity, tenants, settings | Layered host projects | Shared infrastructure, rarely changes |
| Business features (Todo, etc.) | `src/features/` | Changes frequently, feature-scoped |
| Database migrations | `LayeredDemo.EntityFrameworkCore` | Single migration history for all entities |
| Data seeding | `LayeredDemo.Domain` | Cross-cutting (may seed multiple features) |
