using ArchiveModels.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using ServiceLayer.Interfaces;
using System.Collections.ObjectModel;
using VMLayer.Navigation;

namespace VMLayer;

public class OriginalListViewModel : ObservableObject, INavigationParameterReceiver, INavigatedTo
{
    //Сервисы
    private readonly IOriginalService originalService;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;

    //Приватные поля
    private OriginalListDto? _selectedOriginal;

    //Свойства
    public OriginalListDto? SelectedOriginal
    {
        get => _selectedOriginal;
        set
        {
            if (SetProperty(ref _selectedOriginal, value))
            {
                DeleteCommand.NotifyCanExecuteChanged();
                EditCommand.NotifyCanExecuteChanged();
            }
        }
    }
    public ObservableCollection<OriginalListDto> OriginalsList { get; set; } = [];

    //Кнопки
    public IAsyncRelayCommand CreateCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public IAsyncRelayCommand EditCommand { get; }
    private async Task CreateOriginal() => await navigationService.GoToOriginalDetails();
    private async Task DeleteOriginal()
    {
        if (SelectedOriginal != null)
        {
            var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить {SelectedOriginal!.OriginalName} {SelectedOriginal.OriginalCaption}?");
            if (result)
            {
                //Удаление оригинала
                var delResult = await originalService.DeleteOriginal(SelectedOriginal.Id);
                if (delResult.IsSuccess)
                {
                    //обновление списка
                    OriginalsList.Remove(SelectedOriginal);
                    SelectedOriginal = null;

                    await dialogService.Notify("Удалено", "Документ удалён");
                }
                else
                {
                    await dialogService.Notify("Ошибка удаления", delResult.ErrorCode);
                }

            }
        }
    }
    private async Task EditOriginal()
    {
        if (SelectedOriginal != null)
        {
            await navigationService.GoToOriginalDetails(SelectedOriginal.Id);
        }
    }
    private bool CanEditDeleteOriginal() => SelectedOriginal != null;

    //конструктор
    public OriginalListViewModel(INavigationService navigation, IDialogService dialog, IOriginalService original)
    {
        originalService = original;
        navigationService = navigation;
        dialogService = dialog;

        CreateCommand = new AsyncRelayCommand(CreateOriginal);
        DeleteCommand = new AsyncRelayCommand(DeleteOriginal, CanEditDeleteOriginal);
        EditCommand = new AsyncRelayCommand(EditOriginal, CanEditDeleteOriginal);

        //Загружаем перовначальный список (УБРАТБ КОГДА СДЕЛАЕМ НОРМАЛЬНО)
        //LoadOriginalList();
    }

    //private async Task LoadOriginalList()
    //{
    //    var originallist = await originalService.GetOriginalListAsync();
    //    if (originallist.IsSuccess)
    //    {
    //        originallist.Data.ForEach(OriginalsList.Add);
    //    }
    //}

    //обработка навигации
    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        if (parameters.TryGetValue(NavParamConstants.OriginalList, out object? orig_list) && orig_list is OriginalListDto originalListDto)
        {
            UtilityService.UpdateList(OriginalsList, originalListDto);
        }
        return Task.CompletedTask;
    }

    public async Task OnNavigatedTo(NavigationType navigationType)
    {
        if (OriginalsList.Count == 0)
        {
            var originallist = await originalService.GetOriginalListAsync();
            if (originallist.IsSuccess)
            {
                originallist.Data.ForEach(OriginalsList.Add);
            }
        }
    }
}
