var builder = DistributedApplication.CreateBuilder(args);

var dbMigrator = builder.AddProject<Projects.LayeredDemo_DbMigrator>("dbmigrator");

builder.AddProject<Projects.LayeredDemo_Blazor>("blazor")
    .WithExternalHttpEndpoints()
    .WithHttpsHealthCheck("/health")
    .WaitForCompletion(dbMigrator);

builder.Build().Run();
