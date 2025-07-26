using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using ServiceLayer.Interfaces;
using VMLayer.Navigation;
using VMLayer;
using NSubstitute.ReceivedExtensions;

namespace ArchiveTests.VMTests;

public class CompanyListVMTests
{
    [Fact]
    public async Task CreateCommandTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_list));

        var test_vm = new CompanyListViewModel(navigationService, dialogService, companyService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);

        //Act
        await test_vm.CreateCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToCompanyDetails();

    }
    [Fact]
    public async Task EditCommandTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_list));

        var test_vm = new CompanyListViewModel(navigationService, dialogService, companyService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);
        test_vm.SelectedCompany = test_vm.CompanyList[0];

        //Act
        await test_vm.EditCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToCompanyDetails(1);
    }
    [Fact]
    public async Task DeleteCommandTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_list));

        companyService.DeleteCompany(Arg.Any<int>()).Returns(Result<Nothing>.Success());
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var test_vm = new CompanyListViewModel(navigationService, dialogService, companyService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);
        test_vm.SelectedCompany = test_vm.CompanyList[0];

        //Act
        await test_vm.DeleteCommand.ExecuteAsync(null);

        //Assert
        await dialogService.Received().Notify("Удалено", "Компания удалена");
        //Assert.Equal(2, test_vm.OriginalsList.Count);
    }
    [Fact]
    public async Task NavigateParametrRecivedTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        var companyService = Substitute.For<ICompanyService>();
        List<CompanyDto> test_list =
        [
            new() { Id = 1, Name = "test1"},
            new() { Id = 2, Name = "test2", Description = "test_description"}
        ];
        CompanyDto test_dto = new() { Id = 4,Name = "test" };
        companyService.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_list));

        var test_vm = new CompanyListViewModel(navigationService, dialogService, companyService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);

        //Act
        Dictionary<string, object> nav_dic = new() { { NavParamConstants.CompanyList, test_dto } };
        await test_vm.OnNavigatedTo(nav_dic);

        //Assert
        Assert.Equal(3, test_vm.CompanyList.Count);
    }
}
