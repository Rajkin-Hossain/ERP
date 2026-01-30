using RepoArchitect.Core.Models;
using RepoArchitect.Core.Services;
using Xunit;

namespace RepoArchitect.Tests;

public class RuleEvaluatorTests
{
    [Fact]
    public async Task FlagsForbiddenReferences()
    {
        var projects = new List<ProjectInfo>
        {
            new("Core", "Core.csproj", "src/Core/Core.csproj", null, null, Array.Empty<string>(),
                Array.Empty<ProjectReferenceInfo>(), Array.Empty<PackageReferenceInfo>()),
            new("UI", "UI.csproj", "src/UI/UI.csproj", null, null, Array.Empty<string>(),
                Array.Empty<ProjectReferenceInfo>(), Array.Empty<PackageReferenceInfo>())
        };

        var edges = new List<GraphEdge> { new("UI", "Core") };
        var rules = new ArchitectureRuleSet(new List<ArchitectureRule>
        {
            new("NoUiToCore", "*UI*", "*Core*", "UI must not depend on Core")
        });

        var evaluator = new RuleEvaluator();
        var violations = await evaluator.EvaluateAsync(projects, edges, rules);

        Assert.Single(violations);
        Assert.Equal("NoUiToCore", violations[0].RuleName);
    }
}
