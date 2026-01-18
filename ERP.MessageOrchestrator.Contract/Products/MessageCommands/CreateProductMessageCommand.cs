using ERP.MessageOrchestrator.Contract.Interfaces;

namespace ERP.MessageOrchestrator.Contract.Products.MessageCommands;

public record CreateProductMessageCommand(Guid ProductId) : IMessageCommand;

