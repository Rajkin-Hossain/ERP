using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Orchestrator.Contracts.Products.MessageCommands;
using BuildingBlocks.Orchestrator.Contracts.Products.MessageEvents;
using MassTransit;
using System.Diagnostics;

namespace ERP.Products.Messaging.RabbitMQ.Consumers;

public class CreateProductCommandHandler : IConsumer<CreateProductMessageCommand>
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