# Products Module Dependency Matrix

## Layer rules

- Domain has no dependencies on other layers.
- Application depends on Domain.
- Infrastructure depends on Application and Domain.
- Presentation depends on Application (and Infrastructure for composition only).

## Project references

| Project | Depends on | Purpose |
| --- | --- | --- |
| ERP.Products.Domain | - | Entities, value objects, domain events, invariants |
| ERP.Products.Application.Abstraction | ERP.Shared.Domain, ERP.Shared.Event.Contracts, ERP.Shared.Application.Abstractions | Application contracts and ports |
| ERP.Products.Application | ERP.Products.Domain, ERP.Products.Application.Abstraction, ERP.Shared.Event.Contracts | Orchestration, domain-event mapping, policies |
| ERP.Products.CommandHandlers | ERP.Products.Domain, ERP.Shared.Application.Abstractions | Command use cases |
| ERP.Products.QueryHandler | ERP.Shared.Application.Abstractions | Query use cases |
| ERP.Products.Persistance.PgSQL | ERP.Products.Domain, ERP.Products.Application.Abstraction, ERP.Shared.Infrastructures | EF Core, repositories, outbox, unit of work |
| ERP.Products.Messaging.InMemory | ERP.Products.Application.Abstraction, ERP.Shared.Event.Contracts, ERP.Shared.Infrastructures | Integration event publishing |
| ERP.Products.Api | ERP.Products.Application, ERP.Products.CommandHandlers, ERP.Products.QueryHandler, ERP.Products.Persistance.PgSQL | Composition root |

## Integration event flow (DDD boundary)

1) Domain raises domain events.
2) Application maps domain events to integration events.
3) Outbox stores integration events only.
4) Messaging publishes integration events to the saga orchestrator.
