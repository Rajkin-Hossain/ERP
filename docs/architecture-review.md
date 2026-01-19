# Architecture Review Report

> Scope: Microservice-based ERP mono-repo. The analysis is focused on the **Products** bounded context as a standalone microservice, with attention to Clean Architecture and DDD boundaries. This review assumes each bounded context is independently deployed and owns its data store.

## Assumptions & Observations
- Current assemblies follow naming conventions like `*.Domain`, `*.Application`, `*.Presentation`, and `*.Api`, with infrastructure split into `ERP.Products.Persistance.MongoDb`, `ERP.Products.Messaging.RabbitMQ`, and `ERP.Products.Dispatcher.Hangfire`.
- The **Products** service composes multiple assemblies at the API boundary (API references presentation + infrastructure packages).

---

## A. What is done well

### Clean Architecture adherence
- **Domain purity**: Domain types are kept free of ORM or web-framework references, with aggregate roots and value objects encapsulating state and validation.
- **Application isolation**: Application commands/handlers depend on abstractions (repositories/interfaces) rather than infrastructure implementations.
- **Composition root**: The API layer wires together infrastructure/presentation/application dependencies, leaving lower layers free of framework coupling.

### DDD strengths
- **Aggregate root with domain events**: `Product` is an aggregate root that emits domain events for create/update/price change.
- **Value object usage**: `ProductId`, `ProductName`, and other value objects encapsulate invariants and immutability.
- **Explicit domain events**: Domain events are modeled as records and captured by the aggregate.

### Microservice-readiness positives
- **Dedicated Products assemblies**: Clear separation of product-specific domain, application, and infrastructure assemblies.
- **Shared abstractions**: Shared kernel is isolated into `ERP.Shared.*` projects, enabling reuse without direct coupling to Products.

---

## B. Microservice Boundary Health (Products Service)

### Does Products behave like a true service?
- **Mostly yes**: Products is structured as its own domain, application, infrastructure, and API layers. The API project acts as the composition root and wires internal dependencies.

### Hidden coupling risks
- **API directly references infrastructure assemblies**: `ERP.Products.Api` references persistence, messaging, and dispatcher assemblies, which is correct for composition but makes it easy for runtime-only concerns to leak into the API layer if not enforced by tests or conventions.
- **Shared kernel expansion risk**: `ERP.Shared.Domain` contains outbox-related infrastructure-like concerns. This is generic but could evolve into cross-service coupling if service-specific rules creep into shared packages.

### Shared abstractions that are dangerous
- **Outbox shared domain entity**: Sharing outbox models across services can be acceptable, but if any service-specific policies or business behavior enters `ERP.Shared.Domain`, it will create implicit coupling.

---

## C. Clean Architecture Layering

### Domain
- **Good**: Aggregate roots and value objects are contained in the domain assembly.
- **No ORM concerns**: Domain entities are not decorated with persistence attributes.
- **Risk**: Watch for any future additions of EF/MongoDB annotations or ASP.NET references.

### Application
- **Good**: Application uses repositories/interfaces to abstract persistence; no infrastructure references found.
- **Clean**: Commands/handlers are infrastructure-agnostic and utilize MediatR.

### Infrastructure
- **Good**: Infrastructure references application/domain as expected.
- **Risk**: Persistence + messaging + dispatcher are split, but currently all are referenced from API; keep boundaries firm to avoid API logic leaking into infrastructure.

### API / Presentation
- **Good**: Presentation layer maps requests to commands and is thin.
- **Risk**: API composes all dependencies and could inadvertently become a “god layer” unless tests enforce strict references.

---

## D. DDD Health Check

- **Aggregate roots & invariants**: `Product` aggregate encapsulates mutation behavior and raises domain events. Invariants are validated in value objects.
- **Value Objects**: Consistent use of records for `ProductId`, `ProductName`, `Price`, etc.
- **Domain Events**: Domain events are explicit and attached to aggregate operations.
- **Repositories & Unit of Work**: Repository abstractions live in application; infrastructure implements repositories and unit of work.
- **Anti-patterns**:
  - No clear anemic domain model observed; domain includes behavior for creation and updates.
  - Watch for application services that could become god services as complexity grows.

---

## E. Shared Code Risk Analysis

### What is safe
- **Entity/aggregate base types**: Shared `Entity<TId>` and `AggregateRoot<TId>` are generic and reusable.
- **Cross-cutting abstractions**: Shared application interfaces and result types are appropriately generic.

### What is risky
- **Outbox in shared domain**: Shared outbox models can drift into service-specific logic, creating hidden coupling.

### What should NEVER go there
- Any domain entities or value objects tied to a bounded context.
- Service-specific workflows, invariants, or policies.
- Infrastructure implementations or transport-specific dependencies.

### Rules going forward
- Shared projects must stay **dependency-light** and **business-logic-free**.
- `Orchestrator.Contracts` should contain only contracts/events/DTOs; no domain behavior or dependencies.

---

## F. Microservice Extraction Readiness

### If Products moves to its own repo tomorrow, what breaks?
- **Build configuration & solution wiring**: Project references and solution structure would need relocation, but the code itself is largely self-contained.
- **Shared kernel dependencies**: `ERP.Shared.*` would need to become packages or submodules.
- **Orchestrator contracts**: Contracts would need to be consumed as a package to preserve async integration.

### Required changes
- Package the shared kernel (`ERP.Shared.*`) and orchestrator contracts for external consumption.
- Replace any internal project references with package references.

### Hidden coupling
- Shared domain outbox type could tie products to a shared persistence model if not carefully managed.

---

## G. Top 5 High-Impact Improvements (Minimal Changes)

1. **Add architecture tests (done)** to enforce clean layering and microservice boundaries.
2. **Document shared-kernel rules** in a dedicated architecture doc to prevent context leakage.
3. **Clarify infrastructure naming conventions** (e.g., `Persistance.MongoDb` vs `Infrastructure`) for new services.
4. **Add explicit API boundary policies** to prevent direct domain exposure in API DTOs.
5. **Plan shared-kernel packaging strategy** (NuGet or git submodule) ahead of service extraction.
