using ERP.Orchestrator.Contract.Interfaces;

namespace ERP.Orchestrator.Contract.Products.MessageCommands;

public record CreateProductMessageCommand(Guid ProductId) : IMessageCommand;

