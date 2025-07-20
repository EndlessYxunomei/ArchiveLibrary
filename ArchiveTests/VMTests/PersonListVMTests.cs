using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using ServiceLayer.Interfaces;
using VMLayer.Navigation;
using VMLayer;
using NSubstitute.ReceivedExtensions;

namespace ArchiveTests.VMTests;

public class PersonListVMTests
{
    [Fact]
    public async Task CreateCommandTest()
    {
        //Arrange
        var personService = Substitute.For<IPersonService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<PersonListDto> test_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_list));

        var test_vm = new PersonListViewModel(personService, dialogService, navigationService);

        //Act
        await test_vm.CreateCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToPersonDetails();

    }
    [Fact]
    public async Task EditCommandTest()
    {
        //Arrange
        var personService = Substitute.For<IPersonService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<PersonListDto> test_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_list));

        var test_vm = new PersonListViewModel(personService, dialogService, navigationService);
        test_vm.SelectedPerson = test_vm.PersonList[1];

        //Act
        await test_vm.EditCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToPersonDetails(test_vm.SelectedPerson.Id);
    }
    [Fact]
    public async Task DeleteCommandTest()
    {
        //Arrange
        var personService = Substitute.For<IPersonService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        List<PersonListDto> test_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_list));
        personService.DeletePerson(default).ReturnsForAnyArgs(Result<Nothing>.Success());

        var test_vm = new PersonListViewModel(personService, dialogService, navigationService);
        test_vm.SelectedPerson = test_vm.PersonList[1];

        //Act
        await test_vm.DeleteCommand.ExecuteAsync(null);

        //Assert
        await dialogService.Received().Notify("Удалено", "Пользователь удалён");
    }
    [Fact]
    public async Task NavigateParametrRecivedTest()
    {
        //Arrange
        var personService = Substitute.For<IPersonService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<PersonListDto> test_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personService.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_list));

        PersonListDto test_dto = new() { Id = 6, FullName = "test" };

        var test_vm = new PersonListViewModel(personService, dialogService, navigationService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.PersonList, test_dto } });

        //Assert
        Assert.Equal(3, test_vm.PersonList.Count);
    }
}
