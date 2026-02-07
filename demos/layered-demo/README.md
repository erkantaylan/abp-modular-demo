# LayeredDemo

## About this solution

This is a layered startup solution based on [Domain Driven Design (DDD)](https://abp.io/docs/latest/framework/architecture/domain-driven-design) practises. All the fundamental ABP modules are already installed. Check the [Application Startup Template](https://abp.io/docs/latest/solution-templates/layered-web-application) documentation for more info.

**Tech stack:** .NET 10, ABP Framework, Blazor Server, PostgreSQL, OpenIddict, .NET Aspire (optional)

## Quick Start

> **Full setup instructions:** See [SETUP.md](SETUP.md) for comprehensive Windows and Linux guides, including prerequisites, multiple run methods, database configuration, and troubleshooting.

**Prerequisites:** .NET 10 SDK, Node.js v18/v20, PostgreSQL 14+, [ABP CLI](https://abp.io/docs/latest/cli)

```bash
git clone git@github.com:erkantaylan/abp-modular-demo.git
cd abp-modular-demo/demos/layered-demo
make setup   # restore + install-libs + migrate
make cert    # generate OpenIddict certificate
make run     # start the app
```

Open **https://localhost:44377** — login: `admin` / `1q2w3E*`

### Without Make

```bash
dotnet tool install -g Volo.Abp.Studio.Cli   # Install ABP CLI
abp install-libs                               # Install client-side libraries
dotnet run --project src/LayeredDemo.DbMigrator # Run migrations

cd src/LayeredDemo.Blazor
dotnet dev-certs https -v -ep openiddict.pfx -p 77a0c366-a637-445b-bcf0-6406b68816ac
cd ../..

dotnet run --project src/LayeredDemo.Blazor    # Start the app
```

### With .NET Aspire

Aspire orchestrates PostgreSQL (via Docker), migrations, and the app automatically:

```bash
dotnet run --project src/LayeredDemo.AppHost
```

Requires Docker. See [SETUP.md](SETUP.md#run-with-net-aspire-windows) for details.

## Solution Structure

This is a layered monolith application:

| Project | Description |
|---------|-------------|
| `LayeredDemo.Blazor` | ASP.NET Core Blazor Server web application |
| `LayeredDemo.DbMigrator` | Console app for migrations and data seeding |
| `LayeredDemo.AppHost` | .NET Aspire orchestrator (optional) |
| `LayeredDemo.Domain` | Domain entities and business rules |
| `LayeredDemo.Application` | Application services (use cases) |
| `LayeredDemo.EntityFrameworkCore` | EF Core DbContext and migrations |
| `LayeredDemo.HttpApi` | REST API controllers |

See [docs/architecture.md](docs/architecture.md) for the full architecture overview.

## Documentation

| Document | Description |
|----------|-------------|
| [SETUP.md](SETUP.md) | Complete setup & run guide (Windows + Linux) |
| [docs/architecture.md](docs/architecture.md) | Solution architecture and project roles |
| [docs/aspire-orchestration.md](docs/aspire-orchestration.md) | .NET Aspire setup and internals |
| [docs/module-development.md](docs/module-development.md) | How to create feature modules |
| [docs/npm-in-dotnet.md](docs/npm-in-dotnet.md) | Why npm is used and how it works |

## Deploying the application

Deploying an ABP application follows the same process as deploying any .NET or ASP.NET Core application. However, there are important considerations to keep in mind. For detailed guidance, refer to ABP's [deployment documentation](https://abp.io/docs/latest/Deployment/Index).

## Additional resources

* [Web Application Development Tutorial](https://abp.io/docs/latest/tutorials/book-store/part-1)
* [Application Startup Template](https://abp.io/docs/latest/startup-templates/application/index)
