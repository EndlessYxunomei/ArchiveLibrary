using ArchiveModels.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using ServiceLayer.Interfaces;
using System.Collections.ObjectModel;
using VMLayer.Navigation;

namespace VMLayer;

public class PersonListViewModel : ObservableObject, INavigationParameterReceiver, INavigatedTo
{
    //сервисы
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly IPersonService personService;

    //поля
    private PersonListDto? _selectedPerson;

    //свойства
    public PersonListDto? SelectedPerson
    {
        get => _selectedPerson;
        set
        {
            if (SetProperty(ref _selectedPerson, value))
            {
                DeleteCommand.NotifyCanExecuteChanged();
                EditCommand.NotifyCanExecuteChanged();
            }
        }
    }
    public ObservableCollection<PersonListDto> PersonList { get; set; } = [];

    //кнопки
    public IAsyncRelayCommand CreateCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public IAsyncRelayCommand EditCommand { get; }
    private bool CanEditDeletePerson() => SelectedPerson != null;
    private async Task CreatePerson() => await navigationService.GoToPersonDetails();
    private async Task EditPerson()
    {
        if (SelectedPerson != null)
        {
            await navigationService.GoToPersonDetails(SelectedPerson.Id);
        }
    }
    private async Task DeletePerson()
    {
        if (SelectedPerson != null)
        {
            var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить пользователя {SelectedPerson.FullName}?");
            if (result)
            {
                //Удаление оригинала
                var delResult = await personService.DeletePerson(SelectedPerson.Id);

                if (delResult.IsSuccess)
                {
                    //обновление списка
                    PersonList.Remove(SelectedPerson);
                    SelectedPerson = null;

                    await dialogService.Notify("Удалено", "Пользователь удалён");
                }
                else
                {
                    await dialogService.Notify("Ошибка удаления", delResult.ErrorCode);
                }
            }
        }
    }

    //конструктор
    public PersonListViewModel(IPersonService personService, IDialogService dialogService, INavigationService navigationService)
    {
        this.personService = personService;
        this.dialogService = dialogService;
        this.navigationService = navigationService;

        CreateCommand = new AsyncRelayCommand(CreatePerson);
        EditCommand = new AsyncRelayCommand(EditPerson, CanEditDeletePerson);
        DeleteCommand = new AsyncRelayCommand(DeletePerson, CanEditDeletePerson);

        //LoadPersonListAsync();
    }

    //загрузка данных
    //private async Task LoadPersonListAsync()
    //{
    //    var perList = await personService.GetPersonListAsync();
    //    if (perList.IsSuccess)
    //    {
    //        perList.Data.ForEach(PersonList.Add);
    //    }
    //}

    //обработка навигации
    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        if (parameters.TryGetValue(NavParamConstants.PersonList, out object? per_list) && per_list is PersonListDto personListDto)
        {
            UtilityService.UpdateList(PersonList, personListDto);
        }
        return Task.CompletedTask;
    }

    public async Task OnNavigatedTo(NavigationType navigationType)
    {
        if (PersonList.Count == 0)
        {
            var perList = await personService.GetPersonListAsync();
            if (perList.IsSuccess)
            {
                perList.Data.ForEach(PersonList.Add);
            }
        }
    }
}