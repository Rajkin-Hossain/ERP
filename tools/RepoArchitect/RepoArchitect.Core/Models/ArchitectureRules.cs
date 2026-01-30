namespace RepoArchitect.Core.Models;

public sealed record ArchitectureRuleSet(IReadOnlyList<ArchitectureRule> Rules)
{
    public static ArchitectureRuleSet Empty { get; } = new(Array.Empty<ArchitectureRule>());
}

public sealed record ArchitectureRule(
    string Name,
    string From,
    string To,
    string Reason);
