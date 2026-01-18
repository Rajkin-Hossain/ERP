using MassTransit;
using System.Diagnostics;

using ERP.Products.Application.Interfaces;
using ERP.MessageOrchestrator.Contracts.Products.MessageCommands;
namespace ERP.Products.Messaging.RabbitMQ.MessageConsumers;

public class UpdateProductMessageCommandHandler : IConsumer<UpdateProductMessageCommand>
{
    public async Task Consume(ConsumeContext<UpdateProductMessageCommand> context)
    {
        Debug.WriteLine($"Received DeleteProductCommand for ProductId: {context.Message.ProductId}");
    }
}





