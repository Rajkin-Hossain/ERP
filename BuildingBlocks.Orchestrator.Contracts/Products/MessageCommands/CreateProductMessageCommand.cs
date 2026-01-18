using BuildingBlocks.Orchestrator.Contracts.Interfaces;

namespace BuildingBlocks.Orchestrator.Contracts.Products.MessageCommands;

public record CreateProductMessageCommand(Guid ProductId) : IMessageCommand;
