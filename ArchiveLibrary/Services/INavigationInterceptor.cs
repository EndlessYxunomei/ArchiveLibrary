using VMLayer.Navigation;

namespace ArchiveLibrary.Services;

public interface INavigationInterceptor
{
    Task OnNavigatedTo(object bindingContext, NavigationType navigationType);
}
