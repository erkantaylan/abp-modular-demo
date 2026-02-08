var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume("mudblazor-demo-postgres-data")
    .PublishAsContainer();

var database = postgres.AddDatabase("Default");

var dbMigrator = builder.AddProject<Projects.MudBlazorDemo_DbMigrator>("dbmigrator")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.MudBlazorDemo_Blazor>("blazor")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitForCompletion(dbMigrator);

builder.Build().Run();
