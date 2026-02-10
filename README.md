# ABP Modular Demo

Demonstration repository showing how [ABP Framework](https://abp.io/) modular monolith architecture works with different Blazor UI component libraries. Three independent demo applications prove that ABP's layered DDD structure is not locked to any single UI toolkit.

## Requirements

| Tool | Version | Notes |
|------|---------|-------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0+ | Runtime and build toolchain |
| [Node.js](https://nodejs.org/) | 18+ | Required for ABP client-side library installation |
| [PostgreSQL](https://www.postgresql.org/) | 14+ | Or use Docker (see below) |
| [Docker](https://www.docker.com/) | Latest | Required for .NET Aspire; also simplifies PostgreSQL setup |
| [ABP CLI](https://abp.io/docs/latest/cli) | Latest | `dotnet tool install -g Volo.Abp.Studio.Cli` |
| [yarn](https://yarnpkg.com/) | 1.x | Only needed for the FluentUI demo's CSS build |

## Demos

Each demo is a complete, standalone ABP application with its own solution, database, and UI:

| Demo | UI Library | Port | Description |
|------|-----------|------|-------------|
| [fluentui-demo](demos/fluentui-demo/) | [FluentUI Blazor](https://www.fluentui-blazor.net/) v4.13 | 44379 | FluentUI components, SignalR chat, shopping cart, Bulma CSS |
| [mudblazor-demo](demos/mudblazor-demo/) | [MudBlazor](https://mudblazor.com/) v8.5 | 44378 | Material Design components, MudTable/MudDialog/MudForm |
| [layered-demo](demos/layered-demo/) | [LeptonX Lite](https://leptontheme.com/) v5.0 + [Blazorise](https://blazorise.com/) v1.8 | 44377 | ABP's default commercial theme (reference implementation) |

All three share the same layered architecture and a Todo feature module for comparison.

## Quick Start

Pick any demo. The fastest path uses .NET Aspire, which handles PostgreSQL, migrations, and the app in one command.

### Option A: Run with .NET Aspire (recommended)

Aspire starts a PostgreSQL container, runs database migrations, and launches the app automatically. Requires Docker.

```bash
git clone git@github.com:erkantaylan/abp-modular-demo.git
cd abp-modular-demo

# Example: FluentUI demo
cd demos/fluentui-demo
yarn install && yarn build:css   # FluentUI demo only — installs Bulma CSS
dotnet run --project src/FluentUiDemo.AppHost/FluentUiDemo.AppHost.csproj
```

Open the Aspire dashboard to see all resources and endpoints.

For the other demos:
```bash
# MudBlazor demo
cd demos/mudblazor-demo
dotnet run --project src/MudBlazorDemo.AppHost/MudBlazorDemo.AppHost.csproj

# Layered demo
cd demos/layered-demo
dotnet run --project src/LayeredDemo.AppHost/LayeredDemo.AppHost.csproj
```

### Option B: Run without Aspire

Start PostgreSQL manually:

```bash
docker run -d --name demo-pg \
  -e POSTGRES_PASSWORD=postgres \
  -p 5432:5432 \
  postgres:16
```

Then for any demo (using FluentUI as an example):

```bash
cd demos/fluentui-demo

# Install ABP client-side libraries
dotnet tool install -g Volo.Abp.Studio.Cli
abp install-libs

# FluentUI demo only — build CSS
yarn install && yarn build:css

# Run database migrations and seed data
dotnet run --project src/FluentUiDemo.DbMigrator/FluentUiDemo.DbMigrator.csproj

# Generate OpenIddict development certificate
cd src/FluentUiDemo.Blazor
dotnet dev-certs https -v -ep openiddict.pfx -p a1b2c3d4-e5f6-7890-abcd-ef1234567890
cd ../..

# Start the application
dotnet run --project src/FluentUiDemo.Blazor/FluentUiDemo.Blazor.csproj
```

### Default Credentials

All demos use the same default login:

- **Username:** `admin`
- **Password:** `1q2w3E*`

### Database Connection

Each demo uses a separate PostgreSQL database. Default connection (from `appsettings.json`):

```
Host=localhost;Port=5432;Database=<DemoName>;Username=postgres;Password=postgres
```

When running via Aspire, the connection string is injected automatically.

## Project Structure

```
abp-modular-demo/
├── demos/
│   ├── fluentui-demo/          # FluentUI Blazor + Bulma + SignalR chat + shopping
│   │   ├── src/
│   │   │   ├── FluentUiDemo.AppHost/          # .NET Aspire orchestrator
│   │   │   ├── FluentUiDemo.Blazor/           # Blazor Server web app
│   │   │   ├── FluentUiDemo.Domain/           # Domain entities
│   │   │   ├── FluentUiDemo.Application/      # Application services
│   │   │   ├── FluentUiDemo.EntityFrameworkCore/  # EF Core + migrations
│   │   │   ├── FluentUiDemo.DbMigrator/       # Migration console app
│   │   │   └── features/                      # Self-contained modules
│   │   │       ├── FluentUiDemo.Features.Todo/
│   │   │       ├── FluentUiDemo.Features.Chat/
│   │   │       └── FluentUiDemo.Features.Shopping/
│   │   └── README.md
│   ├── mudblazor-demo/         # MudBlazor (Material Design)
│   │   ├── src/                # Same layered structure
│   │   │   └── features/
│   │   │       └── MudBlazorDemo.Features.Todo/
│   │   ├── test/               # Unit and integration tests
│   │   └── README.md
│   └── layered-demo/           # LeptonX Lite + Blazorise (ABP default)
│       ├── src/                # Same layered structure
│       │   └── features/
│       │       └── LayeredDemo.Features.Todo/
│       ├── test/               # Unit and integration tests
│       └── README.md
```

Each demo follows ABP's [Domain-Driven Design](https://docs.abp.io/en/abp/latest/Domain-Driven-Design) layering conventions. Feature modules are self-contained with their own domain entities, application services, EF Core configuration, UI components, localization, and permissions.

## Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Framework | [ABP Framework](https://abp.io/) | 10.0.2 |
| Runtime | [.NET](https://dotnet.microsoft.com/) | 10.0 |
| UI | [Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/) (Interactive Server) | 10.0 |
| Database | [PostgreSQL](https://www.postgresql.org/) | 14+ |
| ORM | [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) | 10.0 |
| Authentication | [OpenIddict](https://documentation.openiddict.com/) (via ABP) | Built-in |
| External Login | Google OAuth, GitHub OAuth | Optional |
| Real-time | [ASP.NET Core SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/) | Built-in |
| Orchestration | [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/) | 13.1.0 |
| Observability | [OpenTelemetry](https://opentelemetry.io/), [Serilog](https://serilog.net/) | 1.14.0 / 9.0 |
| API Docs | [Swagger / Swashbuckle](https://swagger.io/) | Built-in ABP |
| Object Mapping | [Mapperly](https://mapperly.riok.app/) | Source-generated |
| DI Container | [Autofac](https://autofac.org/) | Built-in ABP |

### UI Component Libraries

| Library | Version | Used In |
|---------|---------|---------|
| [FluentUI Blazor](https://www.fluentui-blazor.net/) | 4.13.2 | fluentui-demo |
| [Bulma](https://bulma.io/) | 1.0.4 | fluentui-demo (CSS) |
| [MudBlazor](https://mudblazor.com/) | 8.5.0 | mudblazor-demo |
| [LeptonX Lite](https://leptontheme.com/) | 5.0.2 | layered-demo |
| [Blazorise](https://blazorise.com/) | 1.8.8 | layered-demo |

## Features Demonstrated

- **Modular monolith architecture** — Self-contained feature modules (Todo, Chat, Shopping) plugged into the ABP module system
- **Multiple UI toolkits** — Same backend with FluentUI, MudBlazor, or Blazorise/LeptonX frontends
- **Real-time communication** — SignalR-based live chat with presence tracking (FluentUI demo)
- **Shopping cart** — Product catalog with category filtering and cart management (FluentUI demo)
- **OAuth external login** — Google and GitHub login providers (FluentUI demo)
- **Localization** — English and Turkish language support
- **Cloud-native orchestration** — .NET Aspire with PostgreSQL, OpenTelemetry, and health checks
- **Permission management** — ABP authorization system with role-based access control
- **Domain-Driven Design** — Layered architecture following ABP's DDD conventions

## Additional Resources

- [ABP Framework Documentation](https://abp.io/docs/latest)
- [ABP Web Application Development Tutorial](https://abp.io/docs/latest/tutorials/book-store/part-1)
- [ABP Application Startup Template](https://abp.io/docs/latest/solution-templates/layered-web-application)
- [ABP Deployment Guide](https://abp.io/docs/latest/Deployment/Index)
- [FluentUI Blazor Documentation](https://www.fluentui-blazor.net/)
- [MudBlazor Documentation](https://mudblazor.com/)
- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)

## License

See individual demo directories for license information.
