# Architecture Overview

This solution follows ABP Framework's **layered architecture** based on Domain-Driven Design (DDD) with an Angular frontend. The backend projects form the API server, while Angular runs as a separate single-page application that communicates via REST APIs.

```
┌─────────────────────────────────────────────────────┐
│                    AppHost (Aspire)                  │  Orchestration
│          Spins up PostgreSQL + DbMigrator + API     │
└──────────────┬──────────────────────┬───────────────┘
               │                      │
               ▼                      ▼
┌──────────────────────┐  ┌──────────────────────────┐
│     DbMigrator       │  │    HttpApi.Host (API)    │  Hosts & Entry Points
│  (console app)       │  │  (web server, port 44340)│
└──────────┬───────────┘  └──────┬───────────────────┘
           │                     │            ▲
           │              ┌──────┴──────┐     │ REST/JSON
           │              │             │     │
           │              ▼             │  ┌──┴───────────────┐
           │  ┌─────────────────┐      │  │  Angular (SPA)   │  Frontend
           │  │    HttpApi      │      │  │  (port 4200)     │
           │  │ (REST controllers)│     │  └──────────────────┘
           │  └────────┬────────┘      │
           │           │               ▼
           │           │   ┌──────────────────────┐
           │           │   │    Application        │  Application Layer
           │           │   │ (business orchestration)│
           │           │   └──────────┬───────────┘
           │           │              │
           │           ▼              │
           │  ┌───────────────────────┐
           │  │ Application.Contracts │◄──┘  Contracts Layer
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

## Key Difference from Blazor Template

Unlike the Blazor layered demo (which has a single combined web host), this Angular template has a **separated architecture**:

- **HttpApi.Host** — Serves the REST API and handles authentication (OpenIddict). This is the backend server.
- **Angular app** — A completely separate SPA that runs on its own dev server (port 4200) and calls the API via HTTP.

This separation means you run **two processes** during development: the .NET API server and the Angular dev server. The Angular dev server proxies API requests to the backend to avoid CORS issues during development.

## What Each Project Does

### `angular/`

**The Angular single-page application (frontend).**

A standalone Angular 19 project with its own `package.json`, build tooling, and development server. It uses ABP's Angular UI packages (`@abp/ng.core`, `@abp/ng.theme.lepton-x`, etc.) for:
- Authentication via OAuth 2.0 / OpenID Connect (talks to the API's OpenIddict server)
- Dynamic menu system and permission-based UI
- Localization (multi-language support)
- LeptonX Lite theme

The Angular app communicates with the backend exclusively through REST API calls. It has no direct dependency on any .NET project.

### `AngularDemo.HttpApi.Host`

**The ASP.NET Core API host — serves REST APIs and handles auth.**

This is the main backend entry point. Unlike the Blazor template (which combines UI and API), this project:
- Exposes all REST endpoints from `HttpApi`
- Serves as the OpenIddict authorization server (issues tokens)
- Handles CORS configuration for the Angular app
- Provides Swagger/OpenAPI documentation at `/swagger`

### `AngularDemo.Domain.Shared`

**The shared vocabulary of the entire solution.**

Contains things that every layer might need: enums, constants, error codes, and localization resources. Has zero business logic — just definitions. Every other project references this directly or indirectly.

Example: if you define a `BookType` enum, it goes here so both the Domain and the DTOs can use it.

### `AngularDemo.Domain`

**Where the actual business rules live.**

Contains your entities (the "nouns" of your app), domain services, repository interfaces, and domain events. This is the heart of DDD — if you have a rule like "an order can't exceed $10,000", it gets enforced here.

ABP pre-installs modules for Identity (users/roles), OpenIddict (OAuth tokens), Tenant Management (multi-tenancy), Audit Logging, etc. These are all domain-level concerns.

### `AngularDemo.Application.Contracts`

**The API contract between your backend and any consumer.**

Defines DTOs (Data Transfer Objects) and application service interfaces (`IBookAppService`, etc.). This is what a client needs to know to call your services, without knowing how they're implemented.

Separated so that `HttpApi.Client` can reference just the contracts without pulling in the full application logic.

### `AngularDemo.Application`

**Orchestrates use cases by calling domain logic.**

Application services live here. They receive DTOs, call domain services/repositories, and return DTOs. They don't contain business rules themselves — they coordinate.

Example: `CreateBookAsync(CreateBookDto input)` would validate input, call a domain service, save via a repository, and return a `BookDto`.

### `AngularDemo.EntityFrameworkCore`

**The database layer — translates domain objects to SQL.**

Contains the `DbContext`, entity-to-table mappings, and EF Core migrations. Uses PostgreSQL via Npgsql. This is the only project that knows about the actual database.

ABP auto-registers repositories for your entities, so you rarely write raw SQL or even custom repository implementations.

### `AngularDemo.HttpApi`

**Exposes application services as REST endpoints.**

ABP can auto-generate REST controllers from your `IAppService` interfaces, but this project lets you customize them or add non-standard endpoints. Think of it as the HTTP translation layer.

### `AngularDemo.HttpApi.Client`

**A C# client library for consuming the REST API.**

If another .NET service needs to call this app's API, it references this project and gets strongly-typed HTTP client proxies auto-generated from the contracts. You'd use this in microservice scenarios or integration tests.

### `AngularDemo.DbMigrator`

**A standalone console app that sets up the database.**

Runs EF Core migrations to create/update tables, then seeds initial data (admin user, default tenant, permissions, OpenIddict configuration). You run this once before starting the app, or after adding new migrations.

When using Aspire, this runs automatically before the API Host starts.

### `AngularDemo.AppHost`

**The Aspire orchestrator — manages the full backend stack.**

A special project that uses .NET Aspire to:
1. Spin up a PostgreSQL container (via Docker)
2. Run DbMigrator and wait for it to finish
3. Start the HttpApi.Host, injecting the database connection string

Also gives you the Aspire Dashboard for monitoring traces, logs, and metrics. Note that Angular still runs separately — Aspire manages only the .NET backend.

### `AngularDemo.ServiceDefaults`

**Shared infrastructure configuration for Aspire.**

Configures OpenTelemetry (distributed tracing), health checks, service discovery, and HTTP resilience policies. Both the HttpApi.Host and DbMigrator reference this to get consistent observability.

## Why So Many Projects?

It looks like overkill for a small app, but the separation pays off at scale:

- **Domain isolation**: Business rules never depend on HTTP or database specifics
- **Testability**: You can test Application services without a web server or real database
- **Client generation**: HttpApi.Client gives you a free SDK for other services
- **Module reuse**: Each ABP module (Identity, Tenants, etc.) follows the same layering, so they all compose cleanly
- **Frontend flexibility**: The Angular app is fully decoupled — you could swap it for React, Vue, or another framework without touching the backend
- **Deployment flexibility**: You can deploy the API and Angular app independently (different servers, CDN for static assets, etc.)
