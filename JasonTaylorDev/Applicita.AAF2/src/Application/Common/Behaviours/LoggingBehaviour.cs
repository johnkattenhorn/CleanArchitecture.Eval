using Applicita.AAF2.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Applicita.AAF2.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public LoggingBehaviour(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
    {
        _logger = logger;
        _user = user;
        _identityService = identityService;
    }
    
    //public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    //{
    //    var requestName = typeof(TRequest).Name;
    //    var userId = _user.Id ?? string.Empty;
    //    string? userName = string.Empty;

    //    if (!string.IsNullOrEmpty(userId))
    //    {
    //        userName = await _identityService.GetUserNameAsync(userId);
            
    //    }
    //    _logger.LogInformation("Handling Request: {Name} {@UserId} {@UserName} {@Request}",
    //        requestName, userId, userName, request);

    //    var response = await next();

    //    _logger.LogInformation("Handled Response: {Name} {@UserId} {@UserName} {@Response}",
    //        requestName, userId, userName, response);

    //    return response;
    //}

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = request.GetType().Name;

        try
        {
            var userId = _user.Id ?? string.Empty;
            string? userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);

            }

            _logger.LogInformation("Executing request {RequestName}", requestName);

            TResponse result = await next();

            //if (result.IsSuccess)
            //{
                _logger.LogInformation("Request {RequestName} processed successfully", requestName);
            //}
            //else
            //{
                //using (LogContext.PushProperty("Error", result.Error, true))
                //{
                //    _logger.LogError("Request {RequestName} processed with error", requestName);
                //}
            //}

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Request {RequestName} processing failed", requestName);

            throw;
        }
    }
}
