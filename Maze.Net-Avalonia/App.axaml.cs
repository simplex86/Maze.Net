using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Maze.Avalonia.Views;

namespace Maze.Avalonia;

public class App : Application
{
    public override void Initialize()
    {
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
