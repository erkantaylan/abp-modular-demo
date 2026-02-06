# Architecture Overview

This solution follows ABP Framework's **layered architecture** based on Domain-Driven Design (DDD). Each `.csproj` is a separate layer with a specific responsibility. Dependencies only flow downward — the UI layer knows about everything, but the Domain layer knows nothing about databases or HTTP.

```
┌─────────────────────────────────────────────────────┐
│                    AppHost (Aspire)                  │  Orchestration
│          Spins up PostgreSQL + DbMigrator + Blazor   │
└──────────────┬──────────────────────┬───────────────┘
               │                      │
               ▼                      ▼
┌──────────────────────┐  ┌──────────────────────────┐
│     DbMigrator       │  │        Blazor (UI)       │  Hosts & Entry Points
│  (console app)       │  │   (web app, port 44377)  │
└──────────┬───────────┘  └──────┬───────────────────┘
           │                     │
           │              ┌──────┴──────────────┐
           │              │                     │
           │              ▼                     ▼
           │  ┌─────────────────┐  ┌──────────────────────┐
           │  │    HttpApi      │  │    Application        │  Application Layer
           │  │ (REST controllers)│  │ (business orchestration)│
           │  └────────┬────────┘  └──────────┬───────────┘
           │           │                      │
           │           ▼                      │
           │  ┌───────────────────────┐       │
           │  │ Application.Contracts │◄──────┘  Contracts Layer
           │  │   (DTOs, interfaces)  │
           │  └───────────┬───────────┘
           │              │
           ▼              ▼
┌────────────────────┐  ┌───────────────────┐
│ EntityFrameworkCore │  │     Domain        │  Domain Layer
│  (DB access, ORM)  │──▶ (entities, rules) │
└────────────────────┘  └────────┬──────────┘
                                 │
                        ┌────────▼──────────┐
                        │  Domain.Shared    │  Shared Kernel
                        │ (enums, constants)│
                        └───────────────────┘
```

## What Each Project Does

### `LayeredDemo.Domain.Shared`

**The shared vocabulary of the entire solution.**

Contains things that every layer might need: enums, constants, error codes, and localization resources. Has zero business logic — just definitions. Every other project references this directly or indirectly.

Example: if you define a `BookType` enum, it goes here so both the Domain and the DTOs can use it.

### `LayeredDemo.Domain`

**Where the actual business rules live.**

Contains your entities (the "nouns" of your app), domain services, repository interfaces, and domain events. This is the heart of DDD — if you have a rule like "an order can't exceed $10,000", it gets enforced here.

ABP pre-installs modules for Identity (users/roles), OpenIddict (OAuth tokens), Tenant Management (multi-tenancy), Audit Logging, etc. These are all domain-level concerns.

### `LayeredDemo.Application.Contracts`

**The API contract between your backend and any consumer.**

Defines DTOs (Data Transfer Objects) and application service interfaces (`IBookAppService`, etc.). This is what a client needs to know to call your services, without knowing how they're implemented.

Separated so that `HttpApi.Client` can reference just the contracts without pulling in the full application logic.

### `LayeredDemo.Application`

**Orchestrates use cases by calling domain logic.**

Application services live here. They receive DTOs, call domain services/repositories, and return DTOs. They don't contain business rules themselves — they coordinate.

Example: `CreateBookAsync(CreateBookDto input)` would validate input, call a domain service, save via a repository, and return a `BookDto`.

### `LayeredDemo.EntityFrameworkCore`

**The database layer — translates domain objects to SQL.**

Contains the `DbContext`, entity-to-table mappings, and EF Core migrations. Uses PostgreSQL via Npgsql. This is the only project that knows about the actual database.

ABP auto-registers repositories for your entities, so you rarely write raw SQL or even custom repository implementations.

### `LayeredDemo.HttpApi`

**Exposes application services as REST endpoints.**

ABP can auto-generate REST controllers from your `IAppService` interfaces, but this project lets you customize them or add non-standard endpoints. Think of it as the HTTP translation layer.

### `LayeredDemo.HttpApi.Client`

**A C# client library for consuming the REST API.**

If another .NET service needs to call this app's API, it references this project and gets strongly-typed HTTP client proxies auto-generated from the contracts. You'd use this in microservice scenarios or integration tests.

### `LayeredDemo.Blazor`

**The main web application — ties everything together.**

This is the Blazor Server UI. It references Application, HttpApi, and EntityFrameworkCore to compose the full running app. Handles:
- Razor component rendering (interactive server-side)
- Authentication via OpenIddict (OAuth 2.0)
- LeptonX Lite theme (the visual skin)
- Swagger/OpenAPI docs
- Health checks
- Static file serving (wwwroot/libs for JS/CSS)

### `LayeredDemo.DbMigrator`

**A standalone console app that sets up the database.**

Runs EF Core migrations to create/update tables, then seeds initial data (admin user, default tenant, permissions, OpenIddict configuration). You run this once before starting the app, or after adding new migrations.

When using Aspire, this runs automatically before the Blazor app starts.

### `LayeredDemo.AppHost`

**The Aspire orchestrator — manages the full stack.**

A special project that uses .NET Aspire to:
1. Spin up a PostgreSQL container (via Docker)
2. Run DbMigrator and wait for it to finish
3. Start the Blazor app, injecting the database connection string

Also gives you the Aspire Dashboard for monitoring traces, logs, and metrics across all services.

### `LayeredDemo.ServiceDefaults`

**Shared infrastructure configuration for Aspire.**

Configures OpenTelemetry (distributed tracing), health checks, service discovery, and HTTP resilience policies. Both the Blazor app and DbMigrator reference this to get consistent observability.

## Why So Many Projects?

It looks like overkill for a small app, but the separation pays off at scale:

- **Domain isolation**: Business rules never depend on HTTP or database specifics
- **Testability**: You can test Application services without a web server or real database
- **Client generation**: HttpApi.Client gives you a free SDK for other services
- **Module reuse**: Each ABP module (Identity, Tenants, etc.) follows the same layering, so they all compose cleanly
- **Deployment flexibility**: You could split the Blazor UI from the API if needed later
