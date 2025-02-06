using Applicita.AAF2.Core.Interfaces;
using Applicita.AAF2.Core.Services;
using Applicita.AAF2.Infrastructure.Data;
using Applicita.AAF2.Infrastructure.Data.Queries;
using Applicita.AAF2.UseCases.Contributors.List;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;


namespace Applicita.AAF2.Infrastructure;
public static class InfrastructureServiceExtensions
{
  //public static IServiceCollection AddInfrastructureServices(
  //  this IServiceCollection services,
  //  ConfigurationManager config,
  //  ILogger logger)
  //{
  //  string? connectionString = config.GetConnectionString("Ardalis-AAF2Db");
  //  Guard.Against.Null(connectionString);
  //  services.AddDbContextPool<AppDbContext>(options =>
  //    options.UseSqlServer(config.GetConnectionString("Ardalis-AAF2Db"), sqlOptions =>
  //    {
  //      // Workaround for https://github.com/dotnet/aspire/issues/1023
  //      //sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
  //    }));

  //    // builder.EnrichSqlServerDbContext<AppDbContext>(settings =>
  //    // Disable Aspire default retries as we're using a custom execution strategy
  //    //settings.DisableRetry = true);

  //  services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
  //         .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
  //         .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
  //         .AddScoped<IDeleteContributorService, DeleteContributorService>();


  //  logger.LogInformation("{Project} services registered", "Infrastructure");

  //  return services;
  //}

  public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
  {
    var connectionString = builder.Configuration.GetConnectionString("Ardalis-AAF2Db");
    Guard.Against.Null(connectionString, message: "Connection string 'AAF2Db' not found.");

    //builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
    //builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

    builder.Services.AddDbContext<AppDbContext>((sp, options) =>
    {
      options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
      options.UseSqlServer(connectionString, sqlOptions =>
      {
        sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
      });
    });
    
    builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
             .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
             .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
             .AddScoped<IDeleteContributorService, DeleteContributorService>();

    return builder;
  }
}
