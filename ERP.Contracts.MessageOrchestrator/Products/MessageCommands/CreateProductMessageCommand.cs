using ERP.Contracts.MessageOrchestrator.Interfaces;

namespace ERP.Contracts.MessageOrchestrator.Products.MessageCommands;

public record CreateProductMessageCommand(
    Guid ProductId,
    string Name,
    Guid CategoryId,
    string ImageUrl,
    decimal Price) : IMessageCommand;





