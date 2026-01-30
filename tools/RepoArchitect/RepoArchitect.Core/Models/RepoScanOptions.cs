namespace RepoArchitect.Core.Models;

public sealed record RepoScanOptions(
    string RootPath,
    string? RulesPath = null,
    bool EnableCache = true,
    bool EnableAi = true);
