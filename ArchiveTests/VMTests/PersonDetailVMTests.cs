using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using ServiceLayer.Interfaces;
using VMLayer.Navigation;
using VMLayer;
using NSubstitute.ReceivedExtensions;

namespace ArchiveTests.VMTests;

public class PersonDetailVMTests
{
    [Fact]
    public async Task CancelPersonTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var personService = Substitute.For<IPersonService>();

        var test_vm = new PersonDetailViewModel(navigationService, dialogService, personService);

        //Act
        await test_vm.CancelCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoBack();
    }
    [Fact]
    public async Task SavePersonTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var personService = Substitute.For<IPersonService>();
        PersonListDto test_dto = new() { Id = 1, FullName = "test" };
        personService.CheckPersonFullName(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(Result<Nothing>.Success());
        personService.UpsertPerson(Arg.Any<PersonDetailDto>()).ReturnsForAnyArgs(Result<PersonListDto>.Success(test_dto));

        var test_vm = new PersonDetailViewModel(navigationService, dialogService, personService)
        {
            LastName = "test"
        };

        //Act
        await test_vm.AcseptCommand.ExecuteAsync(null);

        //Assert
        await navigationService.ReceivedWithAnyArgs().GoBackAndReturn(Arg.Any<Dictionary<string, object>>());
    }

    [Fact]
    public async Task CreatePersonNavigationTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        var personService = Substitute.For<IPersonService>();

        var test_vm = new PersonDetailViewModel(navigationService, dialogService, personService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.PersonDetail, 0 } });

        //Assert
        Assert.Equal("", test_vm.LastName);
    }
    [Fact]
    public async Task EditPersonNavigationTest()
    {
        //Arrange
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);


        var personService = Substitute.For<IPersonService>();
        PersonDetailDto test_person = new()
        {
            LastName = "test_l",
            FirstName = "test_f",
            Department = "test_d",
            Id = 5
        };
        personService.GetPersonDetailAsync(5).Returns(Result<PersonDetailDto>.Success(test_person));

        var test_vm = new PersonDetailViewModel(navigationService, dialogService, personService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.PersonDetail, 5 } });

        //Assert
        Assert.Equal("test_l", test_vm.LastName);
    }
}
