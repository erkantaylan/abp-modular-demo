# ABP AngularDemo Makefile
# Cross-platform support for Windows and Linux

# Detect OS
ifeq ($(OS),Windows_NT)
    DETECTED_OS := Windows
    RM_CMD := del /q /s
    RMDIR_CMD := rmdir /q /s
    PATH_SEP := \\
    NULL_REDIRECT := >nul 2>&1
else
    DETECTED_OS := $(shell uname -s)
    RM_CMD := rm -rf
    RMDIR_CMD := rm -rf
    PATH_SEP := /
    NULL_REDIRECT := >/dev/null 2>&1
endif

# Project paths
SOLUTION := AngularDemo.slnx
SRC_DIR := src
TEST_DIR := test
ANGULAR_DIR := angular
HOST_PROJECT := $(SRC_DIR)/AngularDemo.HttpApi.Host/AngularDemo.HttpApi.Host.csproj
MIGRATOR_PROJECT := $(SRC_DIR)/AngularDemo.DbMigrator/AngularDemo.DbMigrator.csproj

# .NET commands
DOTNET := dotnet
ABP := abp
NPM := npm
NG := npx ng

# Configuration
CONFIGURATION := Debug

.PHONY: all help restore build run migrate test clean install-libs setup cert ng-install ng-start ng-build

# Default target
all: help

# Show help
help:
	@echo "ABP AngularDemo - Available Commands"
	@echo "====================================="
	@echo ""
	@echo "  make setup        - Full initial setup (restore + install-libs + ng-install + migrate)"
	@echo "  make restore      - Restore NuGet packages"
	@echo "  make install-libs - Install client-side libraries (requires ABP CLI)"
	@echo "  make build        - Build the .NET solution"
	@echo "  make run          - Run the API Host application"
	@echo "  make watch        - Run the API Host with hot reload"
	@echo "  make migrate      - Run database migrations"
	@echo "  make test         - Run all .NET tests"
	@echo "  make clean        - Clean build artifacts"
	@echo "  make cert         - Generate OpenIddict signing certificate"
	@echo "  make ng-install   - Install Angular npm dependencies"
	@echo "  make ng-start     - Start Angular dev server"
	@echo "  make ng-build     - Build Angular app for production"
	@echo ""
	@echo "Detected OS: $(DETECTED_OS)"
	@echo ""
	@echo "Prerequisites:"
	@echo "  - .NET 10.0+ SDK"
	@echo "  - Node.js v18 or v20"
	@echo "  - ABP CLI (dotnet tool install -g Volo.Abp.Studio.Cli)"

# Full initial setup
setup: restore install-libs ng-install migrate
	@echo "Setup complete! Run 'make run' to start the API, then 'make ng-start' for Angular."

# Restore NuGet packages
restore:
	@echo "Restoring NuGet packages..."
	$(DOTNET) restore $(SOLUTION)

# Install client-side libraries using ABP CLI
install-libs:
	@echo "Installing client-side libraries..."
	$(ABP) install-libs

# Build the solution
build: restore
	@echo "Building solution..."
	$(DOTNET) build $(SOLUTION) --configuration $(CONFIGURATION) --no-restore

# Run database migrations
migrate: build
	@echo "Running database migrations..."
	$(DOTNET) run --project $(MIGRATOR_PROJECT) --configuration $(CONFIGURATION) --no-build

# Run the API Host application
run:
	@echo "Starting API Host application..."
	@echo "API will be available at: https://localhost:44340"
	@echo "Swagger UI: https://localhost:44340/swagger"
	$(DOTNET) run --project $(HOST_PROJECT) --configuration $(CONFIGURATION)

# Run with watch (hot reload)
watch:
	@echo "Starting API Host with hot reload..."
	@echo "API will be available at: https://localhost:44340"
	$(DOTNET) watch run --project $(HOST_PROJECT) --configuration $(CONFIGURATION)

# Run all tests
test: build
	@echo "Running tests..."
	$(DOTNET) test $(SOLUTION) --configuration $(CONFIGURATION) --no-build

# Clean build artifacts
clean:
	@echo "Cleaning build artifacts..."
	$(DOTNET) clean $(SOLUTION) --configuration $(CONFIGURATION) $(NULL_REDIRECT) || true
	$(DOTNET) clean $(SOLUTION) --configuration Release $(NULL_REDIRECT) || true
ifeq ($(OS),Windows_NT)
	-$(RMDIR_CMD) $(SRC_DIR)\AngularDemo.Domain\bin $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\AngularDemo.Domain\obj $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\AngularDemo.Application\bin $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\AngularDemo.Application\obj $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\AngularDemo.HttpApi.Host\bin $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\AngularDemo.HttpApi.Host\obj $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\AngularDemo.DbMigrator\bin $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\AngularDemo.DbMigrator\obj $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(ANGULAR_DIR)\node_modules $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(ANGULAR_DIR)\dist $(NULL_REDIRECT)
else
	-find $(SRC_DIR) -type d -name "bin" -exec $(RMDIR_CMD) {} + $(NULL_REDIRECT)
	-find $(SRC_DIR) -type d -name "obj" -exec $(RMDIR_CMD) {} + $(NULL_REDIRECT)
	-find $(TEST_DIR) -type d -name "bin" -exec $(RMDIR_CMD) {} + $(NULL_REDIRECT)
	-find $(TEST_DIR) -type d -name "obj" -exec $(RMDIR_CMD) {} + $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(ANGULAR_DIR)/node_modules $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(ANGULAR_DIR)/dist $(NULL_REDIRECT)
endif
	@echo "Clean complete."

# Generate OpenIddict signing certificate
cert:
	@echo "Generating OpenIddict signing certificate..."
	cd $(SRC_DIR)/AngularDemo.HttpApi.Host && $(DOTNET) dev-certs https -v -ep openiddict.pfx -p 9643d757-38d9-4daf-a731-849512598df6
	@echo "Certificate generated: $(SRC_DIR)/AngularDemo.HttpApi.Host/openiddict.pfx"

# Install Angular npm dependencies
ng-install:
	@echo "Installing Angular dependencies..."
	cd $(ANGULAR_DIR) && $(NPM) install

# Start Angular development server
ng-start:
	@echo "Starting Angular dev server..."
	@echo "Angular app will be available at: http://localhost:4200"
	cd $(ANGULAR_DIR) && $(NPM) start

# Build Angular app for production
ng-build:
	@echo "Building Angular app for production..."
	cd $(ANGULAR_DIR) && $(NPM) run build -- --configuration production

# Build for release
release:
	@echo "Building for release..."
	$(DOTNET) build $(SOLUTION) --configuration Release

# Publish for deployment
publish:
	@echo "Publishing API Host application..."
	$(DOTNET) publish $(HOST_PROJECT) --configuration Release --output ./publish
