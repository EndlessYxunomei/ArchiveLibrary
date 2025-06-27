using VMLayer.Navigation;

namespace ArchiveLibrary
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
