using ERP.Shared.Integration.Products.MessageCommands;
using ERP.Shared.Integration.Products.MessageEvents;
using ERP.Products.Messaging.RabbitMQ.Interfaces;
using MassTransit;
using System.Diagnostics;

using ERP.Products.Application.Interfaces;
namespace ERP.Products.Messaging.RabbitMQ.MessageConsumers;

public class CreateProductMessageCommandHandler : IConsumer<CreateProductMessageCommand>
{
    public async Task Consume(ConsumeContext<CreateProductMessageCommand> context)
    {
        Debug.WriteLine("CreateProductCommandHandler invoked");
    }
}


public class Test(IServiceBus bus)
{
    public async Task SendTestCommand()
    {
        await bus.PublishAsync(new ProductCreatedMessageEvent(Guid.NewGuid()), CancellationToken.None);
    }
}




