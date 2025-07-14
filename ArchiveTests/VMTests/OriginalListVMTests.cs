using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using ServiceLayer.Interfaces;
using VMLayer.Navigation;
using VMLayer;
using NSubstitute.ReceivedExtensions;

namespace ArchiveTests.VMTests;

public class OriginalListVMTests
{
    [Fact]
    public async Task CreateCommandTest()
    {
        //Arrange
        var originalService = Substitute.For<IOriginalService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<OriginalListDto> test_list =
        [
            new(){ Id = 1, OriginalInventoryNumber = 1, OriginalName = "test1", OriginalCaption = "caption1" },
            new(){ Id = 2, OriginalInventoryNumber = 3, OriginalName = "test2", OriginalCaption = "caption2" },
            new(){ Id = 3, OriginalInventoryNumber = 5, OriginalName = "test3", OriginalCaption = "caption3" }
        ];
        originalService.GetOriginalListAsync().Returns( Result<List<OriginalListDto>>.Success(test_list));
        var test_vm = new OriginalListViewModel(navigationService, dialogService, originalService);

        //Act
        await test_vm.CreateCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToOriginalDetails();

    }
    [Fact]
    public async Task EditCommandTest()
    {
        //Arrange
        var originalService = Substitute.For<IOriginalService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<OriginalListDto> test_list =
        [
            new(){ Id = 1, OriginalInventoryNumber = 1, OriginalName = "test1", OriginalCaption = "caption1" },
            new(){ Id = 2, OriginalInventoryNumber = 3, OriginalName = "test2", OriginalCaption = "caption2" },
            new(){ Id = 3, OriginalInventoryNumber = 5, OriginalName = "test3", OriginalCaption = "caption3" }
        ];
        originalService.GetOriginalListAsync().Returns(Result<List<OriginalListDto>>.Success(test_list));
        var test_vm = new OriginalListViewModel(navigationService, dialogService, originalService);
        test_vm.SelectedOriginal = test_vm.OriginalsList[1];

        //Act
        await test_vm.EditCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToOriginalDetails(test_vm.SelectedOriginal.Id);
    }
    [Fact]
    public async Task DeleteCommandTest()
    {
        //Arrange
        var originalService = Substitute.For<IOriginalService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<OriginalListDto> test_list =
        [
            new(){ Id = 1, OriginalInventoryNumber = 1, OriginalName = "test1", OriginalCaption = "caption1" },
            new(){ Id = 2, OriginalInventoryNumber = 3, OriginalName = "test2", OriginalCaption = "caption2" },
            new(){ Id = 3, OriginalInventoryNumber = 5, OriginalName = "test3", OriginalCaption = "caption3" }
        ];
        originalService.GetOriginalListAsync().Returns(Result<List<OriginalListDto>>.Success(test_list));
        originalService.DeleteOriginal(Arg.Any<int>()).Returns(Result<Nothing>.Success());
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        var test_vm = new OriginalListViewModel(navigationService, dialogService, originalService);
        test_vm.SelectedOriginal = test_vm.OriginalsList[1];

        //Act
        await test_vm.DeleteCommand.ExecuteAsync(null);

        //Assert
        await dialogService.Received().Notify("Удалено", "Документ удалён");
        Assert.Equal(2, test_vm.OriginalsList.Count);
    }
    [Fact]
    public async Task NavigateParametrRecivedTest()
    {
        //Arrange
        var originalService = Substitute.For<IOriginalService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<OriginalListDto> test_list =
        [
            new(){ Id = 1, OriginalInventoryNumber = 1, OriginalName = "test1", OriginalCaption = "caption1" },
            new(){ Id = 2, OriginalInventoryNumber = 3, OriginalName = "test2", OriginalCaption = "caption2" },
            new(){ Id = 3, OriginalInventoryNumber = 5, OriginalName = "test3", OriginalCaption = "caption3" }
        ];
        OriginalListDto test_dto = new() { Id = 4, OriginalInventoryNumber = 7, OriginalName = "test4", OriginalCaption = "caption4" };
        originalService.GetOriginalListAsync().Returns(Result<List<OriginalListDto>>.Success(test_list));
        var test_vm = new OriginalListViewModel(navigationService, dialogService, originalService);

        //Act
        await test_vm.OnNavigatedTo(new() { { NavParamConstants.OriginalList, test_dto } });

        //Assert
        Assert.Equal(4, test_vm.OriginalsList.Count);
    }
}
