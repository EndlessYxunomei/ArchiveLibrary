using ArchiveModels;
using ArchiveModels.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using ServiceLayer.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMLayer.Navigation;
using VMLayer.Validation;

namespace VMLayer;

public class OriginalDetailViewModel: BaseDetailViewModel
{
    //Сервисы
    private readonly IOriginalService originalService;
    private readonly IPersonService personService;
    private readonly ICompanyService companyService;
    private readonly IDocumentService documentService;
    public ValidationErrorExposer ErrorExposer { get; }

    //Приватные поля
    private int id;
    private int oldInventoryNumber;
    private int _inventoryNumber;
    private string _name = string.Empty;
    private string _caption = string.Empty;
    private string? _pageFormat;
    private string? _notes;
    private int _pageCount = 1;
    private CompanyDto? _company;
    private DocumentListDto? _document;
    private PersonListDto? _person;

    //Свойства
    [Required]
    [Range(1, int.MaxValue)]
    public int InventoryNumber
    {
        get => _inventoryNumber;
        set => SetProperty(ref _inventoryNumber, value, true);
    }
    [Required]
    [MaxLength(ArchiveConstants.MAX_NAME_LENGTH)]
    [MinLength(1)]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value, true);
    }
    [Required]
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_CAPTION_LENGTH)]
    [MinLength(1)]
    public string Caption
    {
        get => _caption;
        set => SetProperty(ref _caption, value, true);
    }
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_PAGES_FORMAT_LENGTH)]
    public string? PageFormat
    {
        get => _pageFormat;
        set => SetProperty(ref _pageFormat, value, true);
    }
    [Range(1, int.MaxValue)]
    public int PageCount
    {
        get => _pageCount;
        set => SetProperty(ref _pageCount, value, true);
    }
    public CompanyDto? Company
    {
        get => _company;
        set => SetProperty(ref _company, value);
    }
    public DocumentListDto? Document
    {
        get => _document;
        set => SetProperty(ref _document, value);
    }
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_NOTES_LENGTH)]
    public string? Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value, true);
    }
    public PersonListDto? Person
    {
        get => _person;
        set => SetProperty(ref _person, value);
    }

    //Списки для выбора
    public ObservableCollection<PersonListDto> PersonList { get; set; } = [];
    public ObservableCollection<DocumentListDto> DocumentList { get; set; } = [];
    public ObservableCollection<CompanyDto> CompanyList { get; set; } = [];

    //Кнопки добавления в списки
    public IAsyncRelayCommand AddDocumentCommand { get; }
    public IAsyncRelayCommand AddCompanyCommand { get; }
    public IAsyncRelayCommand AddPersonCommand { get; }

    private async Task AddDocument()
    {
        //await navigationService.GoToCreateDocument();
        await Task.Delay(10);//ЗАглушка
    }
    private async Task AddCompany()
    {
        //var result = await dialogService.ShowCompanyDetailPopup();
        //if (result != null && result is CompanyDto company)
        //{
        //    var newid = await companyService.UpsertCompany(company);
        //    var newDto = await companyService.GetCompanyAsync(newid);
        //    UtilityService.UpdateList(Companylist, newDto);
        //}
        await Task.Delay(10);//ЗАглушка
    }
    private async Task AddPerson()
    {
        await Task.Delay(10);//ЗАглушка
    }

    //Кнопки Сохранить и отмена
    private protected override  async Task SaveChanges()
    {
        ValidateAllProperties();
        var isValidOriginalNumber = await originalService.CheckInventoryNumber(InventoryNumber);

        if(!HasErrors &&
            ((id == 0 && isValidOriginalNumber.IsSuccess)
            || ( id != 0 && ( oldInventoryNumber == InventoryNumber || isValidOriginalNumber.IsSuccess) )))
        {
            OriginalDetailDto detailDto = new()
            {
                Id = id,
                InventoryNumber = InventoryNumber,
                Name = Name,
                Caption = Caption,
                PageFormat = PageFormat,
                PageCount = PageCount,
                Company = Company,
                Document = Document,
                Person = Person,
                Notes = Notes
                //необходимо добавить применимость и обновить её в сервисе
            };

            var res = await originalService.UpsertOriginal(detailDto);
            if (res.IsSuccess)
            {
                ErrorsChanged -= ViewModel_ErrorsChanged;
                await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.OriginalList, res.Data } });
            }
            else
            {
                await dialogService.Notify("Ошибка", res.ErrorCode);
            }
        }
        else
        {
            await dialogService.Notify("Ошибка", "Инвентарный номер занят" );
        }
    }

    //консруктор
    public OriginalDetailViewModel(INavigationService navigationService, IDialogService dialogService,
        IOriginalService originalService, IPersonService personService, ICompanyService companyService, IDocumentService documentService): base(navigationService, dialogService)
    {
        this.originalService = originalService;
        this.personService = personService;
        this.companyService = companyService;
        this.documentService = documentService;

        AddCompanyCommand = new AsyncRelayCommand(AddCompany);
        AddDocumentCommand = new AsyncRelayCommand(AddDocument);
        AddPersonCommand = new AsyncRelayCommand(AddPerson);

        ErrorExposer = new(this);

    }

    //Обработка навигации на страницу
    public override async Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        //НАполение списков документов, компаний и пользователей
        if (PersonList.Count == 0)
        {
            var newPersonList = await personService.GetPersonListAsync();
            if (newPersonList.IsSuccess)
            {
                newPersonList.Data.ForEach(PersonList.Add);
            }
        }
        if (DocumentList.Count == 0)
        {
            var newDocumentList = await documentService.GetDocumentListAsync(DocumentType.AddOriginal);
            if (newDocumentList.IsSuccess)
            {
                newDocumentList.Data.ForEach(DocumentList.Add);
            }
        }
        if (CompanyList.Count == 0)
        {
            var newCompanyList = await companyService.GetCompanyListAsync();
            if (newCompanyList.IsSuccess)
            {
                newCompanyList.Data.ForEach(CompanyList.Add);
            }
        }
        /**var newDocumentList = await documentService.GetDocumentListAsync(DocumentType.AddOriginal);
        var newCompanyList = await companyService.GetCompanyListAsync();
        var newPersonList = await personService.GetPersonListAsync();
        if (newPersonList.IsSuccess)
        {
            newPersonList.Data.ForEach(PersonList.Add);
        }
        if (newCompanyList.IsSuccess)
        {
            newCompanyList.Data.ForEach(CompanyList.Add);
        }
        if (newDocumentList.IsSuccess)
        {
            newDocumentList.Data.ForEach(DocumentList.Add);
        }**/


        if (parameters.TryGetValue(NavParamConstants.OriginalDetail, out object? value_orig) && value_orig is int origId)
        {
            if (origId == 0)
            {
                var newInventoryNumber = await originalService.GetLastInventoryNumber();
                if (newInventoryNumber.IsSuccess)
                {
                    InventoryNumber = newInventoryNumber.Data + 1;
                }
                else
                {
                    await dialogService.Notify("Ошибка", newInventoryNumber.ErrorCode);
                    await CancelChanges();
                }
                return;
            }
            id = origId;
            var orig_res = await originalService.GetOriginalDetailAsync(origId);
            if (orig_res.IsSuccess)
            {
                InventoryNumber = orig_res.Data.InventoryNumber;
                oldInventoryNumber = orig_res.Data.InventoryNumber;
                Name = orig_res.Data.Name;
                Caption = orig_res.Data.Caption;
                Notes = orig_res.Data.Notes;
                PageFormat = orig_res.Data.PageFormat;
                PageCount = orig_res.Data.PageCount;

                if (orig_res.Data.Company != null) Company = CompanyList.FirstOrDefault(x => x.Id == orig_res.Data.Company.Id);
                if (orig_res.Data.Document != null) Document = DocumentList.FirstOrDefault(x => x.Id == orig_res.Data.Document.Id);
                if (orig_res.Data.Person != null) Person = PersonList.FirstOrDefault(x => x.Id == orig_res.Data.Person.Id);
            }
            else
            {
                await dialogService.Notify("Ошибка", orig_res.ErrorCode);
            }
        }
        if (parameters.TryGetValue(NavParamConstants.DocumentList, out object? value_doc) && value_doc is DocumentListDto document)
        {
            UtilityService.UpdateList(DocumentList, document);
        }
        if (parameters.TryGetValue(NavParamConstants.CompanyList, out object? value_comp) && value_comp is CompanyDto company)
        {
            UtilityService.UpdateList(CompanyList, company);
        }
        if (parameters.TryGetValue(NavParamConstants.PersonList, out object? value_per) && value_per is PersonListDto person)
        {
            UtilityService.UpdateList(PersonList, person);
        }
        //return Task.CompletedTask;
    }
}
