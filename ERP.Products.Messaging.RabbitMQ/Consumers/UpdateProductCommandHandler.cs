using BuildingBlocks.Orchestrator.Contracts.Products.MessageCommands;
using MassTransit;
using System.Diagnostics;

namespace ERP.Products.Messaging.RabbitMQ.Consumers;

public class UpdateProductCommandHandler : IConsumer<UpdateProductCommand>
{
    public async Task Consume(ConsumeContext<UpdateProductCommand> context)
    {
        Debug.WriteLine($"Received DeleteProductCommand for ProductId: {context.Message.ProductId}");
    }
}
