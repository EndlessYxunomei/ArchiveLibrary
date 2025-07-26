using ArchiveModels;
using ArchiveModels.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using ServiceLayer.Interfaces;
using System.Collections.ObjectModel;
using VMLayer.Navigation;

namespace VMLayer;

public class ApplicabilityListViewModel : ObservableObject
{
    //сервисы
    private readonly IDialogService dialogService;
    private readonly IApplicabilityService applicabilityService;

    //приватные поля
    private ApplicabilityDto? _selectedApplicability;

    //свойства
    public ApplicabilityDto? SelectedApplicability
    {
        get => _selectedApplicability;
        set
        {
            if (SetProperty(ref _selectedApplicability, value))
            {
                DeleteCommand.NotifyCanExecuteChanged();
                EditCommand.NotifyCanExecuteChanged();
            }
        }
    }

    //списки
    public ObservableCollection<ApplicabilityDto> ApplicabilityList { get; set; } = [];

    //кнопки
    public IAsyncRelayCommand CreateCommand { get; }
    public IAsyncRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }

    private bool CanEditDeleteApplicability() => SelectedApplicability != null;
    private async Task CreateApplicability()
    {
        var dialog_result = await dialogService.Ask("Создание применимости","Введите новую применимость", ArchiveConstants.MAX_DESCRIPTION_LENGTH);

        await UpdateApplicabilityList(dialog_result);
    }
    private async Task EditApplicability()
    {
        var dialog_result = await dialogService.Ask("Редактирование применимости", "Введите новую применимость",
            SelectedApplicability!.Description, ArchiveConstants.MAX_DESCRIPTION_LENGTH);

        await UpdateApplicabilityList(dialog_result, SelectedApplicability.Description);
    }
    private async Task UpdateApplicabilityList(string? result, string? oldResult = null)
    {
        if (string.IsNullOrEmpty(result) == false
            && (oldResult == null || (oldResult != null && result != oldResult)))
        {
            var isNotExists = await applicabilityService.CheckApplicability(result!);
            if (isNotExists.IsSuccess)
            {
                int dto_id = oldResult == null ? 0 : SelectedApplicability!.Id;
                ApplicabilityDto new_applicability = new()
                {
                    Description = result!,
                    Id = dto_id
                };
                var res = await applicabilityService.UpsertApplicability(new_applicability);
                if (res.IsSuccess)
                {
                    UtilityService.UpdateList(ApplicabilityList, res.Data);
                }
                else
                {
                    await dialogService.Notify(res.ErrorCode, res.ErrorData);
                }
            }
            else
            {
                await dialogService.Notify(isNotExists.ErrorCode, isNotExists.ErrorCode);
            }
        }
    }
    private async Task DeleteApplicability()
    {
        var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить применимость {SelectedApplicability!.Description}?");
        if (result)
        {
            var delRes = await applicabilityService.DeleteApplicability(SelectedApplicability!.Id);
            if (delRes.IsSuccess)
            {
                ApplicabilityList.Remove(SelectedApplicability);
                SelectedApplicability = null;

                await dialogService.Notify("Удалено", "Применимость удалена");
            }
            else
            {
                await dialogService.Notify("Ошибка удаления", delRes.ErrorCode);
            }
        }
    }

    //конструктор
    public ApplicabilityListViewModel(IDialogService dialogService, IApplicabilityService applicabilityService)
    {
        this.dialogService = dialogService;
        this.applicabilityService = applicabilityService;

        CreateCommand = new AsyncRelayCommand(CreateApplicability);
        DeleteCommand = new AsyncRelayCommand(DeleteApplicability, CanEditDeleteApplicability);
        EditCommand = new AsyncRelayCommand(EditApplicability, CanEditDeleteApplicability);

        LoadApplicabilities().Wait();
    }

    //загрузка данных
    private async Task LoadApplicabilities()
    {
        var list = await applicabilityService.GetApplicabilityListAsync();
        if (list.IsSuccess)
        {
            list.Data.ForEach(ApplicabilityList.Add);
        }
    }
}
