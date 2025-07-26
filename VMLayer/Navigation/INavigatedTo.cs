namespace VMLayer.Navigation;

public interface INavigatedTo
{
    Task OnNavigatedTo(NavigationType navigationType);
}
