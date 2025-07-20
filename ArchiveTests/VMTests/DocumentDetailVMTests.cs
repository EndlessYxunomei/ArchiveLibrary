using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using ServiceLayer.Interfaces;
using VMLayer.Navigation;
using VMLayer;
using NSubstitute.ReceivedExtensions;
using ArchiveModels;

namespace ArchiveTests.VMTests;

public class DocumentDetailVMTests
{
    [Fact]
    public async Task CancelDocumentTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();

        var test_vm = new DocumentDetailViewModel(navigationService, dialogService, documentService, companyService);

        //Act
        await test_vm.CancelCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoBack();
    }
    [Fact]
    public async Task SaveDocumentTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        DocumentListDto test_dto = new() { Id = 1, DocumentType = DocumentType.AddCorrection, Name = "test" };
        documentService.CheckDocument(Arg.Any<string>(), default).ReturnsForAnyArgs(Result<Nothing>.Success());
        documentService.UpsertDocument(Arg.Any<DocumentDetailDto>()).ReturnsForAnyArgs(Result<DocumentListDto>.Success(test_dto));

        var test_vm = new DocumentDetailViewModel(navigationService, dialogService, documentService, companyService)
        {
            Name = "test1",
            Description = "test_d",
            Date = DateTime.Now,
            DocumentType = DocumentType.AddCorrection,
            Company = test_company_list[0]
            
        };

        //Act
        await test_vm.AcseptCommand.ExecuteAsync(null);

        //Assert
        await navigationService.ReceivedWithAnyArgs().GoBackAndReturn(Arg.Any<Dictionary<string, object>>());
    }

    [Fact]
    public async Task AddCompanyTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();

        var test_vm = new DocumentDetailViewModel(navigationService, dialogService, documentService, companyService);

        //Act
        await test_vm.AddCompanyCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToCompanyDetails(0);
    }

    [Fact]
    public async Task CreateDocumentNavigationTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();

        var test_vm = new DocumentDetailViewModel(navigationService, dialogService, documentService, companyService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.DocumentDetail, 0 } });

        //Assert
        Assert.Equal("", test_vm.Name);
        Assert.NotEmpty(test_vm.CompanyList);
    }
    [Fact]
    public async Task EditDocumentNavigationTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();
        DocumentDetailDto test_document = new()
        { 
            DocumentType = DocumentType.AddCorrection,
            Date = DateTime.Now,
            Description = "Test",
            Name = "Test_name",
            Id = 5
        };
        documentService.GetDocumentDetailAsync(5).Returns(Result<DocumentDetailDto>.Success(test_document));

        var test_vm = new DocumentDetailViewModel(navigationService, dialogService, documentService, companyService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.DocumentDetail, 5 } });

        //Assert
        Assert.NotEmpty(test_vm.CompanyList);
        Assert.Equal("Test_name", test_vm.Name);
    }
    [Fact]
    public async Task NavigationParametrsTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_company_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_company_list));

        var documentService = Substitute.For<IDocumentService>();


        var test_vm = new DocumentDetailViewModel(navigationService, dialogService, documentService, companyService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.CompanyList, new CompanyDto() { Id = 3, Name = "test_company" } } });

        //Assert
        Assert.Equal(3, test_vm.CompanyList.Count);
    }
}
