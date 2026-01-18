using ERP.MessageOrchestrator.Contracts.Products.MessageCommands;
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




