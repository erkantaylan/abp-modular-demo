# Setup & Run Guide

Complete instructions for setting up and running the LayeredDemo project on **Windows** and **Linux**.

LayeredDemo is an ABP Framework (.NET 10) application with a Blazor Server UI, PostgreSQL database, OpenIddict authentication, and optional .NET Aspire orchestration.

---

## Table of Contents

- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Windows Setup](#windows-setup)
  - [Install Prerequisites on Windows](#install-prerequisites-on-windows)
  - [Run Manually (Windows)](#run-manually-windows)
  - [Run with Makefile (Windows)](#run-with-makefile-windows)
  - [Run with PowerShell Scripts (Windows)](#run-with-powershell-scripts-windows)
  - [Run with .NET Aspire (Windows)](#run-with-net-aspire-windows)
- [Linux Setup](#linux-setup)
  - [Install Prerequisites on Linux (Ubuntu/Debian)](#install-prerequisites-on-linux-ubuntudebian)
  - [Run Manually (Linux)](#run-manually-linux)
  - [Run with Makefile (Linux)](#run-with-makefile-linux)
  - [Run with .NET Aspire (Linux)](#run-with-net-aspire-linux)
- [Database Configuration](#database-configuration)
- [HTTPS Certificate (OpenIddict)](#https-certificate-openiddict)
- [Running Tests](#running-tests)
- [Troubleshooting](#troubleshooting)

---

## Prerequisites

All platforms require the following:

| Tool | Version | Purpose |
|------|---------|---------|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | 10.0+ | Build and run the application |
| [Node.js](https://nodejs.org/) | v18 or v20 (LTS) | Client-side library installation |
| [PostgreSQL](https://www.postgresql.org/download/) | 14+ | Database (unless using Aspire, which runs it in Docker) |
| [Git](https://git-scm.com/) | Latest | Clone the repository |
| [ABP CLI](https://abp.io/docs/latest/cli) | Latest | Install client-side libraries |
| [Docker](https://www.docker.com/get-started/) | Latest | **Only if using .NET Aspire** |

### Install the ABP CLI

```bash
dotnet tool install -g Volo.Abp.Studio.Cli
```

To update an existing installation:

```bash
dotnet tool update -g Volo.Abp.Studio.Cli
```

---

## Quick Start

For those who want to get running fast (assumes prerequisites are installed and PostgreSQL is running):

```bash
git clone git@github.com:erkantaylan/abp-modular-demo.git
cd abp-modular-demo/demos/layered-demo
make setup   # restore + install-libs + migrate
make cert    # generate OpenIddict certificate
make run     # start the app
```

Open **https://localhost:44377** and log in with `admin` / `1q2w3E*`.

---

## Windows Setup

### Install Prerequisites on Windows

**1. .NET 10 SDK**

Download from https://dotnet.microsoft.com/download/dotnet/10.0 and run the installer.

**2. Node.js**

Download the LTS version (v18 or v20) from https://nodejs.org/ and run the installer. npm is included.

**3. PostgreSQL**

Download from https://www.postgresql.org/download/windows/ and run the installer. Remember the password you set for the `postgres` user during installation.

**4. Git**

Download from https://git-scm.com/download/win and run the installer.

**5. Docker Desktop (only for Aspire)**

Download from https://www.docker.com/products/docker-desktop/ and install. Required only if you want to use .NET Aspire orchestration.

**Verify installations** in PowerShell:

```powershell
dotnet --version    # Should show 10.0.x
node --version      # Should show v18.x or v20.x
npm --version       # Comes with Node.js
psql --version      # Should show 14+
git --version
abp --version       # After installing ABP CLI
```

### Run Manually (Windows)

**1. Clone and navigate to the project:**

```powershell
git clone git@github.com:erkantaylan/abp-modular-demo.git
cd abp-modular-demo\demos\layered-demo
```

**2. Install client-side libraries:**

```powershell
abp install-libs
```

This reads `package.json` in `src/LayeredDemo.Blazor/`, runs npm, and copies the required files to `wwwroot/libs/`.

If this fails, try installing npm packages manually first:

```powershell
cd src\LayeredDemo.Blazor
npm install
cd ..\..
abp install-libs
```

**3. Configure the database** (if not using defaults):

Edit the connection string in both:
- `src\LayeredDemo.Blazor\appsettings.json`
- `src\LayeredDemo.DbMigrator\appsettings.json`

See [Database Configuration](#database-configuration) for details.

**4. Run database migrations:**

```powershell
dotnet run --project src\LayeredDemo.DbMigrator
```

This creates the database, applies all migrations, and seeds initial data (including the admin user).

**5. Generate the OpenIddict certificate:**

```powershell
cd src\LayeredDemo.Blazor
dotnet dev-certs https -v -ep openiddict.pfx -p 77a0c366-a637-445b-bcf0-6406b68816ac
cd ..\..
```

Trust the HTTPS development certificate:

```powershell
dotnet dev-certs https --trust
```

**6. Run the application:**

```powershell
dotnet run --project src\LayeredDemo.Blazor
```

Open **https://localhost:44377** and log in with:
- Username: `admin`
- Password: `1q2w3E*`

### Run with Makefile (Windows)

Requires `make` (install via [Chocolatey](https://chocolatey.org/): `choco install make`).

```powershell
make setup    # Full setup: restore + install-libs + migrate
make cert     # Generate OpenIddict signing certificate
make build    # Build the solution
make run      # Run the Blazor app
make watch    # Run with hot reload
make test     # Run all tests
make clean    # Clean build artifacts
make help     # Show all available commands
```

### Run with PowerShell Scripts (Windows)

The repository includes scripts that run setup steps in parallel:

```powershell
# Full initialization (install-libs + migrate + generate cert — all in parallel)
.\etc\scripts\initialize-solution.ps1

# Database migration only
.\etc\scripts\migrate-database.ps1
```

After running `initialize-solution.ps1`, start the app:

```powershell
dotnet run --project src\LayeredDemo.Blazor
```

### Run with .NET Aspire (Windows)

Aspire orchestrates PostgreSQL (via Docker), database migrations, and the Blazor app — all with a single command. **Requires Docker Desktop running.**

```powershell
dotnet run --project src\LayeredDemo.AppHost
```

Aspire will:
1. Start a PostgreSQL container with pgAdmin
2. Run the DbMigrator (waits for PostgreSQL to be ready)
3. Start the Blazor app (waits for migrations to complete)
4. Open the Aspire Dashboard for monitoring

You do **not** need a local PostgreSQL installation when using Aspire — it manages the database in Docker with a persistent named volume (`layered-demo-postgres-data`).

> **Note:** You still need to generate the OpenIddict certificate before the first run. Run `make cert` or see [HTTPS Certificate](#https-certificate-openiddict).

---

## Linux Setup

### Install Prerequisites on Linux (Ubuntu/Debian)

**1. .NET 10 SDK**

```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET SDK
sudo apt-get update
sudo apt-get install -y dotnet-sdk-10.0
```

Or follow the official guide: https://learn.microsoft.com/dotnet/core/install/linux-ubuntu

**2. Node.js (via NodeSource)**

```bash
# Install Node.js 20.x LTS
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs
```

**3. PostgreSQL**

```bash
sudo apt-get install -y postgresql postgresql-contrib

# Start the service
sudo systemctl start postgresql
sudo systemctl enable postgresql

# Set password for the postgres user
sudo -u postgres psql -c "ALTER USER postgres WITH PASSWORD 'postgres';"
```

**4. Git**

```bash
sudo apt-get install -y git
```

**5. Build tools (for make)**

```bash
sudo apt-get install -y build-essential
```

**6. Docker (only for Aspire)**

```bash
# Install Docker Engine
sudo apt-get install -y docker.io
sudo systemctl start docker
sudo systemctl enable docker
sudo usermod -aG docker $USER
# Log out and back in for the group change to take effect
```

**Verify installations:**

```bash
dotnet --version    # Should show 10.0.x
node --version      # Should show v18.x or v20.x
npm --version       # Comes with Node.js
psql --version      # Should show 14+
git --version
make --version
abp --version       # After installing ABP CLI
```

### Run Manually (Linux)

**1. Clone and navigate to the project:**

```bash
git clone git@github.com:erkantaylan/abp-modular-demo.git
cd abp-modular-demo/demos/layered-demo
```

**2. Install client-side libraries:**

```bash
abp install-libs
```

If this fails, try:

```bash
cd src/LayeredDemo.Blazor
npm install
cd ../..
abp install-libs
```

**3. Configure the database** (if not using defaults):

Edit the connection string in both:
- `src/LayeredDemo.Blazor/appsettings.json`
- `src/LayeredDemo.DbMigrator/appsettings.json`

See [Database Configuration](#database-configuration) for details.

**4. Run database migrations:**

```bash
dotnet run --project src/LayeredDemo.DbMigrator
```

**5. Generate the OpenIddict certificate:**

```bash
cd src/LayeredDemo.Blazor
dotnet dev-certs https -v -ep openiddict.pfx -p 77a0c366-a637-445b-bcf0-6406b68816ac
cd ../..
```

> On Linux, `dotnet dev-certs https --trust` is not supported natively. See [Troubleshooting](#linux-https-certificate-trust) for browser-specific workarounds.

**6. Run the application:**

```bash
dotnet run --project src/LayeredDemo.Blazor
```

Open **https://localhost:44377** and log in with:
- Username: `admin`
- Password: `1q2w3E*`

### Run with Makefile (Linux)

```bash
make setup    # Full setup: restore + install-libs + migrate
make cert     # Generate OpenIddict signing certificate
make build    # Build the solution
make run      # Run the Blazor app
make watch    # Run with hot reload
make test     # Run all tests
make clean    # Clean build artifacts
make help     # Show all available commands
```

### Run with .NET Aspire (Linux)

Requires Docker to be running.

```bash
dotnet run --project src/LayeredDemo.AppHost
```

Aspire will manage PostgreSQL via Docker, run migrations, and start the Blazor app. See the [Windows Aspire section](#run-with-net-aspire-windows) for details on what Aspire provides.

> **Note:** Generate the OpenIddict certificate before the first run: `make cert`.

---

## Database Configuration

The default connection string is:

```
Host=localhost;Port=5432;Database=LayeredDemo;Username=postgres;Password=postgres
```

This is configured in two places:

| File | Purpose |
|------|---------|
| `src/LayeredDemo.Blazor/appsettings.json` | Runtime connection for the web app |
| `src/LayeredDemo.DbMigrator/appsettings.json` | Connection for migrations and seeding |

**Both files must use the same connection string.** Update the `ConnectionStrings.Default` value in each if your PostgreSQL setup differs from the defaults.

When using **.NET Aspire**, the connection string is injected automatically via environment variables — the values in `appsettings.json` are overridden at runtime.

### Initial Data

The DbMigrator seeds:
- The `admin` user with password `1q2w3E*`
- OpenIddict application registrations (`LayeredDemo_App`, `LayeredDemo_Swagger`)
- Default permissions and roles

---

## HTTPS Certificate (OpenIddict)

The application uses OpenIddict for authentication, which requires a signing certificate (`openiddict.pfx`). This file must exist in the `src/LayeredDemo.Blazor/` directory.

**Generate the certificate:**

```bash
# From the demos/layered-demo directory:
cd src/LayeredDemo.Blazor
dotnet dev-certs https -v -ep openiddict.pfx -p 77a0c366-a637-445b-bcf0-6406b68816ac
cd ../..
```

Or use the Makefile:

```bash
make cert
```

The password `77a0c366-a637-445b-bcf0-6406b68816ac` is configured in `src/LayeredDemo.Blazor/appsettings.json` under `AuthServer.CertificatePassPhrase`.

**Trust the dev certificate (Windows/macOS):**

```bash
dotnet dev-certs https --trust
```

> For production, use separate RSA certificates for signing and encryption. See the [OpenIddict documentation](https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios) and [ABP OpenIddict configuration](https://abp.io/docs/latest/Deployment/Configuring-OpenIddict#production-environment).

---

## Running Tests

The solution includes unit and integration tests:

```bash
# Run all tests
dotnet test LayeredDemo.slnx

# Or via Makefile
make test
```

Test projects:
- `test/LayeredDemo.Domain.Tests` — Domain layer tests
- `test/LayeredDemo.Application.Tests` — Application service tests
- `test/LayeredDemo.EntityFrameworkCore.Tests` — EF Core integration tests
- `test/features/LayeredDemo.Features.Todo.Tests` — Todo feature module tests

Tests use an in-memory database by default, so PostgreSQL is not required for running tests.

---

## Troubleshooting

### `abp install-libs` hangs or fails

- Verify Node.js is in your PATH: `node --version`
- Clear the npm cache: `npm cache clean --force`
- On corporate networks, configure npm proxy: `npm config set proxy http://your-proxy:port`
- Try manual npm install first, then re-run `abp install-libs`:
  ```bash
  cd src/LayeredDemo.Blazor && npm install && cd ../.. && abp install-libs
  ```

### Port 44377 already in use

**Windows:**
```powershell
netstat -ano | findstr :44377
taskkill /PID <pid> /F
```

**Linux:**
```bash
lsof -i :44377
kill <pid>
```

Or change the port in `src/LayeredDemo.Blazor/Properties/launchSettings.json`.

### Database connection refused

- **Verify PostgreSQL is running:**
  - Windows: `Get-Service postgresql*` or check the Services app
  - Linux: `sudo systemctl status postgresql`
- **Test the connection:** `psql -U postgres -h localhost`
- **Check credentials** match the connection string in `appsettings.json`
- **Ensure the PostgreSQL service allows password authentication** — check `pg_hba.conf` for `md5` or `scram-sha-256` on localhost connections

### SSL/HTTPS certificate errors

- **Windows/macOS:** Run `dotnet dev-certs https --trust` and restart your browser
- **Firefox:** Add a security exception for `https://localhost:44377`

### Linux HTTPS certificate trust

`dotnet dev-certs https --trust` is not supported on Linux. Options:

- **Chromium/Chrome:** Launch with `--ignore-certificate-errors` for localhost (development only)
- **Firefox:** Navigate to `https://localhost:44377`, click "Advanced", then "Accept the Risk and Continue"
- **System-wide:** Export the certificate and add it to your system's CA store:
  ```bash
  dotnet dev-certs https -ep /tmp/aspnetcore-dev.pem --format pem --no-password
  sudo cp /tmp/aspnetcore-dev.pem /usr/local/share/ca-certificates/aspnetcore-dev.crt
  sudo update-ca-certificates
  ```

### `dotnet restore` fails with package errors

- Verify `NuGet.Config` is present in the solution root
- Clear the NuGet cache: `dotnet nuget locals all --clear`
- Re-run: `dotnet restore LayeredDemo.slnx`

### Aspire fails to start

- Ensure Docker is running: `docker info`
- Check if port 5432 is already in use by a local PostgreSQL instance — stop it or let Aspire use a different port
- View Aspire logs in the Dashboard for detailed error messages
