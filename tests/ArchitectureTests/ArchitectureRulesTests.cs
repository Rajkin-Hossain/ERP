using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace ArchitectureTests;

public class ArchitectureRulesTests
{
    private static readonly Assembly ProductsDomain = LoadAssembly("ERP.Products.Domain");
    private static readonly Assembly ProductsApplication = LoadAssembly("ERP.Products.Application");
    private static readonly Assembly ProductsPersistence = LoadAssembly("ERP.Products.Persistance.MongoDb");
    private static readonly Assembly ProductsMessaging = LoadAssembly("ERP.Products.Messaging.RabbitMQ");
    private static readonly Assembly ProductsDispatcher = LoadAssembly("ERP.Products.Dispatcher.Hangfire");
    private static readonly Assembly ProductsPresentation = LoadAssembly("ERP.Products.Presentation");
    private static readonly Assembly ProductsApi = LoadAssembly("ERP.Products.Api");
    private static readonly Assembly SharedDomain = LoadAssembly("ERP.Shared.Domain");
    private static readonly Assembly SharedApplication = LoadAssembly("ERP.Shared.Application");
    private static readonly Assembly SharedPresentation = LoadAssembly("ERP.Shared.Presentation");
    private static readonly Assembly OrchestratorContracts = LoadAssembly("ERP.MessageOrchestrator.Contracts");

    private static readonly Assembly[] ProductServiceAssemblies =
    [
        ProductsDomain,
        ProductsApplication,
        ProductsPersistence,
        ProductsMessaging,
        ProductsDispatcher,
        ProductsPresentation,
        ProductsApi
    ];

    [Fact]
    public void Domain_Should_Not_Depend_On_Infrastructure_Or_Presentation_Technologies()
    {
        AssertNoDependency(
            ProductsDomain,
            "ERP.Products.Persistance.MongoDb",
            "ERP.Products.Messaging.RabbitMQ",
            "ERP.Products.Dispatcher.Hangfire",
            "ERP.Products.Api",
            "ERP.Products.Presentation",
            "Microsoft.AspNetCore",
            "Microsoft.EntityFrameworkCore",
            "MongoDB",
            "Hangfire",
            "MassTransit",
            "RabbitMQ");
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure_Or_Presentation_Technologies()
    {
        AssertNoDependency(
            ProductsApplication,
            "ERP.Products.Persistance.MongoDb",
            "ERP.Products.Messaging.RabbitMQ",
            "ERP.Products.Dispatcher.Hangfire",
            "ERP.Products.Api",
            "ERP.Products.Presentation",
            "Microsoft.AspNetCore",
            "Microsoft.EntityFrameworkCore",
            "MongoDB",
            "Hangfire",
            "MassTransit",
            "RabbitMQ");
    }

    [Fact]
    public void Infrastructure_Should_Not_Depend_On_Api_Or_Presentation()
    {
        var infrastructureAssemblies = new[]
        {
            ProductsPersistence,
            ProductsMessaging,
            ProductsDispatcher
        };

        foreach (var assembly in infrastructureAssemblies)
        {
            AssertNoDependency(
                assembly,
                "ERP.Products.Api",
                "ERP.Products.Presentation");
        }
    }

    [Fact]
    public void Api_Should_Not_Depend_On_Domain_Directly()
    {
        AssertNoDependency(
            ProductsApi,
            "ERP.Products.Domain");
    }

    [Fact]
    public void Product_Service_Should_Not_Depend_On_Other_Bounded_Contexts()
    {
        foreach (var assembly in ProductServiceAssemblies)
        {
            AssertNoDependency(
                assembly,
                "ERP.MessageOrchestrator",
                "ERP.AppHost");
        }
    }

    [Fact]
    public void Shared_Kernel_Should_Not_Depend_On_Bounded_Contexts()
    {
        var sharedAssemblies = new[]
        {
            SharedDomain,
            SharedApplication,
            SharedPresentation
        };

        foreach (var assembly in sharedAssemblies)
        {
            AssertNoDependency(
                assembly,
                "ERP.Products",
                "ERP.MessageOrchestrator");
        }
    }

    [Fact]
    public void Orchestrator_Contracts_Should_Be_Dependence_Free()
    {
        AssertNoDependency(
            OrchestratorContracts,
            "ERP.Products",
            "ERP.Shared",
            "Microsoft.AspNetCore",
            "Microsoft.EntityFrameworkCore",
            "MongoDB");
    }

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

        Assert.True(
            result.IsSuccessful,
            $"{assembly.GetName().Name} has forbidden dependencies: {string.Join(", ", result.FailingTypes.Select(t => t.FullName))}");
    }
}
