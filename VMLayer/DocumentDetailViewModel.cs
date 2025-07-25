using ArchiveModels;
using ArchiveModels.DTO;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using ServiceLayer.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using VMLayer.Navigation;
using VMLayer.Validation;

namespace VMLayer;

public class DocumentDetailViewModel: BaseDetailViewModel
{
    //Сервисы
    private readonly IDocumentService documentService;
    private readonly ICompanyService companyService;

    //Приватные поля
    private int id;
    private string _name = string.Empty;
    private string? _description;
    private DateTime _date = DateTime.Today;
    private CompanyDto? _company;
    private DocumentType _documentType;
    private string oldName = string.Empty;
    private DateTime oldDate;

    //Свойства
    public ValidationErrorExposer ErrorExposer { get; }
    public IReadOnlyList<DocumentType> DocumentTypes { get; } = (IReadOnlyList<DocumentType>)Enum.GetValues(typeof(DocumentType));

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
    [Required]
    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value, true);
    }
    [Required]
    public CompanyDto? Company
    {
        get => _company;
        set => SetProperty(ref _company, value, true);
    }
    [Required]
    public DocumentType DocumentType
    {
        get => _documentType;
        set => SetProperty(ref _documentType, value, true);
    }

    //Списки выбора
    public ObservableCollection<CompanyDto> CompanyList { get; set; } = [];

    //Кнопки добавления в списки
    public IAsyncRelayCommand AddCompanyCommand { get; }
    private async Task AddCompany()
    {
        await navigationService.GoToCompanyDetails();
    }

    //Кнопка сохранить
    private protected override async Task SaveChanges()
    {
        if (HasErrors) { return; }
        var documentIsNotExists = await documentService.CheckDocument(Name, new(Date.Year, Date.Month, Date.Day));

        if ((id == 0 && documentIsNotExists.IsSuccess)
            || (id != 0 && ((Name == oldName && Date == oldDate) || documentIsNotExists.IsSuccess)))
        {
            DocumentDetailDto detailDto = new()
            {
                Name = Name,
                DocumentType = DocumentType,
                Description = Description,
                Date = Date,
                Company = Company,
                Id = id
            };
            var res = await documentService.UpsertDocument(detailDto);
            if (res.IsSuccess)
            {
                ErrorsChanged -= ViewModel_ErrorsChanged;
                await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.DocumentList, res.Data } });
            }
            else
            {
                await dialogService.Notify("Ошибка", res.ErrorCode);
            }
        }
        else
        {
            await dialogService.Notify("Ошибка", "Документ с таким номером и датой уже существует");
        }
    }

    //конструктор
    public DocumentDetailViewModel(INavigationService navigationService, IDialogService dialogService,
        IDocumentService documentService, ICompanyService companyService): base (navigationService, dialogService)
    {
        this.documentService = documentService;
        this.companyService = companyService;

        AddCompanyCommand = new AsyncRelayCommand(AddCompany);

        ErrorExposer = new(this);
    }

    //обработка навигации
    public override async Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        //наполнение списков
        if (CompanyList.Count == 0)
        {
            var newCompanyList = await companyService.GetCompanyListAsync();
            if (newCompanyList.IsSuccess)
            {
                newCompanyList.Data.ForEach(CompanyList.Add);
            }
        }

        if (parameters.TryGetValue(NavParamConstants.DocumentDetail, out object? doc_det) && doc_det is int docId)
        {
            if (docId == 0)
            {
                return;
            }
            
            id = docId;
            var doc_res = await documentService.GetDocumentDetailAsync(id);
            if (doc_res.IsSuccess)
            {
                Name = doc_res.Data.Name;
                Description = doc_res.Data.Description;
                DocumentType = doc_res.Data.DocumentType;
                Date = doc_res.Data.Date;
                if (doc_res.Data.Company != null) Company = CompanyList.FirstOrDefault(x => x.Id == doc_res.Data.Company.Id);

                oldDate = doc_res.Data.Date;
                oldName = doc_res.Data.Name;
            }
            else
            {
                await dialogService.Notify("Ошибка", doc_res.ErrorCode);
            }
        }
        if (parameters.TryGetValue(NavParamConstants.CompanyList, out object? value_comp) && value_comp is CompanyDto company)
        {
            UtilityService.UpdateList(CompanyList, company);
        }
    }
}
