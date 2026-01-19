using ERP.Contracts.MessageOrchestrator.Interfaces;

namespace ERP.Contracts.MessageOrchestrator.Products.MessageCommands;

public sealed record UpdateProductPriceMessageCommand(
    Guid ProductId,
    decimal NewPrice) : IMessageCommand;
