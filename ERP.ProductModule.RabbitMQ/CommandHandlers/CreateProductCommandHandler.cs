using ERP.Orchestrator.Contract.ProductModule.Commands;
using ERP.Orchestrator.Contract.ProductModule.Events;
using ERP.SharedKernal.Interfaces;
using MassTransit;
using System.Diagnostics;

namespace ERP.ProductModule.RabbitMQ.CommandHandlers;

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
        await bus.PublishEvent(new ProductCreatedEvent(Guid.NewGuid()), CancellationToken.None);
    }
}