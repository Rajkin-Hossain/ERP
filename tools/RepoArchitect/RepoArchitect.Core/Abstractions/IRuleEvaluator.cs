using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Abstractions;

public interface IRuleEvaluator
{
    Task<IReadOnlyList<ArchitectureViolation>> EvaluateAsync(
        IReadOnlyCollection<ProjectInfo> projects,
        IReadOnlyCollection<GraphEdge> edges,
        ArchitectureRuleSet rules,
        CancellationToken cancellationToken = default);
}
