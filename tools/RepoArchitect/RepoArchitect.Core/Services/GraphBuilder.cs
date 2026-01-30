using RepoArchitect.Core.Abstractions;
using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Services;

public sealed class GraphBuilder : IGraphBuilder
{
    public GraphData BuildGraph(IReadOnlyCollection<ProjectInfo> projects, string rootPath)
    {
        var nodes = projects
            .Select(p => new GraphNode(
                Id: p.Name,
                Label: p.Name,
                Path: p.RelativePath,
                Folder: GetFolder(p.RelativePath)))
            .OrderBy(n => n.Label)
            .ToList();

        var edges = new List<GraphEdge>();
        var projectLookup = projects.ToDictionary(p => p.Path, p => p, StringComparer.OrdinalIgnoreCase);

        foreach (var project in projects)
        {
            foreach (var reference in project.ProjectReferences)
            {
                if (reference.RelativePath is null)
                {
                    continue;
                }

                if (!projectLookup.TryGetValue(reference.RelativePath, out var target))
                {
                    continue;
                }

                edges.Add(new GraphEdge(project.Name, target.Name));
            }
        }

        return new GraphData(nodes, edges.Distinct().ToList());
    }

    public IReadOnlyList<IReadOnlyList<string>> DetectCycles(GraphData graph)
    {
        var adjacency = graph.Edges
            .GroupBy(e => e.Source)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Target).ToList());

        var index = 0;
        var stack = new Stack<string>();
        var indices = new Dictionary<string, int>();
        var lowlink = new Dictionary<string, int>();
        var onStack = new HashSet<string>();
        var cycles = new List<IReadOnlyList<string>>();

        foreach (var node in graph.Nodes)
        {
            if (!indices.ContainsKey(node.Id))
            {
                StrongConnect(node.Id, adjacency, ref index, stack, indices, lowlink, onStack, cycles);
            }
        }

        return cycles;
    }

    private static void StrongConnect(
        string node,
        Dictionary<string, List<string>> adjacency,
        ref int index,
        Stack<string> stack,
        Dictionary<string, int> indices,
        Dictionary<string, int> lowlink,
        HashSet<string> onStack,
        List<IReadOnlyList<string>> cycles)
    {
        indices[node] = index;
        lowlink[node] = index;
        index++;
        stack.Push(node);
        onStack.Add(node);

        if (adjacency.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!indices.ContainsKey(neighbor))
                {
                    StrongConnect(neighbor, adjacency, ref index, stack, indices, lowlink, onStack, cycles);
                    lowlink[node] = Math.Min(lowlink[node], lowlink[neighbor]);
                }
                else if (onStack.Contains(neighbor))
                {
                    lowlink[node] = Math.Min(lowlink[node], indices[neighbor]);
                }
            }
        }

        if (lowlink[node] == indices[node])
        {
            var component = new List<string>();
            string? current;
            do
            {
                current = stack.Pop();
                onStack.Remove(current);
                component.Add(current);
            } while (current != node && stack.Count > 0);

            if (component.Count > 1)
            {
                cycles.Add(component);
            }
        }
    }

    private static string GetFolder(string relativePath)
    {
        var directory = Path.GetDirectoryName(relativePath) ?? string.Empty;
        return directory.Replace(Path.DirectorySeparatorChar, '/');
    }
}
