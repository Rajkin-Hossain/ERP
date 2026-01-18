using BuildingBlocks.Orchestrator.Contracts.Interfaces;

namespace BuildingBlocks.Orchestrator.Contracts.Products.MessageEvents;

public record ProductCreatedMessageEvent(Guid ProductId) : IMessageEvent;
