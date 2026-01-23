using ERP.Products.Application.Interfaces;
using MediatR;

namespace ERP.Products.Application.Behaviors;

public sealed class OutboxDispatchBehavior<TRequest, TResponse>(IOutboxDispatchTrigger outboxDispatchTrigger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITriggerOutbox
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next();

        outboxDispatchTrigger.EnqueueJob();

        return response;
    }
}
