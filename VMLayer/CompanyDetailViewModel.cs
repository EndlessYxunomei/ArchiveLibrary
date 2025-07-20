using ArchiveModels;
using ArchiveModels.DTO;
using ServiceLayer.Interfaces;
using System.ComponentModel.DataAnnotations;
using VMLayer.Navigation;
using VMLayer.Validation;

namespace VMLayer;

public class CompanyDetailViewModel : BaseDetailViewModel
{
    //серивсы
    private readonly ICompanyService companyService;

    //приватные поля
    private int id;
    private string _name = string.Empty;
    private string oldName = string.Empty;
    private string? _description;

    //свойства
    [Required]
    [MaxLength(ArchiveConstants.MAX_NAME_LENGTH)]
    [MinLength(1)]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value, true);
    }
    [EmptyOrWithinRange(MinLength = 1, MaxLength = ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value, true);
    }
    public ValidationErrorExposer ErrorExposer { get; }

    //кнопка сохранить
    private protected override async Task SaveChanges()
    {
        if (HasErrors) { return; }
        var CompanyIsNotExists = await companyService.CheckCompany(Name);

        if ((id == 0 && CompanyIsNotExists.IsSuccess)
            || (id != 0 && ((Name == oldName) || CompanyIsNotExists.IsSuccess)))
        {
            CompanyDto companyDto = new()
            { 
                Id = id,
                Name = Name,
                Description = Description
            };
            var res = await companyService.UpsertCompany(companyDto);
            if (res.IsSuccess)
            {
                ErrorsChanged -= ViewModel_ErrorsChanged;
                await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.CompanyList, res.Data } });
            }
            else
            {
                await dialogService.Notify("Ошибка", res.ErrorCode);
            }
        }
        else
        {
            await dialogService.Notify("Ошибка", "Компния с таким именем уже существует");
        }
    }
    //конструктор
    public CompanyDetailViewModel(INavigationService navigationService, IDialogService dialogService, ICompanyService companyService) : base(navigationService, dialogService)
    {
        this.companyService = companyService;

        ErrorExposer = new(this);
    }

    //навигация
    public override async Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        if (parameters.TryGetValue(NavParamConstants.CompanyDetail, out object? comp_det) && comp_det is int copmId)
        {
            if (copmId == 0)
            {
                return;
            }

            id = copmId;
            var doc_res = await companyService.GetCompanyAsync(copmId);
            if (doc_res.IsSuccess)
            {
                Name = doc_res.Data.Name;
                Description = doc_res.Data.Description;
                oldName = doc_res.Data.Name;
            }
            else
            {
                await dialogService.Notify("Ошибка", doc_res.ErrorCode);
            }
        }
    }
}
