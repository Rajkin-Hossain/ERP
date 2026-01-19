using ERP.MessageOrchestrator.Contracts.Products.MessageCommands;
using ERP.Products.Domain.Entities;
using MassTransit;

namespace ERP.Products.Messaging.RabbitMQ.MessageConsumers;

public class CreateProductMessageCommandHandler : IConsumer<CreateProductMessageCommand>
{
    public async Task Consume(ConsumeContext<CreateProductMessageCommand> context)
    {
        var message = context.Message;

        // If any of these fail, a DomainException is thrown, 
        // and MassTransit will handle it (retry/dead-letter).
        var product = Product.Create(
            message.Name,
            message.CategoryId,
            message.ImageUrl,
            message.Price
        );

        // Note: In a real implementation, you would save this to the DB here.
    }
}
