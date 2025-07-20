using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using ServiceLayer.Interfaces;
using VMLayer.Navigation;
using VMLayer;
using NSubstitute.ReceivedExtensions;

namespace ArchiveTests.VMTests;

public class CompanyDetailVMTests
{
    [Fact]
    public async Task CancelCompanyTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();

        var test_vm = new CompanyDetailViewModel(navigationService, dialogService, companyService);

        //Act
        await test_vm.CancelCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoBack();
    }
    [Fact]
    public async Task SaveCompanyTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();
        companyService.CheckCompany(Arg.Any<string>()).Returns(Result<Nothing>.Success());
        CompanyDto test_dto = new() { Id=1, Name="Test" };
        companyService.UpsertCompany(Arg.Any<CompanyDto>()).Returns(Result<CompanyDto>.Success(test_dto));

        var test_vm = new CompanyDetailViewModel(navigationService, dialogService, companyService)
        {
            Name = "Test",
            Description = "Test"
        };


        //Act
        await test_vm.AcseptCommand.ExecuteAsync(null);

        //Assert
        await navigationService.ReceivedWithAnyArgs().GoBackAndReturn(Arg.Any<Dictionary<string, object>>());
    }
    [Fact]
    public async Task CreateCompanyNavigationTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();

        var test_vm = new CompanyDetailViewModel(navigationService, dialogService, companyService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.CompanyDetail, 0 } });

        //Assert
        Assert.Equal("", test_vm.Name);
    }
    [Fact]
    public async Task EditCompanyNavigationTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var companyService = Substitute.For<ICompanyService>();
        CompanyDto test_dto = new() { Id = 1, Name = "Test" };
        companyService.GetCompanyAsync(default).ReturnsForAnyArgs(Result<CompanyDto>.Success(test_dto));

        var test_vm = new CompanyDetailViewModel(navigationService, dialogService, companyService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.CompanyDetail, 1 } });

        //Assert
        Assert.Equal("Test", test_vm.Name);
    }
}
