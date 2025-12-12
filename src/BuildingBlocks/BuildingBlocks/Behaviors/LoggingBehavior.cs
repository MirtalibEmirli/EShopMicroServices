using BuildingBlocks.CQRS;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest,TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest :notnull, ICommand<TResponse>
    where TResponse :notnull 
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {RequestName} with payload: {@Request}", typeof(TRequest).Name, request);
        var timer = System.Diagnostics.Stopwatch.StartNew();
        var response = await  next();
        timer.Stop();
        if(timer.ElapsedMilliseconds > 300)
        {
            logger.LogWarning("Long Running Request: {RequestName} took {ElapsedMilliseconds}ms",
                typeof(TRequest).Name, timer.ElapsedMilliseconds);
        }
       return response;
    }
}
