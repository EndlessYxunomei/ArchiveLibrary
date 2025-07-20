using ArchiveModels;
using ArchiveModels.DTO;
using ServiceLayer.Interfaces;
using System.ComponentModel.DataAnnotations;
using VMLayer.Navigation;
using VMLayer.Validation;

namespace VMLayer;

public class PersonDetailViewModel : BaseDetailViewModel
{
    //сервисы
    private readonly IPersonService personService;

    //поля
    private string? _firstName;
    private string? oldFirstName;
    private string _lastName = string.Empty;
    private string oldLastName = string.Empty;
    private string? _department;
    private int id;

    //свойства
    [StringLength(ArchiveConstants.MAX_PERSON_NAME_LENGTH)]
    public string? FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value, true);
    }
    [Required]
    [StringLength(ArchiveConstants.MAX_PERSON_NAME_LENGTH)]
    [MinLength(1)]
    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value, true);
    }
    [StringLength(ArchiveConstants.MAX_PERSON_DEPARTMENT_LENGTH)]
    public string? Department
    {
        get => _department;
        set => SetProperty(ref _department, value, true);
    }

    public ValidationErrorExposer ErrorExposer { get; }

    //кнопка сохранить
    private protected override async Task SaveChanges()
    {
        if (HasErrors) { return; }
        var PersonIsNotExists = await personService.CheckPersonFullName(LastName, FirstName);
        
        if ((id == 0 && PersonIsNotExists.IsSuccess)
            || (id != 0 && ((LastName == oldLastName && FirstName == oldFirstName) || PersonIsNotExists.IsSuccess)))
        {
            PersonDetailDto personDto = new()
            {
                Id = id,
                LastName = LastName,
                FirstName = FirstName,
                Department = Department
            };
            var res = await personService.UpsertPerson(personDto);
            if (res.IsSuccess)
            {
                ErrorsChanged -= ViewModel_ErrorsChanged;
                await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.PersonList, res.Data } });
            }
            else
            {
                await dialogService.Notify("Ошибка", res.ErrorCode);
            }
        }
        else
        {
            await dialogService.Notify("Ошибка", "Пользователь с таким именем уже существует");
        }
    }

    //конструктор
    public PersonDetailViewModel(INavigationService navigationService, IDialogService dialogService, IPersonService personService) : base(navigationService, dialogService)
    {
        this.personService = personService;

        ErrorExposer = new(this);
    }

    //навигация
    public override async Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        if (parameters.TryGetValue(NavParamConstants.PersonDetail, out object? per_det) && per_det is int perId)
        {
            if (perId == 0)
            {
                return;
            }

            id = perId;
            var per_res = await personService.GetPersonDetailAsync(perId);
            if (per_res.IsSuccess)
            {
                LastName = per_res.Data.LastName;
                FirstName = per_res.Data.FirstName;
                oldLastName = per_res.Data.LastName;
                oldFirstName = per_res.Data.FirstName;
                Department = per_res.Data.Department;
            }
            else
            {
                await dialogService.Notify("Ошибка", per_res.ErrorCode);
            }
        }
    }
}
