using ERP.Shared.Command.Contracts.Modules.Orders;
using ERP.Shared.Event.Contracts.Modules.Products;
using Wolverine;

namespace ERP.InMemorySagaOrchestrator;

public class Saga(IMessageBus messageBus)
{
    private readonly IMessageBus _messageBus = messageBus;

    public async Task ProductCreatedEventHandler(ProductCreatedEvent @event)
    {
        //Commands always send from Saga so that, the respective bounded context command handler will handle it.
        await _messageBus.SendAsync(
            new AddProductSnapshotToOrderCommand(
                @event.ProductId,
                @event.Name,
                @event.CategoryId,
                @event.ImageUrl,
                @event.Price)
        );
    }
}


