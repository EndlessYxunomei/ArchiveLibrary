

namespace VMLayer.Navigation;

public interface INavigationService
{
    //общая переадресация
    Task GoBack();
    Task GoBackAndReturn(Dictionary<string, object> parameters);

    //навигация на конкретные старницы
    Task GoToOriginalDetails(int id = 0);
    Task GoToOriginalList();
    Task GoToDocumentDetails(int id = 0);
    Task GoToDocumentList();
    Task GoToCompanyList();
    Task GoToCompanyDetails(int id = 0);

}
