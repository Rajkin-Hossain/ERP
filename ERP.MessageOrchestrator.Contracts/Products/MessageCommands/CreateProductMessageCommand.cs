using ERP.MessageOrchestrator.Contracts.Interfaces;

namespace ERP.MessageOrchestrator.Contracts.Products.MessageCommands;

public record CreateProductMessageCommand(Guid ProductId) : IMessageCommand;




