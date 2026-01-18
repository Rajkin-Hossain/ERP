using ERP.Shared.Integration.Interfaces;

namespace ERP.Shared.Integration.Products.MessageCommands;

public record CreateProductMessageCommand(Guid ProductId) : IMessageCommand;




