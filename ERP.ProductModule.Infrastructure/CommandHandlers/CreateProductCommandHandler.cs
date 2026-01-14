using ERP.Orchestrator.Contract.ProductModule.Commands;
using MassTransit;
using System.Diagnostics;

namespace ERP.ProductModule.Infrastructure.CommandHandlers;

public class CreateProductCommandHandler : IConsumer<CreateProductCommand>
{
    public async Task Consume(ConsumeContext<CreateProductCommand> context)
    {
       Debug.WriteLine("CreateProductCommandHandler invoked");
    }
}