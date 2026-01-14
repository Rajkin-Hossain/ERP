using ERP.Orchestrator.Contract.ProductModule.Commands;
using MassTransit;
using System.Diagnostics;

namespace ERP.ProductModule.Infrastructure.CommandHandlers;

public class UpdateProductCommandHandler : IConsumer<UpdateProductCommand>
{
    public async Task Consume(ConsumeContext<UpdateProductCommand> context)
    {
        Debug.WriteLine($"Received DeleteProductCommand for ProductId: {context.Message.ProductId}");
    }
}
