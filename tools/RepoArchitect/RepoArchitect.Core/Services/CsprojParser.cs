using System.Xml.Linq;
using RepoArchitect.Core.Abstractions;
using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Services;

public sealed class CsprojParser : IProjectParser
{
    public Task<ProjectInfo> ParseAsync(string projectFilePath, CancellationToken cancellationToken = default)
    {
        var doc = XDocument.Load(projectFilePath);
        var projectName = Path.GetFileNameWithoutExtension(projectFilePath);
        var projectDir = Path.GetDirectoryName(projectFilePath) ?? string.Empty;

        var assemblyName = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "AssemblyName")?.Value;
        var rootNamespace = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "RootNamespace")?.Value;

        var targetFramework = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "TargetFramework")?.Value;
        var targetFrameworks = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "TargetFrameworks")?.Value;
        var tfmList = new List<string>();

        if (!string.IsNullOrWhiteSpace(targetFramework))
        {
            tfmList.Add(targetFramework);
        }
        else if (!string.IsNullOrWhiteSpace(targetFrameworks))
        {
            tfmList.AddRange(targetFrameworks.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }

        var projectReferences = doc.Descendants()
            .Where(e => e.Name.LocalName == "ProjectReference")
            .Select(e =>
            {
                var include = e.Attribute("Include")?.Value ?? string.Empty;
                var fullPath = Path.GetFullPath(Path.Combine(projectDir, include));
                var name = Path.GetFileNameWithoutExtension(include);
                return new ProjectReferenceInfo(include, name, fullPath);
            })
            .ToList();

        var packageReferences = doc.Descendants()
            .Where(e => e.Name.LocalName == "PackageReference")
            .Select(e =>
            {
                var id = e.Attribute("Include")?.Value ?? e.Attribute("Update")?.Value ?? string.Empty;
                var version = e.Attribute("Version")?.Value ?? e.Element(XName.Get("Version"))?.Value;
                return new PackageReferenceInfo(id, version);
            })
            .Where(p => !string.IsNullOrWhiteSpace(p.Id))
            .ToList();

        var info = new ProjectInfo(
            Name: projectName,
            Path: projectFilePath,
            RelativePath: projectFilePath,
            AssemblyName: assemblyName,
            RootNamespace: rootNamespace,
            TargetFrameworks: tfmList,
            ProjectReferences: projectReferences,
            PackageReferences: packageReferences);

        return Task.FromResult(info);
    }
}
