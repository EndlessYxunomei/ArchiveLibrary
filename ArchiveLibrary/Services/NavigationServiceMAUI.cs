using VMLayer.Navigation;

namespace ArchiveLibrary.Services
{
    public class NavigationServiceMAUI: INavigationService
    {
        private static async Task Navigate(string pageName, Dictionary<string, object> parameters)
        {
            await Shell.Current.GoToAsync(pageName);
            if (Shell.Current.CurrentPage.BindingContext is INavigationParameterReceiver receiver)
            {
                await receiver.OnNavigatedTo(parameters);
            }
        }
        private static async Task Navigate(string pageName) => await Shell.Current.GoToAsync(pageName);

        public async Task GoBack() => await Shell.Current.GoToAsync("..");
        public async Task GoBackAndReturn(Dictionary<string, object> parameters)
        {
            await GoBack();
            if (Shell.Current.CurrentPage.BindingContext is INavigationParameterReceiver receiver)
            {
                await receiver.OnNavigatedTo(parameters);
            }
        }

        public Task GoToOriginalDetails(int id = 0) => Navigate(NavigationConstants.OriginalDetail, new() { { NavParamConstants.OriginalDetail, id } });
        public Task GoToOriginalList() => Navigate(NavigationConstants.OriginalList);
    }
}
