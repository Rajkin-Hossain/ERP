using ERP.Shared.Integration.Interfaces;

namespace ERP.Shared.Integration.ProductModule.Commands;

public record CreateProductCommand(Guid ProductId) : ICommand;



