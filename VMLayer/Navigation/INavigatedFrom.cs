namespace VMLayer.Navigation;

public interface INavigatedFrom
{
    Task OnNavigatedFrom(NavigationType navigationType);
}
