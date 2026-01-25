using ERP.Shared.Command.Contracts.Interfaces;

namespace ERP.Shared.Command.Contracts.Modules.Orders;

public sealed record AddProductSnapshotToOrderCommand(
    Guid ProductId,
    string Name,
    Guid CategoryId,
    string ImageUrl,
    decimal Price) : ICommand;
