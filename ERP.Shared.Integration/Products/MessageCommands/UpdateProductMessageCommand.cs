using ERP.Shared.Integration.Interfaces;

namespace ERP.Shared.Integration.Products.MessageCommands;

public record UpdateProductMessageCommand(Guid ProductId) : IMessageCommand;




