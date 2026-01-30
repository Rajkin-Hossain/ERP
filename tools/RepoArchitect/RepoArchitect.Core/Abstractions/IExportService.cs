using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Abstractions;

public interface IExportService
{
    string ExportGraphJson(RepositorySnapshot snapshot);
    string ExportGraphDot(RepositorySnapshot snapshot);
}
