using BuildingBlocks.Orchestrator.Contracts.Products.MessageCommands;
using MassTransit;
using System.Diagnostics;

namespace ERP.Products.Messaging.RabbitMQ.Consumers;

public class UpdateProductCommandHandler : IConsumer<UpdateProductMessageCommand>
{
    public async Task Consume(ConsumeContext<UpdateProductMessageCommand> context)
    {
        Debug.WriteLine($"Received DeleteProductCommand for ProductId: {context.Message.ProductId}");
    }
}
