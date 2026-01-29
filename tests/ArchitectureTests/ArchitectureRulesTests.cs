using NetArchTest.Rules;
using System.Reflection;
using Xunit;

namespace ArchitectureTests;

public class ArchitectureRulesTests
{
    // ======= Load assemblies via a known type from each project =======
    private static readonly Assembly SharedDomain =
        typeof(ERP.Shared.Domain.Entities.Entity<>).Assembly;

    private static readonly Assembly SharedKernel =
        typeof(ERP.Shared.Kernel.Result.AppResult<>).Assembly;

    private static readonly Assembly SharedPresentation =
        typeof(ERP.Shared.Presentation.Models.ApiResponse<>).Assembly;

    private static readonly Assembly SharedAppAbstractions =
        typeof(ERP.Shared.Application.Abstractions.Outbox.IOutboxStorage).Assembly;

    private static readonly Assembly SharedInfrastructure =
        typeof(ERP.Shared.Infrastructures.Outbox.OutboxMessage).Assembly;

    private static readonly Assembly ProductsDomain =
        typeof(ERP.Products.Domain.Entities.Product).Assembly;

    private static readonly Assembly ProductsAppAbstractions =
        typeof(ERP.Products.Application.Abstraction.Interfaces.IIntegrationEventMapper).Assembly;

    private static readonly Assembly ProductsApplication =
        typeof(ERP.Products.Application.Integration.ProductIntegrationEventMapper).Assembly;

    private static readonly Assembly ProductsCommandApi =
        typeof(ERP.Products.CommandApi.EndPoints.CreateProductEndpoint).Assembly;

    private static readonly Assembly ProductsQueryApi =
        typeof(ERP.Products.QueryApi.EndPoints.GetProductsEndpoint).Assembly;

    private static readonly Assembly ProductsCommandHandlers =
        typeof(ERP.Products.CommandHandlers.CommandHandlers.CreateProductCommandHandler).Assembly;

    private static readonly Assembly ProductsQueryHandlers =
        typeof(ERP.Products.QueryHandler.QueryHandlers.GetProductQueryHandler).Assembly;

    private static readonly Assembly ProductsEventHandlers =
        typeof(ERP.Products.EventHandler.EventHandlers.ProductCreatedEventHandler).Assembly;

    private static readonly Assembly ProductsPgSql =
        typeof(ERP.Products.Persistance.PgSQL.Data.Write.ProductDbContext).Assembly;

    private static readonly Assembly ProductsMessaging =
        typeof(ERP.Products.Messaging.InMemory.Jobs.OutboxJob).Assembly;

    private static readonly Assembly ProductsHangfire =
        typeof(ERP.Products.JobSchedule.Hangfire.JobSchedulers.JobScheduler).Assembly;

    // ======= Namespace constants (adjust if yours differ) =======
    private const string ProductsNamespace = "ERP.Products";
    private const string SharedNamespace = "ERP.Shared";

    // ---------------------------
    // SHARED RULES
    // ---------------------------

    [Fact]
    public void Shared_projects_must_not_depend_on_any_bounded_context()
    {
        // Shared.* should not depend on ERP.Products.*
        AssertNoDependencyOn(SharedDomain, ProductsNamespace);
        AssertNoDependencyOn(SharedKernel, ProductsNamespace);
        AssertNoDependencyOn(SharedPresentation, ProductsNamespace);
        AssertNoDependencyOn(SharedAppAbstractions, ProductsNamespace);
        AssertNoDependencyOn(SharedInfrastructure, ProductsNamespace);
    }

    [Fact]
    public void SharedApplicationAbstractions_must_only_depend_on_SharedDomain_and_SharedKernel()
    {
        var result = Types.InAssembly(SharedAppAbstractions)
            .ShouldNot()
            .HaveDependencyOnAny(
                $"{SharedNamespace}.Infrastructures",
                $"{SharedNamespace}.Presentation",
                $"{SharedNamespace}.Event.Contracts",
                $"{SharedNamespace}.Command.Contracts",
                $"{SharedNamespace}.Query.Contracts",
                ProductsNamespace
            )
            .GetResult();

        Assert.True(result.IsSuccessful, FormatFailure(result));
    }

