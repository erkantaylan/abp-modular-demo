var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume("angular-demo-postgres-data")
    .PublishAsContainer();

var database = postgres.AddDatabase("Default");

var dbMigrator = builder.AddProject<Projects.AngularDemo_DbMigrator>("dbmigrator")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.AngularDemo_HttpApi_Host>("httpapi-host")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitForCompletion(dbMigrator);

builder.Build().Run();
