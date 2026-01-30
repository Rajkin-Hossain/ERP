using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Abstractions;

public interface IGraphBuilder
{
    GraphData BuildGraph(IReadOnlyCollection<ProjectInfo> projects, string rootPath);
    IReadOnlyList<IReadOnlyList<string>> DetectCycles(GraphData graph);
}
