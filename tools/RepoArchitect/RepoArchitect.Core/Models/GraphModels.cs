namespace RepoArchitect.Core.Models;

public sealed record GraphNode(string Id, string Label, string Path, string Folder);

public sealed record GraphEdge(string Source, string Target);

public sealed record GraphData(IReadOnlyList<GraphNode> Nodes, IReadOnlyList<GraphEdge> Edges);

public sealed record ArchitectureViolation(
    string RuleName,
    string FromProject,
    string ToProject,
    string Reason);

public sealed record RepositorySnapshot(
    string RootPath,
    IReadOnlyList<ProjectInfo> Projects,
    GraphData Graph,
    IReadOnlyList<IReadOnlyList<string>> Cycles,
    IReadOnlyList<ArchitectureViolation> Violations,
    DateTimeOffset ScannedAt);
