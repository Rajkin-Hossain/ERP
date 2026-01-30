using System.Text.Json;
using RepoArchitect.Core.Models;

namespace RepoArchitect.Core.Services;

public sealed class CacheStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public async Task<RepoCache> LoadAsync(string cachePath, CancellationToken cancellationToken)
    {
        if (!File.Exists(cachePath))
        {
            return RepoCache.Empty;
        }

        await using var stream = File.OpenRead(cachePath);
        var entries = await JsonSerializer.DeserializeAsync<Dictionary<string, ProjectCacheEntry>>(stream, JsonOptions, cancellationToken)
                      ?? new Dictionary<string, ProjectCacheEntry>(StringComparer.OrdinalIgnoreCase);

        return new RepoCache(entries);
    }

    public async Task SaveAsync(string cachePath, RepoCache cache, CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(cachePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var stream = File.Create(cachePath);
        await JsonSerializer.SerializeAsync(stream, cache.Entries, JsonOptions, cancellationToken);
    }
}
