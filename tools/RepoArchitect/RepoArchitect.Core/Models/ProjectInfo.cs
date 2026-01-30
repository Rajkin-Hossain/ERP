namespace RepoArchitect.Core.Models;

public sealed record ProjectInfo(
    string Name,
    string Path,
    string RelativePath,
    string? AssemblyName,
    string? RootNamespace,
    IReadOnlyList<string> TargetFrameworks,
    IReadOnlyList<ProjectReferenceInfo> ProjectReferences,
    IReadOnlyList<PackageReferenceInfo> PackageReferences);

public sealed record ProjectReferenceInfo(string Include, string? ProjectName, string? RelativePath);

public sealed record PackageReferenceInfo(string Id, string? Version);
