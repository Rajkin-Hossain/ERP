using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Abstractions;

public interface IProjectParser
{
    Task<ProjectInfo> ParseAsync(string projectFilePath, CancellationToken cancellationToken = default);
}