    [Fact]
    public void SharedPresentation_must_not_depend_on_application_abstractions_or_infrastructure()
    {
        var result = Types.InAssembly(SharedPresentation)
            .ShouldNot()
            .HaveDependencyOnAny(
                $"{SharedNamespace}.Application.Abstractions",
                $"{SharedNamespace}.Infrastructures",
                ProductsNamespace
            )
            .GetResult();

        Assert.True(result.IsSuccessful, FormatFailure(result));
    }

    // ---------------------------
    // PRODUCTS BOUNDED CONTEXT RULES
    // ---------------------------

    [Fact]
    public void ProductsDomain_must_not_depend_on_products_infrastructure_or_contracts_or_shared_infrastructure()
    {
        var result = Types.InAssembly(ProductsDomain)
            .ShouldNot()
            .HaveDependencyOnAny(
                $"{SharedNamespace}.Infrastructures",
                $"{SharedNamespace}.Presentation",
                $"{SharedNamespace}.Event.Contracts",
                $"{SharedNamespace}.Command.Contracts",
                $"{SharedNamespace}.Query.Contracts",
                $"{ProductsNamespace}.Persistance",
                $"{ProductsNamespace}.Messaging",
                $"{ProductsNamespace}.JobSchedule",
                $"{ProductsNamespace}.CommandApi",
                $"{ProductsNamespace}.QueryApi",
                $"{ProductsNamespace}.CommandHandlers",
                $"{ProductsNamespace}.QueryHandler",
                $"{ProductsNamespace}.EventHandler"
            )
            .GetResult();

        Assert.True(result.IsSuccessful, FormatFailure(result));
    }

    [Fact]
    public void ProductsApis_must_not_depend_on_products_domain_or_products_infrastructure()
    {
        // CommandApi
        AssertNoDependencyOn(ProductsCommandApi, $"{ProductsNamespace}.Domain");
        AssertNoDependencyOn(ProductsCommandApi, $"{ProductsNamespace}.Persistance");
        AssertNoDependencyOn(ProductsCommandApi, $"{ProductsNamespace}.Messaging");
        AssertNoDependencyOn(ProductsCommandApi, $"{ProductsNamespace}.JobSchedule");

        // QueryApi
        AssertNoDependencyOn(ProductsQueryApi, $"{ProductsNamespace}.Domain");
        AssertNoDependencyOn(ProductsQueryApi, $"{ProductsNamespace}.Persistance");
        AssertNoDependencyOn(ProductsQueryApi, $"{ProductsNamespace}.Messaging");
        AssertNoDependencyOn(ProductsQueryApi, $"{ProductsNamespace}.JobSchedule");
    }

    [Fact]
    public void ProductsInfrastructure_projects_must_not_depend_on_other_bounded_contexts()
    {
        // In a multi-BC future, these infra assemblies should never depend on ERP.Orders.*, ERP.Payments.*, etc.
        // For now, enforce "no dependency on ERP.<OtherBC>" by keeping a list.
        // Example:
        var otherBoundedContexts = new[]
        {
            "ERP.Orders",
            "ERP.Payments",
            "ERP.Shipments",
            "ERP.Inventory"
        };

        foreach (var other in otherBoundedContexts)
        {
            AssertNoDependencyOn(ProductsPgSql, other);
            AssertNoDependencyOn(ProductsMessaging, other);
            AssertNoDependencyOn(ProductsHangfire, other);
        }
    }

    [Fact]
    public void Messaging_layer_should_not_reference_products_domain_if_using_integration_events_in_outbox()
    {
        // You want domain->integration mapping to live in Products.Application, not in Messaging infra.
        AssertNoDependencyOn(ProductsMessaging, $"{ProductsNamespace}.Domain");
    }

    // ---------------------------
    // HELPERS
    // ---------------------------

    private static void AssertNoDependencyOn(Assembly assembly, string forbiddenNamespace)
    {
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(forbiddenNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, FormatFailure(result));
    }

    private static string FormatFailure(TestResult result)
    {
        var failing = string.Join(Environment.NewLine, result.FailingTypeNames ?? Array.Empty<string>());
        return $"Architecture rule failed. Failing types:{Environment.NewLine}{failing}";
    }
}
