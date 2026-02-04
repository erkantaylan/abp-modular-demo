var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .PublishAsContainer();

var database = postgres.AddDatabase("LayeredDemo");

var dbMigrator = builder.AddProject<Projects.LayeredDemo_DbMigrator>("dbmigrator")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.LayeredDemo_Blazor>("blazor")
    .WithExternalHttpEndpoints()
    .WithHttpsHealthCheck("/health")
    .WithReference(database)
    .WaitForCompletion(dbMigrator);

builder.Build().Run();
