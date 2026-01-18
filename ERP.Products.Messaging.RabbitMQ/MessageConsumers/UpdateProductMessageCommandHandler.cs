using ERP.MessageOrchestrator.Contract.Products.MessageCommands;
using MassTransit;
using System.Diagnostics;

namespace ERP.Products.Messaging.RabbitMQ.MessageConsumers;

public class UpdateProductMessageCommandHandler : IConsumer<UpdateProductMessageCommand>
{
    public async Task Consume(ConsumeContext<UpdateProductMessageCommand> context)
    {
        Debug.WriteLine($"Received DeleteProductCommand for ProductId: {context.Message.ProductId}");
    }
}

