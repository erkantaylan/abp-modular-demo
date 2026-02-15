var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .PublishAsContainer();

var database = postgres.AddDatabase("Default");

// Install ABP client-side libs (login/account MVC pages need them)
var installLibs = builder.AddExecutable("install-libs", "abp", "../AngularDemo.HttpApi.Host", "install-libs")
    .ExcludeFromManifest();

var angular = builder.AddJavaScriptApp("angular", "../../angular", "start")
    .WithNpm()
    .WithHttpEndpoint(env: "PORT");

var angularUrl = angular.GetEndpoint("http");

// DbMigrator registers OpenIddict clients — needs the Angular URL for redirect URIs
var dbMigrator = builder.AddProject<Projects.AngularDemo_DbMigrator>("dbmigrator")
    .WithReference(database)
    .WaitFor(database)
    .WithEnvironment("OpenIddict__Applications__AngularDemo_App__RootUrl", angularUrl);

// API host needs Angular URL for CORS and redirect allowlist
var apiHost = builder.AddProject<Projects.AngularDemo_HttpApi_Host>("httpapi-host")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitForCompletion(dbMigrator)
    .WaitForCompletion(installLibs)
    .WithEnvironment("App__AngularUrl", angularUrl)
    .WithEnvironment("App__CorsOrigins", angularUrl)
    .WithEnvironment("App__RedirectAllowedUrls", angularUrl);

// Angular needs the API host URL for OAuth and API calls
angular
    .WithReference(apiHost)
    .WaitFor(apiHost);

builder.Build().Run();
