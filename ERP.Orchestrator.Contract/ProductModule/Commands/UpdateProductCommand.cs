using ERP.Orchestrator.Contract.Interfaces;

namespace ERP.Orchestrator.Contract.ProductModule.Commands;

public record UpdateProductCommand(Guid ProductId) : ICommand;
