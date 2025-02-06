var builder = DistributedApplication.CreateBuilder(args);

// TODO: Move this to README.md
//
// From Package Manager Console
// dotnet user-secrets init --project src\AppHost\
// dotnet user-secrets set "Parameters:sqlpassword" "YourSecurePassword" --project src\AppHost\

var sqlPassword = builder.Configuration["Parameters:sqlpassword"];

if (string.IsNullOrEmpty(sqlPassword))
{
  throw new InvalidOperationException("SQL password not found in user secrets. Please set it using 'dotnet user-secrets set \"Parameters:sqlpassword\" \"YourSecurePassword\"'");
}

var sqlPasswordParameter = builder.AddParameter("sqlpassword", sqlPassword, secret: true);

var seq = builder.AddSeq("seq")
  .ExcludeFromManifest()
  .WithDataVolume()
  .WithLifetime(ContainerLifetime.Persistent)
  .WithEnvironment("ACCEPT_EULA", "Y");

var sql = builder.AddSqlServer("sql", sqlPasswordParameter)
  .WithDataVolume()
  .WithLifetime(ContainerLifetime.Persistent);

var database = sql.AddDatabase("Ardalis-AAF2Db");

var migrationService = builder.AddProject<Projects.Applicita_AAF2_MigrationService>("migration")
  .WithReference(database)
  .WaitFor(database)
  .WithReference(seq)
  .WaitFor(seq);

builder.AddProject<Projects.Applicita_AAF2_Api>("api").WithReference(seq)
  .WaitFor(seq)
  .WaitForCompletion(migrationService)
  .WithReference(database)
  .WaitFor(database);

builder.Build().Run();
