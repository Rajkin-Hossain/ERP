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

        var nameResult = ProductName.Create(message.Name);
        var categoryIdResult = CategoryId.Create(message.CategoryId);
        var imageResult = ImageUrl.Create(message.ImageUrl);
        var priceResult = Price.Create(message.Price);

        if (!nameResult.IsSuccess || !categoryIdResult.IsSuccess || !imageResult.IsSuccess || !priceResult.IsSuccess)
        {
             // In a real system, you might move this to an error queue or log it
             throw new Exception("Invalid product data received from message bus.");
        }

        var product = Product.Create(
            nameResult.Value!,
            categoryIdResult.Value!,
            imageResult.Value!,
            priceResult.Value!
        );
        
        // Note: In a real implementation, you would save this to the DB here.
    }
}




