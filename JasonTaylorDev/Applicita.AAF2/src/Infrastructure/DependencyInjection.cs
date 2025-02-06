using Applicita.AAF2.Application.Common.Interfaces;
using Applicita.AAF2.Domain.Constants;
using Applicita.AAF2.Infrastructure.Data;
using Applicita.AAF2.Infrastructure.Data.Interceptors;
using Applicita.AAF2.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("AAF2Db");
        Guard.Against.Null(connectionString, message: "Connection string 'AAF2Db' not found.");
        
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
            });
        });

        //builder.Services.AddDbContextPool<ApplicationDbContext>((sp,options) =>
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("db1"), sqlOptions =>
        //    {
        //        options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        //        // Workaround for https://github.com/dotnet/aspire/issues/1023
        //        sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
        //    }));

        builder.EnrichSqlServerDbContext<ApplicationDbContext>(settings =>
            // Disable Aspire default retries as we're using a custom execution strategy
            settings.DisableRetry = true);
        
        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        builder.Services.AddAuthorizationBuilder();

        builder.Services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.Services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
    }
}
