using RepoArchitect.Core.Models;
using RepoArchitect.Core.Services;
using Xunit;

namespace RepoArchitect.Tests;

public class GraphBuilderTests
{
    [Fact]
    public void DetectsCyclesInGraph()
    {
        var projects = new List<ProjectInfo>
        {
            new("A", "A.csproj", "A.csproj", null, null, Array.Empty<string>(),
                new List<ProjectReferenceInfo> { new("B.csproj", "B", "B.csproj") }, Array.Empty<PackageReferenceInfo>()),
            new("B", "B.csproj", "B.csproj", null, null, Array.Empty<string>(),
                new List<ProjectReferenceInfo> { new("C.csproj", "C", "C.csproj") }, Array.Empty<PackageReferenceInfo>()),
            new("C", "C.csproj", "C.csproj", null, null, Array.Empty<string>(),
                new List<ProjectReferenceInfo> { new("A.csproj", "A", "A.csproj") }, Array.Empty<PackageReferenceInfo>())
        };

        var builder = new GraphBuilder();
        var graph = builder.BuildGraph(projects, Directory.GetCurrentDirectory());
        var cycles = builder.DetectCycles(graph);

        Assert.Single(cycles);
        Assert.Contains("A", cycles[0]);
        Assert.Contains("B", cycles[0]);
        Assert.Contains("C", cycles[0]);
    }
}
