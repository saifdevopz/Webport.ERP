using Projects;

var builder = DistributedApplication.CreateBuilder(args);

//builder.AddDockerComposeEnvironment("aspire-docker-demo");

//builder.AddContainer("grafana", "grafana/grafana")
//       .WithBindMount("../../../compose/grafana/config", "/etc/grafana", isReadOnly: true)
//       .WithBindMount("../../../compose/grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
//       .WithHttpEndpoint(port: 3000, targetPort: 3000, name: "http");

//builder.AddContainer("prometheus", "prom/prometheus")
//       .WithBindMount("../../../compose/prometheus", "/etc/prometheus", isReadOnly: true)
//       .WithHttpEndpoint(port: 9090, targetPort: 9090);



//var username = builder.AddParameter("pg-username", "admin");
//var password = builder.AddParameter("pg-password", "admin");

var postgresInstance = builder.AddPostgres("postgres"/*, username, password, port: 5432*/)
                              .WithDataVolume();
/*   .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050)*/

var postgresDb = postgresInstance.AddDatabase("demo-db");

//var redisInstance = builder.AddRedis("cache");

var api =
builder.AddProject<Webport_ERP_Api>("apiservice")
.WithReference(postgresDb).WaitFor(postgresInstance);
//.WithReference(redisInstance).WaitFor(redisInstance);

builder.AddProject<BlazorDashboard>("BlazorDashboard")
    .WithExternalHttpEndpoints()
    .WithReference(api)
    .WaitFor(api);

await builder.Build().RunAsync();


