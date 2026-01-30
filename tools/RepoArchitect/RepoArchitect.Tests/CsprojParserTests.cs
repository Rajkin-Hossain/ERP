using RepoArchitect.Core.Services;
using Xunit;

namespace RepoArchitect.Tests;

public class CsprojParserTests
{
    [Fact]
    public async Task ParsesProjectAndPackageReferences()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var projectPath = Path.Combine(tempDir, "Sample.csproj");

        await File.WriteAllTextAsync(projectPath, @"<Project Sdk=\"Microsoft.NET.Sdk\">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <AssemblyName>Sample.Assembly</AssemblyName>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include=\"..\\Other\\Other.csproj\" />
    <PackageReference Include=\"Serilog\" Version=\"2.12.0\" />
  </ItemGroup>
</Project>");

        var parser = new CsprojParser();
        var info = await parser.ParseAsync(projectPath);

        Assert.Equal("Sample", info.Name);
        Assert.Equal("Sample.Assembly", info.AssemblyName);
        Assert.Single(info.ProjectReferences);
        Assert.Single(info.PackageReferences);
        Assert.Equal("Serilog", info.PackageReferences[0].Id);
    }
}
