using ERP.MessageOrchestrator.Contracts.Products.MessageCommands;
using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using MassTransit;

namespace ERP.Products.Messaging.RabbitMQ.MessageConsumers;

public class CreateProductMessageCommandHandler : IConsumer<CreateProductMessageCommand>
{
    public async Task Consume(ConsumeContext<CreateProductMessageCommand> context)
    {
        var message = context.Message;

        var product = Product.Create(
            new ProductName(message.Name),
            new CategoryId(message.CategoryId),
            new ImageUrl(message.ImageUrl),
            new Price(message.Price)
        );
    }
}




