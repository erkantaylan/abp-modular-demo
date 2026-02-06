# Windows Setup Guide

Step-by-step instructions to get LayeredDemo running on Windows.

## Prerequisites

Install these before proceeding:

| Tool | Version | Download |
|------|---------|----------|
| .NET SDK | 10.0+ | https://dotnet.microsoft.com/download/dotnet/10.0 |
| Node.js | v18 or v20 (LTS) | https://nodejs.org/ |
| PostgreSQL | 14+ | https://www.postgresql.org/download/windows/ |
| Git | Latest | https://git-scm.com/download/win |

### Verify installations

Open PowerShell and run:

```powershell
dotnet --version    # Should show 10.0.x
node --version      # Should show v18.x or v20.x
npm --version       # Comes with Node.js
psql --version      # Should show 14+
git --version
```

## 1. Install the ABP CLI

```powershell
dotnet tool install -g Volo.Abp.Studio.Cli
```

If already installed, update it:

```powershell
dotnet tool update -g Volo.Abp.Studio.Cli
```

Verify:

```powershell
abp --version
```

## 2. Clone the Repository

```powershell
git clone git@github.com:erkantaylan/abp-modular-demo.git
cd abp-modular-demo/demos/layered-demo
```

## 3. Install Node Packages (Client-Side Libraries)

ABP uses its own CLI to install client-side packages (Bootstrap, jQuery, FontAwesome, etc.) into `wwwroot/libs/`. This wraps npm under the hood.

```powershell
abp install-libs
```

This reads `package.json` in `src/LayeredDemo.Blazor/`, runs npm install, and copies the required files to `wwwroot/libs/`.

### If `abp install-libs` fails

Try installing npm packages manually first, then re-run:

```powershell
cd src\LayeredDemo.Blazor
npm install
cd ..\..
abp install-libs
```

### Verify

Check that `src\LayeredDemo.Blazor\wwwroot\libs\` contains folders like `bootstrap`, `jquery`, `@fortawesome`, etc.

## 4. Set Up PostgreSQL

Make sure PostgreSQL is running. The default connection string expects:

| Setting | Value |
|---------|-------|
| Host | localhost |
| Port | 5432 |
| Database | LayeredDemo |
| Username | postgres |
| Password | postgres |

You can change these in `src\LayeredDemo.Blazor\appsettings.json` and `src\LayeredDemo.DbMigrator\appsettings.json`.

## 5. Run Database Migrations

This creates the database and seeds initial data:

```powershell
dotnet run --project src\LayeredDemo.DbMigrator
```

## 6. Generate the OpenIddict Signing Certificate

```powershell
cd src\LayeredDemo.Blazor
dotnet dev-certs https -v -ep openiddict.pfx -p 77a0c366-a637-445b-bcf0-6406b68816ac
cd ..\..
```

Also trust the HTTPS development certificate if you haven't already:

```powershell
dotnet dev-certs https --trust
```

## 7. Run the Application

```powershell
dotnet run --project src\LayeredDemo.Blazor
```

The app will be available at: **https://localhost:44377**

Default admin credentials:
- Username: `admin`
- Password: `1q2w3E*`

## Using the Makefile

If you have `make` installed (e.g., via [Chocolatey](https://chocolatey.org/) `choco install make`), you can use the Makefile shortcuts:

```powershell
make setup    # Full setup: restore + install-libs + migrate
make build    # Build the solution
make run      # Run the Blazor app
make test     # Run all tests
make clean    # Clean build artifacts
make help     # Show all available commands
```

## Using the PowerShell Scripts

The repo includes PowerShell scripts that run setup steps in parallel:

```powershell
# Full initialization (install-libs + migrate + generate cert)
.\etc\scripts\initialize-solution.ps1

# Database migration only
.\etc\scripts\migrate-database.ps1
```

## Using .NET Aspire (Optional)

The solution includes an Aspire AppHost that manages PostgreSQL via Docker automatically:

```powershell
dotnet run --project src\LayeredDemo.AppHost
```

This requires Docker Desktop to be running. Aspire will provision PostgreSQL and start the Blazor app with the Aspire dashboard for monitoring.

## Troubleshooting

### `abp install-libs` hangs or fails
- Make sure Node.js is in your PATH
- Try `npm cache clean --force` then re-run
- On corporate networks, check npm proxy settings: `npm config set proxy http://your-proxy:port`

### Port 44377 already in use
- Find and kill the process: `netstat -ano | findstr :44377` then `taskkill /PID <pid> /F`
- Or change the port in `src\LayeredDemo.Blazor\Properties\launchSettings.json`

### Database connection refused
- Verify PostgreSQL is running: `Get-Service postgresql*` or check Services app
- Verify credentials: `psql -U postgres -h localhost`

### SSL certificate errors
- Run `dotnet dev-certs https --trust` and restart your browser
- If using Firefox, add an exception for `localhost:44377`

### `dotnet restore` fails with package errors
- Check `NuGet.Config` is present in the solution root
- Try `dotnet nuget locals all --clear` then re-run
