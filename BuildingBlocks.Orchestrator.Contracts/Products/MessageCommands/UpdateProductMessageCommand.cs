using BuildingBlocks.Orchestrator.Contracts.Interfaces;

namespace BuildingBlocks.Orchestrator.Contracts.Products.MessageCommands;

public record UpdateProductMessageCommand(Guid ProductId) : IMessageCommand;
