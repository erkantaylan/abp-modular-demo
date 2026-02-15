var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume("angular-demo-postgres-data")
    .PublishAsContainer();

var database = postgres.AddDatabase("Default");

// Install ABP client-side libs (login/account MVC pages need them)
var installLibs = builder.AddExecutable("install-libs", "abp", "../AngularDemo.HttpApi.Host", "install-libs")
    .ExcludeFromManifest();

var dbMigrator = builder.AddProject<Projects.AngularDemo_DbMigrator>("dbmigrator")
    .WithReference(database)
    .WaitFor(database);

var apiHost = builder.AddProject<Projects.AngularDemo_HttpApi_Host>("httpapi-host")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitForCompletion(dbMigrator)
    .WaitForCompletion(installLibs);

builder.AddJavaScriptApp("angular", "../../angular", "start")
    .WithNpm()
    .WithReference(apiHost)
    .WaitFor(apiHost)
    .WithHttpEndpoint(env: "PORT");

builder.Build().Run();
