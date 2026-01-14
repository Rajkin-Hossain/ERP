using ERP.Orchestrator.Contract.Interfaces;

namespace ERP.Orchestrator.Contract.ProductModule.Commands;

public record CreateProductCommand(Guid ProductId) : ICommand;
