using NetArchTest.Rules;
using System.Reflection;
using Xunit;

namespace ArchitectureTests;

public class ArchitectureRulesTests
{
    // Shared Kernel Assemblies
    private static readonly Assembly SharedDomain = LoadAssembly("ERP.Shared.Domain");
    private static readonly Assembly SharedApplication = LoadAssembly("ERP.Shared.Application");
    private static readonly Assembly SharedPresentation = LoadAssembly("ERP.Shared.Presentation");

    // Message Orchestrator Assemblies
    private static readonly Assembly OrchestratorImplementation = LoadAssembly("ERP.MessageOrchestrator");
    private static readonly Assembly OrchestratorContracts = LoadAssembly("ERP.Contracts.MessageOrchestrator");

    // Products Module Assemblies
    private static readonly Assembly ProductsDomain = LoadAssembly("ERP.Products.Domain");
    private static readonly Assembly ProductsApplication = LoadAssembly("ERP.Products.Application");
    private static readonly Assembly ProductsPersistence = LoadAssembly("ERP.Products.Persistance.MongoDb");
    private static readonly Assembly ProductsMessaging = LoadAssembly("ERP.Products.Messaging.RabbitMQ");
    private static readonly Assembly ProductsDispatcher = LoadAssembly("ERP.Products.Dispatcher.Hangfire");
    private static readonly Assembly ProductsPresentation = LoadAssembly("ERP.Products.Presentation");
    private static readonly Assembly ProductsApi = LoadAssembly("ERP.Products.Api");

    // All Product Module Assemblies
    private static readonly Assembly[] ProductModuleAssemblies =
    [
        ProductsDomain,
        ProductsApplication,
        ProductsPersistence,
        ProductsMessaging,
        ProductsDispatcher,
        ProductsPresentation,
        ProductsApi
    ];

    private static readonly Assembly[] InfrastructureAssemblies =
    [
        ProductsPersistence,
        ProductsMessaging,
        ProductsDispatcher
    ];

    #region Shared Kernel Rules

    [Fact]
    public void Shared_Domain_Should_Have_No_Dependencies()
    {
        AssertNoDependency(
            SharedDomain,
            "ERP.Shared.Application",
            "ERP.Shared.Presentation",
            "ERP.Products",
            "ERP.MessageOrchestrator",
            "ERP.Contracts.MessageOrchestrator");
    }

    [Fact]
    public void Shared_Application_Should_Only_Depend_On_Shared_Domain()
    {
        AssertNoDependency(
            SharedApplication,
            "ERP.Shared.Presentation",
            "ERP.Products",
            "ERP.MessageOrchestrator",
            "ERP.Contracts.MessageOrchestrator");
    }

    [Fact]
    public void Shared_Presentation_Should_Only_Depend_On_Shared_Application_And_Domain()
    {
        AssertNoDependency(
            SharedPresentation,
            "ERP.Products",
            "ERP.MessageOrchestrator",
            "ERP.Contracts.MessageOrchestrator");
    }

    #endregion

    #region Message Orchestrator Rules

    [Fact]
    public void Orchestrator_Contracts_Should_Have_No_Dependencies()
    {
        AssertNoDependency(
            OrchestratorContracts,
            "ERP.Shared",
            "ERP.Products",
            "ERP.MessageOrchestrator");
    }

    [Fact]
    public void Orchestrator_Implementation_Should_Only_Depend_On_Contracts_And_Shared()
    {
        AssertNoDependency(
            OrchestratorImplementation,
            "ERP.Products");
    }

    #endregion

    #region Module Layer Rules (Products)

    [Fact]
    public void Products_Domain_Should_Only_Depend_On_Shared_Domain()
    {
        AssertNoDependency(
            ProductsDomain,
            "ERP.Products.Application",
            "ERP.Products.Persistance",
            "ERP.Products.Messaging",
            "ERP.Products.Dispatcher",
            "ERP.Products.Presentation",
            "ERP.Products.Api",
            "ERP.Shared.Application",
            "ERP.Shared.Presentation");
    }

    [Fact]
    public void Products_Application_Should_Only_Depend_On_Domain_And_Shared()
    {
        AssertNoDependency(
            ProductsApplication,
            "ERP.Products.Persistance",
            "ERP.Products.Messaging",
            "ERP.Products.Dispatcher",
            "ERP.Products.Presentation",
            "ERP.Products.Api",
            "ERP.Shared.Presentation");
    }

    [Fact]
    public void Products_Infrastructure_Should_Only_Depend_On_Application_And_Shared()
    {
        foreach (var assembly in InfrastructureAssemblies)
        {
            AssertNoDependency(
                assembly,
                "ERP.Products.Presentation",
                "ERP.Products.Api",
                "ERP.Shared.Presentation");
        }
    }

    [Fact]
    public void Products_Infrastructure_Projects_Should_Not_Depend_On_Each_Other()
    {
        AssertNoDependency(ProductsPersistence, "ERP.Products.Messaging", "ERP.Products.Dispatcher");
        AssertNoDependency(ProductsMessaging, "ERP.Products.Persistance", "ERP.Products.Dispatcher");
        AssertNoDependency(ProductsDispatcher, "ERP.Products.Persistance", "ERP.Products.Messaging");
    }

    [Fact]
    public void Products_Presentation_Should_Only_Depend_On_Application_And_Shared()
    {
        AssertNoDependency(
            ProductsPresentation,
            "ERP.Products.Persistance",
            "ERP.Products.Messaging",
            "ERP.Products.Dispatcher",
            "ERP.Products.Api");
    }

    [Fact]
    public void Products_Api_Should_Only_Depend_On_Allowed_Layers()
    {
        // Api -> Presentation, Infrastructure, Application
        // Should NOT depend on Domain directly (as per previous rules, though often debated, the user said Api -> Presentation, Infrastructure, Application)
        AssertNoDependency(
            ProductsApi,
            "ERP.Products.Domain");
    }

    #endregion

    #region Cross-Module Isolation

    [Fact]
    public void Products_Module_Should_Not_Depend_On_Other_Modules_Implementation()
    {
        foreach (var assembly in ProductModuleAssemblies)
        {
            AssertNoDependency(
                assembly,
                "ERP.MessageOrchestrator", // Implementation
                "ERP.AppHost");
        }
        
        // Only Messaging is allowed to depend on Orchestrator Contracts (specific rule from previous conversation)
        foreach (var assembly in ProductModuleAssemblies)
        {
            if (assembly != ProductsMessaging)
            {
                AssertNoDependency(assembly, "ERP.Contracts.MessageOrchestrator");
            }
        }
    }

    #endregion

    private static Assembly LoadAssembly(string assemblyName)
    {
        return Assembly.Load(assemblyName);
    }

    private static void AssertNoDependency(Assembly assembly, params string[] forbiddenDependencies)
    {
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenDependencies)
            .GetResult();

        if (result.IsSuccessful)
        {
            return;
        }

        var failingTypes = result.FailingTypes != null
            ? string.Join(", ", result.FailingTypes.Select(t => t.FullName))
            : "None";

        Assert.True(
            result.IsSuccessful,
            $"{assembly.GetName().Name} has forbidden dependencies: {failingTypes}");
    }
}
