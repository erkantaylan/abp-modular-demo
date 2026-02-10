var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume("fluentui-demo-postgres-data")
    .PublishAsContainer();

var database = postgres.AddDatabase("Default");

var dbMigrator = builder.AddProject<Projects.FluentUiDemo_DbMigrator>("dbmigrator")
    .WithReference(database)
    .WaitFor(database)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);

builder.AddProject<Projects.FluentUiDemo_Blazor>("blazor")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitForCompletion(dbMigrator)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);

builder.Build().Run();
