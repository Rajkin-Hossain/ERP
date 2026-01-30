using RepoArchitect.Core.Abstractions;
using RepoArchitect.Core.Models;

namespace RepoArchitect.App;

public sealed class RepoArchitectState
{
    private readonly IRepoScanner _scanner;
    private readonly RepoScanOptions _options;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private RepositorySnapshot? _snapshot;

    public RepoArchitectState(IRepoScanner scanner, RepoScanOptions options)
    {
        _scanner = scanner;
        _options = options;
    }

    public async Task<RepositorySnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default)
    {
        if (_snapshot is not null)
        {
            return _snapshot;
        }

        return await RefreshAsync(cancellationToken);
    }

    public async Task<RepositorySnapshot> RefreshAsync(CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            _snapshot = await _scanner.ScanAsync(_options, cancellationToken);
            return _snapshot;
        }
        finally
        {
            _gate.Release();
        }
    }
}
