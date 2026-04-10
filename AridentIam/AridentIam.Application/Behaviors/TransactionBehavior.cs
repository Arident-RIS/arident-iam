using AridentIam.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AridentIam.Application.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (IsQueryRequest())
            return await next(cancellationToken);

        var requestName = typeof(TRequest).Name;

        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            var response = await next(cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Transaction committed for {RequestName}", requestName);

            return response;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Transaction rolled back for {RequestName}", requestName);
            throw;
        }
    }

    private static bool IsQueryRequest()
    {
        var name = typeof(TRequest).Name;
        return name.EndsWith("Query", StringComparison.Ordinal);
    }
}
