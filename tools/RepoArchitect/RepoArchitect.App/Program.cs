using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.FileProviders;
using RepoArchitect.App;
using RepoArchitect.Core.Abstractions;
using RepoArchitect.Core.Models;
using RepoArchitect.Core.Services;
using RepoArchitect.UI;

var options = CommandLineOptions.Parse(args);
var root = options.Root ?? Directory.GetCurrentDirectory();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProjectParser, CsprojParser>();
builder.Services.AddSingleton<IGraphBuilder, GraphBuilder>();
builder.Services.AddSingleton<IRuleEvaluator, RuleEvaluator>();
builder.Services.AddSingleton<IRepoScanner, RepoScanner>();
builder.Services.AddSingleton<IExportService, ExportService>();
builder.Services.AddSingleton<CacheStore>();
var rulesPath = Path.Combine(root, "tools", "RepoArchitect", "config", "architecture-rules.json");
builder.Services.AddSingleton(new RepoScanOptions(root, rulesPath, EnableCache: true, EnableAi: !options.NoAi));
builder.Services.AddSingleton<RepoArchitectState>();
builder.Services.AddSingleton(new DesktopShell());
builder.Services.AddHttpClient();
builder.Services.AddSingleton(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var client = clientFactory.CreateClient();
    client.Timeout = TimeSpan.FromSeconds(30);
    return new AiInsightsService(client, !options.NoAi);
});

var port = options.Port;
if (port.HasValue)
{
    builder.WebHost.UseUrls($"http://127.0.0.1:{port.Value}");
}
else
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.Listen(IPAddress.Loopback, 0, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        });
    });
}

var app = builder.Build();

var embeddedProvider = new ManifestEmbeddedFileProvider(typeof(UiAssets).Assembly, "wwwroot");
app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = embeddedProvider });
app.UseStaticFiles(new StaticFileOptions { FileProvider = embeddedProvider });

app.MapGet("/api/graph", async (RepoArchitectState state, CancellationToken ct) =>
{
    var snapshot = await state.GetSnapshotAsync(ct);
    return Results.Json(snapshot);
});

app.MapGet("/api/refresh", async (RepoArchitectState state, CancellationToken ct) =>
{
    var snapshot = await state.RefreshAsync(ct);
    return Results.Json(snapshot);
});

app.MapGet("/api/export/json", async (RepoArchitectState state, IExportService exporter, CancellationToken ct) =>
{
    var snapshot = await state.GetSnapshotAsync(ct);
    var json = exporter.ExportGraphJson(snapshot);
    return Results.Text(json, "application/json");
});

app.MapGet("/api/export/dot", async (RepoArchitectState state, IExportService exporter, CancellationToken ct) =>
{
    var snapshot = await state.GetSnapshotAsync(ct);
    var dot = exporter.ExportGraphDot(snapshot);
    return Results.Text(dot, "text/plain");
});

app.MapPost("/api/ai/insights", async (
    [FromBody] AiRequest request,
    RepoArchitectState state,
    AiInsightsService aiService,
    CancellationToken ct) =>
{
    if (!aiService.Enabled)
    {
        return Results.StatusCode(StatusCodes.Status403Forbidden);
    }

    if (string.IsNullOrWhiteSpace(request.ApiKey))
    {
        return Results.BadRequest("API key is required.");
    }

    var snapshot = await state.GetSnapshotAsync(ct);
    var message = await aiService.GenerateInsightsAsync(request.ApiKey, request.Model ?? "gpt-4o-mini", snapshot, ct);
    return Results.Json(new { message });
});

app.MapFallbackToFile("index.html", new StaticFileOptions { FileProvider = embeddedProvider });

await app.StartAsync();

var addressFeature = app.Services.GetRequiredService<IServerAddressesFeature>();
var serverUrl = addressFeature.Addresses.FirstOrDefault() ?? "http://127.0.0.1:5000";

var stateService = app.Services.GetRequiredService<RepoArchitectState>();
await stateService.GetSnapshotAsync();

if (!string.IsNullOrWhiteSpace(options.ExportPath))
{
    var exporter = app.Services.GetRequiredService<IExportService>();
    var snapshot = await stateService.GetSnapshotAsync();
    var exportPath = Path.GetFullPath(options.ExportPath);
    Directory.CreateDirectory(Path.GetDirectoryName(exportPath) ?? ".");
    var content = exportPath.EndsWith(".dot", StringComparison.OrdinalIgnoreCase)
        ? exporter.ExportGraphDot(snapshot)
        : exporter.ExportGraphJson(snapshot);
    await File.WriteAllTextAsync(exportPath, content);
}

if (options.OpenBrowser)
{
    DesktopShell.OpenBrowser(serverUrl);
}

if (!options.NoWindow)
{
    try
    {
        var shell = app.Services.GetRequiredService<DesktopShell>();
        shell.Launch(serverUrl, CancellationToken.None);
    }
    catch
    {
        DesktopShell.OpenBrowser(serverUrl);
        await app.WaitForShutdownAsync();
    }
}
else
{
    await app.WaitForShutdownAsync();
}

await app.StopAsync();

public sealed record AiRequest(string ApiKey, string? Model);
