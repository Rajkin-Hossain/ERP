using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Orchestrator.Contracts.Products.MessageCommands;
using BuildingBlocks.Orchestrator.Contracts.Products.MessageEvents;
using MassTransit;
using System.Diagnostics;

namespace ERP.Products.Messaging.RabbitMQ.Consumers;

public class CreateProductCommandHandler : IConsumer<CreateProductCommand>
{
    public async Task Consume(ConsumeContext<CreateProductCommand> context)
    {
        Debug.WriteLine("CreateProductCommandHandler invoked");
    }
}


public class Test(IServiceBus bus)
{
    public async Task SendTestCommand()
    {
        await bus.PublishAsync(new ProductCreatedEvent(Guid.NewGuid()), CancellationToken.None);
    }
}