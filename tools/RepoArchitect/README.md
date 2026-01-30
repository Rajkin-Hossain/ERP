# RepoArchitect

RepoArchitect is a lightweight desktop-style tool that scans a repository for `.csproj` files and renders an interactive dependency graph, dashboards, and architecture checks.

## F5 / Startup Experience
1. Open `tools/RepoArchitect/RepoArchitect.sln` in Visual Studio.
2. Set **RepoArchitect.App** as the Startup Project.
3. Press **F5**.
4. The backend starts on a free local port and a desktop window opens automatically.

## CLI Usage
Run the app directly with:

```bash
dotnet run --project tools/RepoArchitect/RepoArchitect.App -- [options]
```

Options:
- `--root <path>`: repository root (auto-detected if omitted)
- `--port <port>`: fixed port for the backend server
- `--no-window`: run backend only (no desktop window)
- `--open`: open the UI in the default browser
- `--export <path>`: export the graph as JSON or DOT (based on extension)
- `--no-ai`: disable optional AI insights

Examples:
```bash
# Run with desktop window (default)
dotnet run --project tools/RepoArchitect/RepoArchitect.App

# Run backend-only on a fixed port
 dotnet run --project tools/RepoArchitect/RepoArchitect.App -- --no-window --port 5050

# Export graph as JSON
 dotnet run --project tools/RepoArchitect/RepoArchitect.App -- --export artifacts/graph.json

# Export graph as DOT
 dotnet run --project tools/RepoArchitect/RepoArchitect.App -- --export artifacts/graph.dot
```

## Architecture Rules
Rules live in `tools/RepoArchitect/config/architecture-rules.json`.

```json
{
  "rules": [
    {
      "name": "UI-Does-Not-Reference-Domain",
      "from": "*UI*",
      "to": "*Domain*",
      "reason": "UI layer should depend on Application, not Domain directly."
    }
  ]
}
```

- `from` and `to` are glob patterns matched against project name or relative path.
- Violations show in the **Architecture** tab.

## Exporting the Graph
- Use **Exports** tab buttons to download JSON or DOT.
- Or use the CLI `--export` flag for automation.

## Performance Notes
- Parsed projects are cached in `.repoarchitect/cache.json` under the repo root.
- Files are re-parsed only when the `.csproj` timestamp changes.
