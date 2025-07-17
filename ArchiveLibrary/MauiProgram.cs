using ArchiveDB;
using ArchiveLibrary.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceLayer;
using ServiceLayer.Interfaces;
using VMLayer;
using VMLayer.Navigation;

namespace ArchiveLibrary
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
                });

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //ГДЕ НАХОДИТСЯ НАША БАЗА
            //C:\Users\pestr\AppData\Local\Packages\com.companyname.archiveghost.client_9zz4h110yvjzm\LocalState

            builder.Services.AddDbContext<ArchiveDbContext>(op =>
            //op.UseSqlServer(builder.Configuration.GetConnectionString("ArchiveLibrarySQLServer")));
            op.UseSqlite(builder.Configuration.GetConnectionString("ArchiveLibrary")));

            //сервисы
            builder.Services.AddSingleton<IDialogService, DialogServiceMAUI>();
            builder.Services.AddSingleton<INavigationService, NavigationServiceMAUI>();
            builder.Services.AddTransient<IOriginalService, OriginalService>();
            builder.Services.AddTransient<IPersonService, PersonService>();
            builder.Services.AddTransient<IDocumentService, DocumentService>();
            builder.Services.AddTransient<ICompanyService, CompanyService>();

            //навигация
            Routing.RegisterRoute(NavigationConstants.OriginalList, typeof(OriginalListPage));
            builder.Services.AddTransient<OriginalListPage>();
            builder.Services.AddTransient<OriginalListViewModel>();
            //builder.Services.AddTransientWithShellRoute<OriginalListPage, OriginalListViewModel>(NavigationConstants.OriginalList);
            Routing.RegisterRoute(NavigationConstants.OriginalDetail, typeof(OriginalDetailPage));
            builder.Services.AddTransient<OriginalDetailPage>();
            builder.Services.AddTransient<OriginalDetailViewModel>();
            //Routing.RegisterRoute(NavigationConstants.DocumentDetail, typeof(DocumentDetailPage));
            //builder.Services.AddTransient<DocumentDetailPage>();
            builder.Services.AddTransient<DocumentDetailViewModel>();
            //Routing.RegisterRoute(NavigationConstants.DocumentList, typeof(DocumentListPage));
            //builder.Services.AddTransient<DocumentListPage>();
            builder.Services.AddTransient<DocumentListViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
