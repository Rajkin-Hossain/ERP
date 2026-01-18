using ERP.MessageOrchestrator.Contracts.Interfaces;

namespace ERP.MessageOrchestrator.Contracts.Products.MessageCommands;

public record UpdateProductMessageCommand(Guid ProductId) : IMessageCommand;




