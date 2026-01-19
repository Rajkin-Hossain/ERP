using ERP.Shared.Application.Interfaces;
using MediatR;

namespace ERP.Shared.Application.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        return await unitOfWork.ExecuteWithTransactionAsync(async _ => await next(), ct); //next() calls the next behavior/handler in the pipeline which is Handle function
    }
}
