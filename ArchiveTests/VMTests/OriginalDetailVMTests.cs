using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using ServiceLayer.Interfaces;
using VMLayer.Navigation;
using VMLayer;
using NSubstitute.ReceivedExtensions;
using ArchiveModels;

namespace ArchiveTests.VMTests;

public class OriginalDetailVMTests
{
    [Fact]
    public async Task CancelOriginalTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var originalService = Substitute.For<IOriginalService>();
        List<OriginalListDto> test_original_list =
        [
            new(){ Id = 1, OriginalInventoryNumber = 1, OriginalName = "test1", OriginalCaption = "caption1" },
            new(){ Id = 2, OriginalInventoryNumber = 3, OriginalName = "test2", OriginalCaption = "caption2" },
            new(){ Id = 3, OriginalInventoryNumber = 5, OriginalName = "test3", OriginalCaption = "caption3" }
        ];
        originalService.GetOriginalListAsync().Returns(Result<List<OriginalListDto>>.Success(test_original_list));

        var personService = Substitute.For<IPersonService>();
        List<PersonListDto> test_person_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_person_list));

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        List<DocumentListDto> test_document_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = DocumentType.AddOriginal },
        ];
        documentService.GetDocumentListAsync(Arg.Any<DocumentType>()).Returns(Result<List<DocumentListDto>>.Success(test_document_list));

        var test_vm = new OriginalDetailViewModel(navigationService, dialogService, originalService, personService, companyService, documentService);

        //Act
        await test_vm.CancelCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoBack();
    }
    [Fact]
    public async Task SaveOriginalTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var originalService = Substitute.For<IOriginalService>();
        OriginalListDto test_original = new() { Id = 1, OriginalInventoryNumber = 1, OriginalName = "test1", OriginalCaption = "caption1" };
        originalService.UpsertOriginal(Arg.Any<OriginalDetailDto>()).Returns(Result<OriginalListDto>.Success(test_original));
        originalService.CheckInventoryNumber(Arg.Any<int>()).Returns(Result<Nothing>.Success());

        var personService = Substitute.For<IPersonService>();
        List<PersonListDto> test_person_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_person_list));

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        List<DocumentListDto> test_document_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = DocumentType.AddOriginal },
        ];
        documentService.GetDocumentListAsync(Arg.Any<DocumentType>()).Returns(Result<List<DocumentListDto>>.Success(test_document_list));

        var test_vm = new OriginalDetailViewModel(navigationService, dialogService, originalService, personService, companyService, documentService)
        {
            Name = "test_name",
            Caption = "test_caption",
            InventoryNumber = 10
        };

        //Act
        await test_vm.AcseptCommand.ExecuteAsync(null);

        //Assert
        await navigationService.ReceivedWithAnyArgs().GoBackAndReturn(Arg.Any<Dictionary<string,object>>());
    }

    [Fact]
    public async Task AddDocumentTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var originalService = Substitute.For<IOriginalService>();

        var personService = Substitute.For<IPersonService>();
        List<PersonListDto> test_person_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_person_list));

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        List<DocumentListDto> test_document_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = DocumentType.AddOriginal },
        ];
        documentService.GetDocumentListAsync(Arg.Any<DocumentType>()).Returns(Result<List<DocumentListDto>>.Success(test_document_list));

        var test_vm = new OriginalDetailViewModel(navigationService, dialogService, originalService, personService, companyService, documentService);

        //Act
        await test_vm.AddDocumentCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToDocumentDetails(0);

    }
    [Fact]
    public async Task AddCompanyTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var originalService = Substitute.For<IOriginalService>();

        var personService = Substitute.For<IPersonService>();
        List<PersonListDto> test_person_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_person_list));

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        List<DocumentListDto> test_document_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = DocumentType.AddOriginal },
        ];
        documentService.GetDocumentListAsync(Arg.Any<DocumentType>()).Returns(Result<List<DocumentListDto>>.Success(test_document_list));

        var test_vm = new OriginalDetailViewModel(navigationService, dialogService, originalService, personService, companyService, documentService);

        //Act
        await test_vm.AddCompanyCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToCompanyDetails(0);
    }
    //ещё не реализовано в модели
    [Fact]
    public async Task AddPersonTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var originalService = Substitute.For<IOriginalService>();

        var personService = Substitute.For<IPersonService>();
        List<PersonListDto> test_person_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_person_list));

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        List<DocumentListDto> test_document_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = DocumentType.AddOriginal },
        ];
        documentService.GetDocumentListAsync(Arg.Any<DocumentType>()).Returns(Result<List<DocumentListDto>>.Success(test_document_list));

        var test_vm = new OriginalDetailViewModel(navigationService, dialogService, originalService, personService, companyService, documentService);

        //Act
        await test_vm.AddPersonCommand.ExecuteAsync(null);

        //Assert
        //await navigationService.Received().GoToDocumentDetails(0);

    }
    
    [Fact]
    public async Task CreateOriginalNavigationTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var originalService = Substitute.For<IOriginalService>();
        originalService.GetLastInventoryNumber().Returns(Result<int>.Success(10));

        var personService = Substitute.For<IPersonService>();
        List<PersonListDto> test_person_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_person_list));

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        List<DocumentListDto> test_document_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = DocumentType.AddOriginal },
        ];
        documentService.GetDocumentListAsync(Arg.Any<DocumentType>()).Returns(Result<List<DocumentListDto>>.Success(test_document_list));

        var test_vm = new OriginalDetailViewModel(navigationService, dialogService, originalService, personService, companyService, documentService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.OriginalDetail, 0 } });

        //Assert
        Assert.Equal(11,test_vm.InventoryNumber);
        Assert.NotEmpty(test_vm.DocumentList);
        Assert.NotEmpty(test_vm.CompanyList);
        Assert.NotEmpty(test_vm.PersonList);
    }
    [Fact]
    public async Task EditOriginalNavigationTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var originalService = Substitute.For<IOriginalService>();
        OriginalDetailDto test_original = new()
        {
            Id = 10,
            InventoryNumber = 999,
            Name = "test_name",
            Caption = "test_caption",
            Document = new() { Id = 1, Name = "test1", DocumentType = DocumentType.AddOriginal },
            Company = new() { Id = 2, Name = "test2", Description = "test_description" },
            Person = new() { Id = 1, FullName = "test1" },
            PageCount = 10,
            PageFormat = "A4",
            Notes = "test_notes"
        };
        originalService.GetOriginalDetailAsync(10).Returns(Result<OriginalDetailDto>.Success(test_original));

        var personService = Substitute.For<IPersonService>();
        List<PersonListDto> test_person_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_person_list));

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        List<DocumentListDto> test_document_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = DocumentType.AddOriginal }
        ];
        documentService.GetDocumentListAsync(Arg.Any<DocumentType>()).Returns(Result<List<DocumentListDto>>.Success(test_document_list));

        var test_vm = new OriginalDetailViewModel(navigationService, dialogService, originalService, personService, companyService, documentService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.OriginalDetail, 10 } });

        //Assert
        Assert.Equal(999, test_vm.InventoryNumber);
        Assert.NotNull(test_vm.Document);
        Assert.NotNull(test_vm.Company);
        Assert.NotNull(test_vm.Person);
    }
    
    [Fact]
    public async Task NavigationParametrsTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var originalService = Substitute.For<IOriginalService>();

        var personService = Substitute.For<IPersonService>();
        List<PersonListDto> test_person_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_person_list));

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        List<DocumentListDto> test_document_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = DocumentType.AddOriginal },
        ];
        documentService.GetDocumentListAsync(Arg.Any<DocumentType>()).Returns(Result<List<DocumentListDto>>.Success(test_document_list));

        var test_vm = new OriginalDetailViewModel(navigationService, dialogService, originalService, personService, companyService, documentService);
        Dictionary<string, object> nav_params = new()
        {
            { NavParamConstants.DocumentList, new DocumentListDto() { Id = 3, Name = "test_document", DocumentType = DocumentType.AddOriginal } },
            { NavParamConstants.CompanyList, new CompanyDto() { Id = 3, Name = "test_company" } },
            { NavParamConstants.PersonList, new PersonListDto() { Id = 3, FullName = "test_person" } }
        };

        //Act
        await test_vm.OnNavigatedTo(nav_params);

        //Assert
        Assert.Equal(3, test_vm.DocumentList.Count);
        Assert.Equal(3, test_vm.CompanyList.Count);
        Assert.Equal(3, test_vm.PersonList.Count);
    }
}
