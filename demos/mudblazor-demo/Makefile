# ABP MudBlazorDemo Makefile
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
SOLUTION := MudBlazorDemo.slnx
SRC_DIR := src
TEST_DIR := test
BLAZOR_PROJECT := $(SRC_DIR)/MudBlazorDemo.Blazor/MudBlazorDemo.Blazor.csproj
MIGRATOR_PROJECT := $(SRC_DIR)/MudBlazorDemo.DbMigrator/MudBlazorDemo.DbMigrator.csproj

# .NET commands
DOTNET := dotnet
ABP := abp

# Configuration
CONFIGURATION := Debug

.PHONY: all help restore build run migrate test clean install-libs setup cert

# Default target
all: help

# Show help
help:
	@echo "ABP MudBlazorDemo - Available Commands"
	@echo "====================================="
	@echo ""
	@echo "  make setup        - Full initial setup (restore + install-libs + migrate)"
	@echo "  make restore      - Restore NuGet packages"
	@echo "  make install-libs - Install client-side libraries (requires ABP CLI)"
	@echo "  make build        - Build the solution"
	@echo "  make run          - Run the Blazor application"
	@echo "  make migrate      - Run database migrations"
	@echo "  make test         - Run all tests"
	@echo "  make clean        - Clean build artifacts"
	@echo "  make cert         - Generate OpenIddict signing certificate"
	@echo ""
	@echo "Detected OS: $(DETECTED_OS)"
	@echo ""
	@echo "Prerequisites:"
	@echo "  - .NET 10.0+ SDK"
	@echo "  - Node.js v18 or v20"
	@echo "  - ABP CLI (dotnet tool install -g Volo.Abp.Studio.Cli)"

# Full initial setup
setup: restore install-libs migrate
	@echo "Setup complete! Run 'make run' to start the application."

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

# Run the Blazor application
run:
	@echo "Starting Blazor application..."
	@echo "Application will be available at: https://localhost:44378"
	$(DOTNET) run --project $(BLAZOR_PROJECT) --configuration $(CONFIGURATION)

# Run with watch (hot reload)
watch:
	@echo "Starting Blazor application with hot reload..."
	@echo "Application will be available at: https://localhost:44378"
	$(DOTNET) watch run --project $(BLAZOR_PROJECT) --configuration $(CONFIGURATION)

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
	-$(RMDIR_CMD) $(SRC_DIR)\MudBlazorDemo.Domain\bin $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\MudBlazorDemo.Domain\obj $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\MudBlazorDemo.Application\bin $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\MudBlazorDemo.Application\obj $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\MudBlazorDemo.Blazor\bin $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\MudBlazorDemo.Blazor\obj $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\MudBlazorDemo.DbMigrator\bin $(NULL_REDIRECT)
	-$(RMDIR_CMD) $(SRC_DIR)\MudBlazorDemo.DbMigrator\obj $(NULL_REDIRECT)
else
	-find $(SRC_DIR) -type d -name "bin" -exec $(RMDIR_CMD) {} + $(NULL_REDIRECT)
	-find $(SRC_DIR) -type d -name "obj" -exec $(RMDIR_CMD) {} + $(NULL_REDIRECT)
	-find $(TEST_DIR) -type d -name "bin" -exec $(RMDIR_CMD) {} + $(NULL_REDIRECT)
	-find $(TEST_DIR) -type d -name "obj" -exec $(RMDIR_CMD) {} + $(NULL_REDIRECT)
endif
	@echo "Clean complete."

# Generate OpenIddict signing certificate
cert:
	@echo "Generating OpenIddict signing certificate..."
	$(DOTNET) dev-certs https -v -ep openiddict.pfx -p 77a0c366-a637-445b-bcf0-6406b68816ac
	@echo "Certificate generated: openiddict.pfx"

# Build for release
release:
	@echo "Building for release..."
	$(DOTNET) build $(SOLUTION) --configuration Release

# Publish for deployment
publish:
	@echo "Publishing Blazor application..."
	$(DOTNET) publish $(BLAZOR_PROJECT) --configuration Release --output ./publish
