namespace RepoArchitect.App;

public sealed record CommandLineOptions(
    string? Root,
    int? Port,
    bool NoWindow,
    bool OpenBrowser,
    string? ExportPath,
    bool NoAi)
{
    public static CommandLineOptions Parse(string[] args)
    {
        string? root = null;
        int? port = null;
        var noWindow = false;
        var openBrowser = false;
        string? exportPath = null;
        var noAi = false;

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "--root" when i + 1 < args.Length:
                    root = args[++i];
                    break;
                case "--port" when i + 1 < args.Length:
                    if (int.TryParse(args[++i], out var parsed))
                    {
                        port = parsed;
                    }
                    break;
                case "--no-window":
                    noWindow = true;
                    break;
                case "--open":
                    openBrowser = true;
                    break;
                case "--export" when i + 1 < args.Length:
                    exportPath = args[++i];
                    break;
                case "--no-ai":
                    noAi = true;
                    break;
            }
        }

        return new CommandLineOptions(root, port, noWindow, openBrowser, exportPath, noAi);
    }
}
