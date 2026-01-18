using ERP.MessageOrchestrator.Contract.Interfaces;

namespace ERP.MessageOrchestrator.Contract.ProductModule.Commands;

public record UpdateProductCommand(Guid ProductId) : ICommand;
