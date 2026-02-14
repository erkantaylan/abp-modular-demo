# Todo Application

A role-based Todo application built with ABP Framework, Angular, and SignalR for real-time updates.

## Architecture

```
┌──────────────────┐     SignalR WebSocket      ┌─────────────────────┐
│  Angular SPA     │◄──────────────────────────►│  HttpApi.Host       │
│  (port 4200)     │     REST API (HTTP)        │  (port 44340)       │
│                  │◄──────────────────────────►│                     │
└──────────────────┘                            │  - TodoAppService   │
                                                │  - TodoHub (SignalR)│
                                                │  - OpenIddict Auth  │
                                                └─────────┬───────────┘
                                                          │
                                                ┌─────────▼───────────┐
                                                │  PostgreSQL         │
                                                │  (port 5432)        │
                                                └─────────────────────┘
```

### Layers

| Layer | Project | Purpose |
|-------|---------|---------|
| Domain | `AngularDemo.Domain` | `Todo` entity, seed data contributor |
| Domain.Shared | `AngularDemo.Domain.Shared` | Localization strings |
| Application.Contracts | `AngularDemo.Application.Contracts` | `ITodoAppService`, DTOs, permissions |
| Application | `AngularDemo.Application` | `TodoAppService`, `TodoHub` (SignalR) |
| EF Core | `AngularDemo.EntityFrameworkCore` | `DbSet<Todo>`, entity configuration |
| HttpApi.Host | `AngularDemo.HttpApi.Host` | SignalR hub endpoint mapping |
| Angular | `angular/src/app/todos/` | Todo list component, SignalR service |

## Seed Users

| Username | Password | Role | Permissions |
|----------|----------|------|-------------|
| admin | 1q2w3E* | admin | All (Create + Complete) |
| alice | 1q2w3E* | TodoCreator | View + Create todos |
| bob | 1q2w3E* | TodoCompleter | View + Complete todos |
| charlie | 1q2w3E* | TodoCreator + TodoCompleter | View + Create + Complete todos |

## Permissions

| Permission | Description |
|------------|-------------|
| `AngularDemo.Todos` | View todos (parent permission) |
| `AngularDemo.Todos.Create` | Create new todos |
| `AngularDemo.Todos.Complete` | Mark todos as complete |

## Running with Docker Compose

From the `demos/angular-modular-demo/` directory:

```bash
# Build and start all services
docker-compose up --build

# Services:
# - Angular: http://localhost:4200
# - API: http://localhost:44340
# - Swagger: http://localhost:44340/swagger
# - PostgreSQL: localhost:5432
```

The DbMigrator runs automatically as an init service, creating the database schema and seeding data before the API starts.

## Running Locally with Aspire

```bash
# From the repo root
cd demos/angular-modular-demo

# Start the .NET Aspire AppHost (starts PostgreSQL, DbMigrator, and API)
dotnet run --project src/AngularDemo.AppHost

# In a separate terminal, start the Angular app
cd angular
yarn install
yarn start
```

- Angular app: http://localhost:4200
- API: https://localhost:44340
- Swagger: https://localhost:44340/swagger

## API Endpoints

| Method | Endpoint | Permission | Description |
|--------|----------|------------|-------------|
| GET | `/api/app/todo` | Authenticated | List all todos |
| POST | `/api/app/todo` | `Todos.Create` | Create a new todo |
| POST | `/api/app/todo/{id}/complete` | `Todos.Complete` | Mark a todo as complete |

### SignalR Hub

- **Endpoint:** `/signalr-hubs/todo`
- **Events:**
  - `TodoCreated` - Broadcast when a new todo is created
  - `TodoCompleted` - Broadcast when a todo is marked complete

## Real-time Updates

The Angular app connects to the SignalR hub on initialization. When any user creates or completes a todo, all connected clients receive the update in real-time without needing to refresh the page. This is demonstrated by:

1. Login as **alice** in one browser (can create todos)
2. Login as **bob** in another browser (can complete todos)
3. When alice creates a todo, it appears in bob's list immediately
4. When bob completes a todo, alice sees the status change immediately
