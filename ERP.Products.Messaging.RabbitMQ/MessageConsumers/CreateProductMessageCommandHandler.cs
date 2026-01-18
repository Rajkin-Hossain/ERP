using ERP.Orchestrator.Contract.Products.MessageCommands;
using ERP.Orchestrator.Contract.Products.MessageEvents;
using MassTransit;
using System.Diagnostics;
using ERP.Products.Messaging.RabbitMQ.Interfaces;

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
