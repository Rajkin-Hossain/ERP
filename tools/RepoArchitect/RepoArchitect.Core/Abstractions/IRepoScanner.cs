using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Abstractions;

public interface IRepoScanner
{
    Task<RepositorySnapshot> ScanAsync(RepoScanOptions options, CancellationToken cancellationToken = default);
}
