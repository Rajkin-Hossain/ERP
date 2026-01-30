using System.Text.RegularExpressions;
using RepoArchitect.Core.Abstractions;
using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Services;

public sealed class RuleEvaluator : IRuleEvaluator
{
    public Task<IReadOnlyList<ArchitectureViolation>> EvaluateAsync(
        IReadOnlyCollection<ProjectInfo> projects,
        IReadOnlyCollection<GraphEdge> edges,
        ArchitectureRuleSet rules,
        CancellationToken cancellationToken = default)
    {
        var projectLookup = projects.ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);
        var violations = new List<ArchitectureViolation>();

        foreach (var edge in edges)
        {
            if (!projectLookup.TryGetValue(edge.Source, out var fromProject) ||
                !projectLookup.TryGetValue(edge.Target, out var toProject))
            {
                continue;
            }

            foreach (var rule in rules.Rules)
            {
                if (IsMatch(fromProject, rule.From) && IsMatch(toProject, rule.To))
                {
                    violations.Add(new ArchitectureViolation(rule.Name, fromProject.Name, toProject.Name, rule.Reason));
                }
            }
        }

        return Task.FromResult<IReadOnlyList<ArchitectureViolation>>(violations);
    }

    private static bool IsMatch(ProjectInfo project, string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            return false;
        }

        var normalizedPath = project.RelativePath.Replace(Path.DirectorySeparatorChar, '/');
        return GlobMatch(project.Name, pattern) || GlobMatch(normalizedPath, pattern);
    }

    private static bool GlobMatch(string input, string pattern)
    {
        var regexPattern = "^" + Regex.Escape(pattern)
            .Replace("\\*", ".*")
            .Replace("\\?", ".") + "$";
        return Regex.IsMatch(input, regexPattern, RegexOptions.IgnoreCase);
    }
}
