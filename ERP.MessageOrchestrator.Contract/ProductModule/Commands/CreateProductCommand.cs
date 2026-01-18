using ERP.MessageOrchestrator.Contract.Interfaces;

namespace ERP.MessageOrchestrator.Contract.ProductModule.Commands;

public record CreateProductCommand(Guid ProductId) : ICommand;
