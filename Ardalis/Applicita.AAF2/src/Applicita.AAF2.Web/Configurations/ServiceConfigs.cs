using Applicita.AAF2.Core.Interfaces;
using Applicita.AAF2.Infrastructure;
using Applicita.AAF2.Infrastructure.Email;

namespace Applicita.AAF2.Web.Configurations;

public static class ServiceConfigs
{
  public static IHostApplicationBuilder AddServiceConfigs(this IHostApplicationBuilder builder, Microsoft.Extensions.Logging.ILogger logger)
  {
    builder.AddInfrastructureServices()
            .Services.AddMediatrConfigs();
    
    if (builder.Environment.IsDevelopment())
    {
      // Use a local test email server
      // See: https://ardalis.com/configuring-a-local-test-email-server/
      builder.Services.AddScoped<IEmailSender, MimeKitEmailSender>();

      // Otherwise use this:
      //builder.Services.AddScoped<IEmailSender, FakeEmailSender>();

    }
    else
    {
      builder.Services.AddScoped<IEmailSender, MimeKitEmailSender>();
    }

    logger.LogInformation("{Project} services registered", "Mediatr and Email Sender");

    return builder;
  }
}
