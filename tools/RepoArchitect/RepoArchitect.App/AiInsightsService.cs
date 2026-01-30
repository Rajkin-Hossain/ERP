using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RepoArchitect.Core.Models;

namespace RepoArchitect.App;

public sealed class AiInsightsService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    public bool Enabled { get; }

    public AiInsightsService(HttpClient httpClient, bool enabled)
    {
        _httpClient = httpClient;
        Enabled = enabled;
    }

    public async Task<string> GenerateInsightsAsync(string apiKey, string model, RepositorySnapshot snapshot, CancellationToken cancellationToken)
    {
        if (!Enabled)
        {
            return "AI insights are disabled. Remove --no-ai to enable.";
        }

        var summary = BuildSummary(snapshot);
        var payload = new
        {
            model,
            messages = new[]
            {
                new { role = "system", content = "You are an architecture assistant. Provide concise recommendations." },
                new { role = "user", content = summary }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(payload, JsonOptions), Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return $"AI request failed: {response.StatusCode} {body}";
        }

        var parsed = JsonSerializer.Deserialize<OpenAiResponse>(body, JsonOptions);
        return parsed?.Choices.FirstOrDefault()?.Message.Content ?? "No insights returned.";
    }

    public static string BuildSummary(RepositorySnapshot snapshot)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Repository architecture summary:");
        builder.AppendLine($"Projects: {snapshot.Projects.Count}");
        builder.AppendLine($"Dependencies: {snapshot.Graph.Edges.Count}");
        builder.AppendLine($"Cycles detected: {snapshot.Cycles.Count}");
        builder.AppendLine($"Violations: {snapshot.Violations.Count}");
        builder.AppendLine();
        builder.AppendLine("Top dependencies per project:");

        foreach (var project in snapshot.Projects.OrderBy(p => p.Name))
        {
            builder.AppendLine($"- {project.Name}: {project.ProjectReferences.Count} project refs, {project.PackageReferences.Count} packages");
        }

        return builder.ToString();
    }

    private sealed record OpenAiResponse(IReadOnlyList<Choice> Choices)
    {
        public sealed record Choice(Message Message);
    }

    private sealed record Message(string Content);
}
