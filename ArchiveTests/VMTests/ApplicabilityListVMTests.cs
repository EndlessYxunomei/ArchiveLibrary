using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using ServiceLayer.Interfaces;
using VMLayer.Navigation;
using VMLayer;
using NSubstitute.ReceivedExtensions;

namespace ArchiveTests.VMTests;

public class ApplicabilityListVMTests
{
    [Fact]
    public async Task CreateCommandTest()
    {
        //Arrange
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        dialogService.Ask(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>()).Returns("test");

        List<ApplicabilityDto> test_list =
        [
            new() { Id = 1, Description = "test1"},
            new() { Id = 2, Description = "test2"}
        ];
        ApplicabilityDto test_dto = new() { Id = 3, Description = "test" };

        var applicabilityService = Substitute.For<IApplicabilityService>();
        applicabilityService.CheckApplicability(Arg.Any<string>()).ReturnsForAnyArgs(Result<Nothing>.Success());
        applicabilityService.GetApplicabilityListAsync().Returns(Result<List<ApplicabilityDto>>.Success(test_list));
        applicabilityService.UpsertApplicability(Arg.Any<ApplicabilityDto>()).ReturnsForAnyArgs(Result<ApplicabilityDto>.Success(test_dto));

        var test_vm = new ApplicabilityListViewModel(dialogService, applicabilityService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);

        //Act
        await test_vm.CreateCommand.ExecuteAsync(null);

        //Assert
        Assert.Equal(3, test_vm.ApplicabilityList.Count);
    }
    [Fact]
    public async Task EditCommandTest()
    {
        //Arrange
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        dialogService.Ask(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>()).Returns("test");

        List<ApplicabilityDto> test_list =
        [
            new() { Id = 1, Description = "test1"},
            new() { Id = 2, Description = "test2"}
        ];
        ApplicabilityDto test_dto = new() { Id = 2, Description = "test" };

        var applicabilityService = Substitute.For<IApplicabilityService>();
        applicabilityService.CheckApplicability(Arg.Any<string>()).ReturnsForAnyArgs(Result<Nothing>.Success());
        applicabilityService.GetApplicabilityListAsync().Returns(Result<List<ApplicabilityDto>>.Success(test_list));
        applicabilityService.UpsertApplicability(Arg.Any<ApplicabilityDto>()).ReturnsForAnyArgs(Result<ApplicabilityDto>.Success(test_dto));

        var test_vm = new ApplicabilityListViewModel(dialogService, applicabilityService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);
        test_vm.SelectedApplicability = test_vm.ApplicabilityList[1];

        //Act
        await test_vm.EditCommand.ExecuteAsync(null);

        //Assert
        Assert.Equal(2, test_vm.ApplicabilityList.Count);
    }
    [Fact]
    public async Task DeleteCommandTest()
    {
        //Arrange
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        List<ApplicabilityDto> test_list =
        [
            new() { Id = 1, Description = "test1"},
            new() { Id = 2, Description = "test2"}
        ];

        var applicabilityService = Substitute.For<IApplicabilityService>();
        applicabilityService.GetApplicabilityListAsync().Returns(Result<List<ApplicabilityDto>>.Success(test_list));
        applicabilityService.DeleteApplicability(default).ReturnsForAnyArgs(Result<Nothing>.Success());

        var test_vm = new ApplicabilityListViewModel(dialogService, applicabilityService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);
        test_vm.SelectedApplicability = test_vm.ApplicabilityList[1];

        //Act
        await test_vm.DeleteCommand.ExecuteAsync(null);

        //Assert
        await dialogService.Received().Notify("Удалено", "Применимость удалена");
    }
}
