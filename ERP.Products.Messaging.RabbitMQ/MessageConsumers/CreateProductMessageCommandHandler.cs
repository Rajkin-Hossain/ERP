using ERP.MessageOrchestrator.Contracts.Products.MessageCommands;
using ERP.MessageOrchestrator.Contracts.Products.MessageEvents;
using ERP.Products.Messaging.RabbitMQ.Interfaces;
using MassTransit;
using System.Diagnostics;

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




