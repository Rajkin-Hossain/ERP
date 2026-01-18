using ERP.Orchestrator.Contract.Interfaces;

namespace ERP.Orchestrator.Contract.Products.MessageCommands;

public record UpdateProductMessageCommand(Guid ProductId) : IMessageCommand;

