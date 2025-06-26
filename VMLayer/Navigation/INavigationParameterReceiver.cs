
namespace VMLayer.Navigation;

public interface INavigationParameterReceiver
{
    Task OnNavigatedTo(Dictionary<string, object> parameters);
}
