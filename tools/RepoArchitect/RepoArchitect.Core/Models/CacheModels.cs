namespace RepoArchitect.Core.Models;

public sealed record ProjectCacheEntry(
    string ProjectPath,
    DateTimeOffset LastWriteTimeUtc,
    ProjectInfo Project);

public sealed record RepoCache(IReadOnlyDictionary<string, ProjectCacheEntry> Entries)
{
    public static RepoCache Empty { get; } = new(new Dictionary<string, ProjectCacheEntry>(StringComparer.OrdinalIgnoreCase));
}
