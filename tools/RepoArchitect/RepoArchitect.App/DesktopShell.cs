using System.Diagnostics;
using PhotinoNET;

namespace RepoArchitect.App;

public sealed class DesktopShell
{
    public void Launch(string url, CancellationToken cancellationToken)
    {
        var window = new PhotinoWindow("RepoArchitect")
            .SetUseOsDefaultLocation(false)
            .SetLocation(100, 100)
            .SetSize(1400, 900)
            .SetResizable(true)
            .SetContextMenuEnabled(false)
            .Center()
            .Load(url);

        window.WaitForClose();
    }

    public static void OpenBrowser(string url)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
}
