using ERP.MessageOrchestrator.Contract.Interfaces;

namespace ERP.MessageOrchestrator.Contract.Products.MessageCommands;

public record UpdateProductMessageCommand(Guid ProductId) : IMessageCommand;

