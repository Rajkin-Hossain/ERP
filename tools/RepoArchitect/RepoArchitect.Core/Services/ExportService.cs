using System.Text;
using System.Text.Json;
using RepoArchitect.Core.Abstractions;
using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Services;

public sealed class ExportService : IExportService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public string ExportGraphJson(RepositorySnapshot snapshot)
    {
        return JsonSerializer.Serialize(snapshot, JsonOptions);
    }

    public string ExportGraphDot(RepositorySnapshot snapshot)
    {
        var builder = new StringBuilder();
        builder.AppendLine("digraph RepoArchitect {");
        builder.AppendLine("  rankdir=LR;");

        foreach (var node in snapshot.Graph.Nodes)
        {
            builder.AppendLine($"  \"{node.Id}\" [label=\"{node.Label}\"]; ");
        }

        foreach (var edge in snapshot.Graph.Edges)
        {
            builder.AppendLine($"  \"{edge.Source}\" -> \"{edge.Target}\";");
        }

        builder.AppendLine("}");
        return builder.ToString();
    }
}
