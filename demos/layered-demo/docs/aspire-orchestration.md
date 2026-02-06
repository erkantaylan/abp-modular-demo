# How .NET Aspire Orchestration Works

## What Is Aspire?

.NET Aspire is a development orchestrator. Instead of manually starting PostgreSQL, running migrations, then launching the app — Aspire does it all with one command. It also gives you a dashboard for monitoring logs, traces, and metrics.

**Aspire is for local development and testing.** It's not a production deployment tool (though it can generate deployment manifests).

## The AppHost — What It Does

`LayeredDemo.AppHost/Program.cs` is only 19 lines but does a lot:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// 1. Spin up a PostgreSQL container via Docker
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()                                    // Also spin up pgAdmin web UI
    .WithDataVolume("layered-demo-postgres-data")     // Persist data between restarts
    .PublishAsContainer();                             // Package as container for deployment

// 2. Create a database named "Default" inside that PostgreSQL instance
var database = postgres.AddDatabase("Default");

// 3. Run the DbMigrator, but only after PostgreSQL is ready
var dbMigrator = builder.AddProject<Projects.LayeredDemo_DbMigrator>("dbmigrator")
    .WithReference(database)      // Inject the connection string
    .WaitFor(database);           // Don't start until PostgreSQL accepts connections

// 4. Run the Blazor app, but only after migrations are done
builder.AddProject<Projects.LayeredDemo_Blazor>("blazor")
    .WithExternalHttpEndpoints()  // Expose HTTP ports externally
    .WithReference(database)      // Inject the connection string
    .WaitForCompletion(dbMigrator); // Don't start until DbMigrator exits successfully

builder.Build().Run();
```

## Startup Sequence

```
Time ──────────────────────────────────────────────────────►

[Docker pulls postgres image]
         │
         ▼
[PostgreSQL container starts]
         │ health check passes
         ▼
[DbMigrator starts]──────[creates tables, seeds data]──────[exits]
                                                              │
                                                              ▼
                                                    [Blazor app starts]
                                                              │
                                                              ▼
                                                    [App ready on HTTPS]
```

## Connection String Injection

The magic: you never configure a connection string manually when using Aspire.

When `WithReference(database)` is called, Aspire automatically sets an environment variable like:

```
ConnectionStrings__Default=Host=localhost;Port=<random>;Database=Default;Username=postgres;Password=<generated>
```

The ABP projects read `ConnectionStrings:Default` from configuration, and .NET's configuration system picks up environment variables automatically. So the apps connect to the Aspire-managed PostgreSQL without any `appsettings.json` changes.

## What You Get

When you run `dotnet run --project src/LayeredDemo.AppHost`:

| Resource | What It Is | How to Access |
|----------|-----------|---------------|
| **Aspire Dashboard** | Logs, traces, metrics viewer | URL printed in console (https://localhost:xxxxx) |
| **PostgreSQL** | Database server in Docker | Auto-connected by Aspire |
| **pgAdmin** | Database management web UI | Link in Aspire Dashboard |
| **DbMigrator** | Runs once, then stops | View logs in Dashboard |
| **Blazor App** | The actual web app | Link in Aspire Dashboard |

## ServiceDefaults — Shared Observability

`LayeredDemo.ServiceDefaults` is referenced by both Blazor and DbMigrator. It configures:

- **OpenTelemetry**: Distributed tracing (see request flows across services), metrics (request counts, durations), and logs — all sent to the Aspire Dashboard
- **Health checks**: `/health` and `/alive` endpoints for container orchestrators
- **Service discovery**: Aspire's built-in DNS-based discovery (not used much in this single-app setup, but essential for microservices)
- **HTTP resilience**: Retry policies and circuit breakers for outgoing HTTP calls

## Aspire vs Manual Setup

| | Without Aspire | With Aspire |
|---|---|---|
| **PostgreSQL** | Install locally, create DB manually | Docker container, auto-provisioned |
| **Connection string** | Edit appsettings.json | Auto-injected via environment |
| **Migrations** | Run DbMigrator manually | Runs automatically before app starts |
| **Monitoring** | Set up Seq/Jaeger/etc. yourself | Aspire Dashboard included |
| **Command** | 3+ separate steps | `dotnet run --project AppHost` |

## Data Persistence

The `WithDataVolume("layered-demo-postgres-data")` call creates a Docker named volume. Your database data survives container restarts. To start completely fresh:

```powershell
docker volume rm layered-demo-postgres-data
```

## When NOT to Use Aspire

- **Production deployment**: Use proper infrastructure (managed PostgreSQL, Kubernetes, etc.)
- **CI/CD**: You might still use it for integration tests, but not for the pipeline itself
- **When Docker isn't available**: Aspire needs Docker for container resources like PostgreSQL
