using ERP.Shared.Command.Contracts.Interfaces;

namespace ERP.Shared.Command.Contracts.Modules.Products;

public sealed record CreateProductCommand(string Name,
    decimal Price,
    string ImageUrl,
    string CategoryId) : ICommand;