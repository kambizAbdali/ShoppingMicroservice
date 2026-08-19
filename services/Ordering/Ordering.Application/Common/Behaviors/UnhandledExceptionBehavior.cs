using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Exceptions;
using ApplicationException = Ordering.Application.Common.Exceptions.ApplicationException;

namespace Ordering.Application.Common.Behaviors;

/// <summary>
/// A global MediatR pipeline behavior that catches every unhandled exception,
/// logs it, and translates unexpected errors into a standard application exception.
/// </summary>
public class UnhandledExceptionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;

    public UnhandledExceptionBehavior(
        ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        try
        {
            _logger.LogInformation(
                "Handling request: {RequestName} - {@Request}",
                requestName, request);

            var response = await next();

            _logger.LogInformation(
                "Handled request: {RequestName}",
                requestName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred while handling {RequestName} - {@Request}",
                requestName, request);

            // Translate unexpected exceptions into a standard application exception.
            throw TranslateException(ex, requestName);
        }
    }

    private static Exception TranslateException(Exception ex, string requestName)
    {
        // Pass known domain exceptions through unchanged,
        // since they already carry a specific meaning for the upper layer.
        if (ex is ApplicationException)
        {
            return ex;
        }

        // Translate unexpected errors into a standard internal-server-error exception.
        return new OperationFailedException(
            $"An unexpected error occurred while processing '{requestName}'.",
            ex);
    }
}
