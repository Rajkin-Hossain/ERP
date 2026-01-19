# Architecture Tests

## How to run

```bash
dotnet test
```

## Updating assembly name patterns

The tests load assemblies by name (for example, `ERP.Products.Domain`, `ERP.Products.Persistance.MongoDb`).
If a project name changes, update the constants in `ArchitectureRulesTests.cs` to match the new
assembly names.

## Extending rules for new microservices

1. Add the new microservice assemblies to the test project via `ProjectReference` in
   `ArchitectureTests.csproj`.
2. Create new assembly lists in `ArchitectureRulesTests.cs` (for example, `OrdersDomain`,
   `OrdersApplication`, `OrdersApi`, and any infrastructure assemblies).
3. Reuse the existing test helpers to enforce Clean Architecture rules and the shared-kernel
   boundaries for the new bounded context.
