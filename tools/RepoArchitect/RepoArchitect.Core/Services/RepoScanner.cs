using System.Text.Json;
using RepoArchitect.Core.Abstractions;
using RepoArchitect.Core.Models;
using RepoArchitect.Core.Utilities;

namespace RepoArchitect.Core.Services;

public sealed class RepoScanner : IRepoScanner
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly HashSet<string> IgnoredDirectories = new(StringComparer.OrdinalIgnoreCase)
    {
        "bin",
        "obj",
        ".git",
        ".vs",
        "node_modules"
    };

    private readonly IProjectParser _parser;
    private readonly IGraphBuilder _graphBuilder;
    private readonly IRuleEvaluator _ruleEvaluator;
    private readonly CacheStore _cacheStore;

    public RepoScanner(
        IProjectParser parser,
        IGraphBuilder graphBuilder,
        IRuleEvaluator ruleEvaluator,
        CacheStore cacheStore)
    {
        _parser = parser;
        _graphBuilder = graphBuilder;
        _ruleEvaluator = ruleEvaluator;
        _cacheStore = cacheStore;
    }

    public async Task<RepositorySnapshot> ScanAsync(RepoScanOptions options, CancellationToken cancellationToken = default)
    {
        var rootPath = RepoRootDetector.FindRepoRoot(options.RootPath);
        var rulesPath = options.RulesPath ?? Path.Combine(rootPath, "tools", "RepoArchitect", "config", "architecture-rules.json");
        var cachePath = Path.Combine(rootPath, ".repoarchitect", "cache.json");

        var cache = options.EnableCache
            ? await _cacheStore.LoadAsync(cachePath, cancellationToken)
            : RepoCache.Empty;

        var projectFiles = EnumerateProjectFiles(rootPath).ToList();
        var projectInfos = new List<ProjectInfo>();
        var cacheEntries = new Dictionary<string, ProjectCacheEntry>(StringComparer.OrdinalIgnoreCase);

        foreach (var projectFile in projectFiles)
        {
            var lastWrite = File.GetLastWriteTimeUtc(projectFile);
            if (options.EnableCache && cache.Entries.TryGetValue(projectFile, out var cached) &&
                cached.LastWriteTimeUtc.UtcDateTime == lastWrite)
            {
                projectInfos.Add(RewritePaths(cached.Project, rootPath));
                cacheEntries[projectFile] = cached;
                continue;
            }

            var parsed = await _parser.ParseAsync(projectFile, cancellationToken);
            var normalized = RewritePaths(parsed, rootPath);
            projectInfos.Add(normalized);
            cacheEntries[projectFile] = new ProjectCacheEntry(projectFile, lastWrite, normalized);
        }

        if (options.EnableCache)
        {
            await _cacheStore.SaveAsync(cachePath, new RepoCache(cacheEntries), cancellationToken);
        }

        var graph = _graphBuilder.BuildGraph(projectInfos, rootPath);
        var cycles = _graphBuilder.DetectCycles(graph);
        var rules = await LoadRulesAsync(rulesPath, cancellationToken);
        var violations = await _ruleEvaluator.EvaluateAsync(projectInfos, graph.Edges, rules, cancellationToken);

        return new RepositorySnapshot(
            RootPath: rootPath,
            Projects: projectInfos,
            Graph: graph,
            Cycles: cycles,
            Violations: violations,
            ScannedAt: DateTimeOffset.UtcNow);
    }

    private static IEnumerable<string> EnumerateProjectFiles(string rootPath)
    {
        var stack = new Stack<string>();
        stack.Push(rootPath);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            foreach (var dir in Directory.EnumerateDirectories(current))
            {
                var name = Path.GetFileName(dir);
                if (IgnoredDirectories.Contains(name))
                {
                    continue;
                }

                stack.Push(dir);
            }

            foreach (var file in Directory.EnumerateFiles(current, "*.csproj"))
            {
                yield return file;
            }
        }
    }

    private static ProjectInfo RewritePaths(ProjectInfo project, string rootPath)
    {
        var relativePath = Path.GetRelativePath(rootPath, project.Path);
        var projectDirectory = Path.GetDirectoryName(project.Path) ?? rootPath;
        var projectReferences = project.ProjectReferences
            .Select(reference =>
            {
                var includePath = reference.Include;
                var fullPath = Path.GetFullPath(Path.Combine(projectDirectory, includePath));
                return reference with { RelativePath = fullPath };
            })
            .ToList();

        return project with
        {
            RelativePath = relativePath,
            ProjectReferences = projectReferences
        };
    }

    private static async Task<ArchitectureRuleSet> LoadRulesAsync(string rulesPath, CancellationToken cancellationToken)
    {
        if (!File.Exists(rulesPath))
        {
            return ArchitectureRuleSet.Empty;
        }

        await using var stream = File.OpenRead(rulesPath);
        var rules = await JsonSerializer.DeserializeAsync<ArchitectureRuleSet>(stream, JsonOptions, cancellationToken);
        return rules ?? ArchitectureRuleSet.Empty;
    }
}
