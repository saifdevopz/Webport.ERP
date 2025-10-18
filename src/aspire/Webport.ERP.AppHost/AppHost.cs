using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("aspire-docker-demo");

//var postgresInstance = builder.AddPostgres("postgres")
//                              .WithDataVolume("aspire-postgres-data")
//                              .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050));

//var postgresDb = postgresInstance.AddDatabase("demo-db");

//var redisInstance = builder.AddRedis("cache");

builder.AddProject<Webport_ERP_Api>("WebportERP");
//.WithReference(postgresDb).WaitFor(postgresInstance)
//.WithReference(redisInstance).WaitFor(redisInstance);

//builder.AddProject<BlazorDashboard>("BlazorDashboard");

await builder.Build().RunAsync();
