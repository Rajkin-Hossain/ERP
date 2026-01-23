using ERP.Contracts.MessageOrchestrator.Products.MessageCommands;
using MassTransit;
using System.Diagnostics;

namespace ERP.Products.Messaging.RabbitMQ.MessageConsumers;

public class UpdateProductMessageCommandHandler : IConsumer<UpdateProductMessageCommand>
{
    public async Task Consume(ConsumeContext<UpdateProductMessageCommand> context)
    {
        var message = context.Message;
        var json = System.Text.Json.JsonSerializer.Serialize(message, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

        Debug.WriteLine($"UpdateProductCommandHandler invoked with payload:\n{json}");

        await Task.CompletedTask;
    }
}





