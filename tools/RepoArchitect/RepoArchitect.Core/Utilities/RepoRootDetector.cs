namespace RepoArchitect.Core.Utilities;

public static class RepoRootDetector
{
    public static string FindRepoRoot(string startPath)
    {
        var current = new DirectoryInfo(startPath);
        if (File.Exists(startPath))
        {
            current = new FileInfo(startPath).Directory ?? current;
        }

        while (current is not null)
        {
            if (current.EnumerateDirectories(".git").Any())
            {
                return current.FullName;
            }

            if (current.EnumerateFiles("*.sln").Any() || current.EnumerateFiles("*.slnx").Any())
            {
                return current.FullName;
            }

            var csprojCount = current.EnumerateFiles("*.csproj").Take(2).Count();
            if (csprojCount > 1)
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}
