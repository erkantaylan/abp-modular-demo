# MudBlazor Demo

ABP Framework layered demo using **MudBlazor** as the UI component library instead of LeptonX Lite / Blazorise.

This proves that ABP's modular monolith architecture works with any Blazor component library -- not just ABP's commercial themes.

## Tech Stack

- .NET 10
- ABP Framework 10.0
- Blazor Server (Interactive)
- **MudBlazor** (Material Design component library)
- ABP Basic Theme (for MVC/account pages)
- PostgreSQL
- OpenIddict
- .NET Aspire (optional)

## What's Different from layered-demo

| Aspect | layered-demo | mudblazor-demo |
|--------|-------------|----------------|
| Blazor UI | LeptonX Lite + Blazorise | **MudBlazor** |
| Layout | LeptonX MainLayout | Custom MudBlazor layout (MudAppBar, MudDrawer, MudMainContent) |
| Navigation | LeptonX nav menu | MudNavMenu with MudNavLink/MudNavGroup |
| Todo list | Blazorise DataGrid | **MudTable** |
| Todo form | Blazorise Modal + Form | **MudDialog + MudForm** |
| Status display | Bootstrap badges | **MudChip** |
| Actions | Blazorise EntityActions | **MudMenu** with MudMenuItem |
| Date picker | Blazorise DateEdit | **MudDatePicker** |
| Notifications | (none) | **MudSnackbar** |

## Quick Start

**Prerequisites:** .NET 10 SDK, Node.js v18/v20, PostgreSQL 14+, [ABP CLI](https://abp.io/docs/latest/cli)

```bash
git clone git@github.com:erkantaylan/abp-modular-demo.git
cd abp-modular-demo/demos/mudblazor-demo
make setup   # restore + install-libs + migrate
make cert    # generate OpenIddict certificate
make run     # start the app
```

App runs at: **https://localhost:44378**

Login: `admin` / `1q2w3E*`

## Run Methods

### Standard
```bash
dotnet run --project src/MudBlazorDemo.Blazor
```

### With .NET Aspire
```bash
dotnet run --project src/MudBlazorDemo.AppHost
```

### With Hot Reload
```bash
make watch
```

## Project Structure

```
demos/mudblazor-demo/
├── src/
│   ├── MudBlazorDemo.Blazor/              <- MudBlazor UI host
│   │   ├── Components/
│   │   │   ├── Layout/
│   │   │   │   ├── MainLayout.razor       <- MudBlazor layout
│   │   │   │   └── NavMenu.razor          <- MudBlazor nav
│   │   │   ├── Pages/Index.razor          <- MudBlazor home page
│   │   │   ├── App.razor
│   │   │   └── Routes.razor
│   │   └── MudBlazorDemoBlazorModule.cs   <- AddMudServices()
│   ├── MudBlazorDemo.Domain/
│   ├── MudBlazorDemo.Domain.Shared/
│   ├── MudBlazorDemo.Application/
│   ├── MudBlazorDemo.Application.Contracts/
│   ├── MudBlazorDemo.EntityFrameworkCore/
│   ├── MudBlazorDemo.HttpApi/
│   ├── MudBlazorDemo.HttpApi.Client/
│   ├── MudBlazorDemo.DbMigrator/
│   ├── MudBlazorDemo.AppHost/
│   ├── MudBlazorDemo.ServiceDefaults/
│   └── features/
│       └── MudBlazorDemo.Features.Todo/   <- Todo with MudBlazor components
│           └── Pages/Todos.razor          <- MudTable, MudDialog, MudForm
├── test/
├── Makefile
└── README.md
```

## Key Integration Points

### 1. MudBlazor Service Registration
In `MudBlazorDemoBlazorModule.cs`:
```csharp
context.Services.AddMudServices();
```

### 2. CSS/JS in App.razor
```html
<link href="_content/MudBlazor/MudBlazor.min.css" rel="stylesheet" />
<script src="_content/MudBlazor/MudBlazor.min.js"></script>
```

### 3. Required Providers in MainLayout.razor
```razor
<MudThemeProvider />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />
```

### 4. Todo Module
The Todo feature module (`MudBlazorDemo.Features.Todo`) is self-contained with:
- Domain entities and EF Core configuration
- Application services and DTOs
- MudBlazor Razor pages (MudTable, MudDialog, MudForm)
- Permissions and localization
- Menu contribution

## Reference

- [MudBlazor Documentation](https://mudblazor.com)
- [ABP MudBlazor Community Article](https://abp.io/community/articles/mudblazor-theme-in-abp-blazor-webassembly-ae23zz17)
- [ABP Framework](https://abp.io)
