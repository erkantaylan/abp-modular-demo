# AngularDemo

## About this solution

This is a layered startup solution based on [Domain Driven Design (DDD)](https://abp.io/docs/latest/framework/architecture/domain-driven-design) practises with an **Angular** frontend. All the fundamental ABP modules are already installed. Check the [Application Startup Template](https://abp.io/docs/latest/solution-templates/layered-web-application) documentation for more info.

**Tech stack:** .NET 10, ABP Framework, Angular 19, PostgreSQL, OpenIddict, .NET Aspire (optional)

## Quick Start

> **Full setup instructions:** See [SETUP.md](SETUP.md) for comprehensive Windows and Linux guides, including prerequisites, multiple run methods, database configuration, and troubleshooting.

**Prerequisites:** .NET 10 SDK, Node.js v18/v20, PostgreSQL 14+, [ABP CLI](https://abp.io/docs/latest/cli)

```bash
git clone git@github.com:erkantaylan/abp-modular-demo.git
cd abp-modular-demo/demos/angular-modular-demo
make setup   # restore + install-libs + ng-install + migrate
make cert    # generate OpenIddict certificate
make run     # start the API (terminal 1)
make ng-start # start Angular dev server (terminal 2)
```

- API + Swagger: **https://localhost:44340/swagger**
- Angular app: **http://localhost:4200**
- Login: `admin` / `1q2w3E*`

### Without Make

```bash
dotnet tool install -g Volo.Abp.Studio.Cli   # Install ABP CLI
dotnet restore AngularDemo.slnx               # Restore NuGet packages
abp install-libs                               # Install client-side libraries
cd angular && npm install && cd ..             # Install Angular dependencies
dotnet run --project src/AngularDemo.DbMigrator # Run migrations

cd src/AngularDemo.HttpApi.Host
dotnet dev-certs https -v -ep openiddict.pfx -p 9643d757-38d9-4daf-a731-849512598df6
cd ../..

dotnet run --project src/AngularDemo.HttpApi.Host  # Start API (terminal 1)
cd angular && npm start                             # Start Angular (terminal 2)
```

### With .NET Aspire

Aspire orchestrates PostgreSQL (via Docker), migrations, and the API automatically:

```bash
dotnet run --project src/AngularDemo.AppHost
```

Then start Angular separately: `cd angular && npm start`

Requires Docker. See [SETUP.md](SETUP.md#run-with-net-aspire-windows) for details.

## Solution Structure

This is a layered monolith application with a separate Angular frontend:

| Project | Description |
|---------|-------------|
| `angular/` | Angular 19 single-page application (frontend) |
| `AngularDemo.HttpApi.Host` | ASP.NET Core API host (backend, port 44340) |
| `AngularDemo.DbMigrator` | Console app for migrations and data seeding |
| `AngularDemo.AppHost` | .NET Aspire orchestrator (optional) |
| `AngularDemo.Domain` | Domain entities and business rules |
| `AngularDemo.Application` | Application services (use cases) |
| `AngularDemo.EntityFrameworkCore` | EF Core DbContext and migrations |
| `AngularDemo.HttpApi` | REST API controllers |

See [docs/architecture.md](docs/architecture.md) for the full architecture overview.

## Documentation

| Document | Description |
|----------|-------------|
| [SETUP.md](SETUP.md) | Complete setup & run guide (Windows + Linux) |
| [docs/architecture.md](docs/architecture.md) | Solution architecture and project roles |
| [docs/angular-development.md](docs/angular-development.md) | Angular development guide (proxy, modules, components) |
| [docs/aspire-orchestration.md](docs/aspire-orchestration.md) | .NET Aspire setup and internals |

## Deploying the application

Deploying an ABP application follows the same process as deploying any .NET or ASP.NET Core application. However, there are important considerations to keep in mind. For detailed guidance, refer to ABP's [deployment documentation](https://abp.io/docs/latest/Deployment/Index).

## Additional resources

* [Web Application Development Tutorial](https://abp.io/docs/latest/tutorials/book-store/part-1)
* [Application Startup Template](https://abp.io/docs/latest/startup-templates/application/index)
* [Angular UI Documentation](https://abp.io/docs/latest/framework/ui/angular)
