using ArchiveModels.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using ServiceLayer.Interfaces;
using System.Collections.ObjectModel;
using VMLayer.Navigation;

namespace VMLayer;

public class CompanyListViewModel : ObservableObject, INavigationParameterReceiver
{
    //серивсы
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ICompanyService companyService;

    //Приватные поля
    private CompanyDto? _selectedCompany;

    //Свойства
    public CompanyDto? SelectedCompany
    {
        get => _selectedCompany;
        set
        {
            if (SetProperty(ref _selectedCompany, value))
            {
                DeleteCommand.NotifyCanExecuteChanged();
                EditCommand.NotifyCanExecuteChanged();
            }
        }
    }
    public ObservableCollection<CompanyDto> CompanyList { get; set; } = [];

    //Кнопки
    public IAsyncRelayCommand CreateCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public IAsyncRelayCommand EditCommand { get; }
    private async Task CreateCompany() => await navigationService.GoToCompanyDetails();
    private async Task EditCompany()
    {
        if (SelectedCompany != null)
        {
            await navigationService.GoToCompanyDetails(SelectedCompany.Id);
        }
    }
    private async Task DeleteCompany()
    {
        if (SelectedCompany != null)
        {
            var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить {SelectedCompany.Name}?");
            if (result)
            {
                //Удаление оригинала
                var delResult = await companyService.DeleteCompany(SelectedCompany.Id);
                if (delResult.IsSuccess)
                {
                    //обновление списка
                    CompanyList.Remove(SelectedCompany);
                    SelectedCompany = null;

                    await dialogService.Notify("Удалено", "Компания удалена");
                }
                else
                {
                    await dialogService.Notify("Ошибка удаления", delResult.ErrorCode);
                }

            }
        }
    }
    private bool CanEditDeleteCompany() => SelectedCompany != null;

    //конструктор
    public CompanyListViewModel(INavigationService navigationService, IDialogService dialogService, ICompanyService companyService)
    {
        this.navigationService = navigationService;
        this.dialogService = dialogService;
        this.companyService = companyService;

        CreateCommand = new AsyncRelayCommand(CreateCompany);
        DeleteCommand = new AsyncRelayCommand(DeleteCompany, CanEditDeleteCompany);
        EditCommand = new AsyncRelayCommand(EditCompany, CanEditDeleteCompany);

        LoadCompanyList();
    }

    //загрузка данных
    private async Task LoadCompanyList()
    {
        var company_list = await companyService.GetCompanyListAsync();
        if (company_list.IsSuccess)
        {
            company_list.Data.ForEach(CompanyList.Add);
        }
    }

    //навигация
    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        if (parameters.TryGetValue(NavParamConstants.CompanyList, out object? comp_list) && comp_list is CompanyDto companyListDto)
        {
            UtilityService.UpdateList(CompanyList, companyListDto);
        }
        return Task.CompletedTask;
    }
}
