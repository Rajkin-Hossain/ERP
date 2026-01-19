using ERP.MessageOrchestrator.Contracts.Interfaces;

namespace ERP.MessageOrchestrator.Contracts.Products.MessageCommands;

public sealed record UpdateProductPriceMessageCommand(
    Guid ProductId,
    decimal NewPrice) : IMessageCommand;
