using ArchiveLibrary.Services;

namespace ArchiveLibrary;

public partial class App : Application
{
    public App(INavigationInterceptor interceptor)
    {
        InitializeComponent();

        MainPage = new AppShell(interceptor);
    }
}
