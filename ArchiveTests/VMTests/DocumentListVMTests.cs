using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using ServiceLayer.Interfaces;
using VMLayer.Navigation;
using VMLayer;
using NSubstitute.ReceivedExtensions;

namespace ArchiveTests.VMTests;

public class DocumentListVMTests
{
    [Fact]
    public async Task CreateCommandTest()
    {
        //Arrange
        var docunentService = Substitute.For<IDocumentService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<DocumentListDto> test_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 3, Name = "test3", DocumentType = ArchiveModels.DocumentType.CreateCopy },
            new() { Id = 4, Name = "test4", DocumentType = ArchiveModels.DocumentType.DeliverCopy },
            new() { Id = 5, Name = "test5", DocumentType = ArchiveModels.DocumentType.AddCorrection }
        ];
        docunentService.GetDocumentListAsync().Returns(Result<List<DocumentListDto>>.Success(test_list));

        var test_vm = new DocumentListViewModel(dialogService, navigationService, docunentService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);

        //Act
        await test_vm.CreateCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToDocumentDetails();

    }
    [Fact]
    public async Task EditCommandTest()
    {
        //Arrange
        var docunentService = Substitute.For<IDocumentService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<DocumentListDto> test_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 3, Name = "test3", DocumentType = ArchiveModels.DocumentType.CreateCopy },
            new() { Id = 4, Name = "test4", DocumentType = ArchiveModels.DocumentType.DeliverCopy },
            new() { Id = 5, Name = "test5", DocumentType = ArchiveModels.DocumentType.AddCorrection }
        ];
        docunentService.GetDocumentListAsync().Returns(Result<List<DocumentListDto>>.Success(test_list));

        var test_vm = new DocumentListViewModel(dialogService, navigationService, docunentService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);
        test_vm.SelectedDocument = test_vm.DocumentList[2];

        //Act
        await test_vm.EditCommand.ExecuteAsync(null);

        //Assert
        await navigationService.Received().GoToDocumentDetails(test_vm.SelectedDocument.Id);
    }
    [Fact]
    public async Task DeleteCommandTest()
    {
        //Arrange
        var docunentService = Substitute.For<IDocumentService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        dialogService.AskYesNo(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        List<DocumentListDto> test_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 3, Name = "test3", DocumentType = ArchiveModels.DocumentType.CreateCopy },
            new() { Id = 4, Name = "test4", DocumentType = ArchiveModels.DocumentType.DeliverCopy },
            new() { Id = 5, Name = "test5", DocumentType = ArchiveModels.DocumentType.AddCorrection }
        ];
        docunentService.GetDocumentListAsync().Returns(Result<List<DocumentListDto>>.Success(test_list));
        docunentService.DeleteDocument(default).ReturnsForAnyArgs(Result<Nothing>.Success());

        var test_vm = new DocumentListViewModel(dialogService, navigationService, docunentService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);
        test_vm.SelectedDocument = test_vm.DocumentList[2];

        //Act
        await test_vm.DeleteCommand.ExecuteAsync(null);

        //Assert
        await dialogService.Received().Notify("Удалено", "Документ удалён");
    }
    [Fact]
    public async Task NavigateParametrRecivedTest()
    {
        //Arrange
        var docunentService = Substitute.For<IDocumentService>();
        var navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        List<DocumentListDto> test_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 3, Name = "test3", DocumentType = ArchiveModels.DocumentType.CreateCopy },
            new() { Id = 4, Name = "test4", DocumentType = ArchiveModels.DocumentType.DeliverCopy },
            new() { Id = 5, Name = "test5", DocumentType = ArchiveModels.DocumentType.AddCorrection }
        ];
        docunentService.GetDocumentListAsync().Returns(Result<List<DocumentListDto>>.Success(test_list));
        DocumentListDto test_dto = new() { Id = 6, Name = "test6", DocumentType = ArchiveModels.DocumentType.AddCorrection };

        var test_vm = new DocumentListViewModel(dialogService, navigationService, docunentService);
        await test_vm.OnNavigatedTo(NavigationType.Unknown);

        //Act
        Dictionary<string, object> nav_dic = new() { { NavParamConstants.DocumentList, test_dto } };
        await test_vm.OnNavigatedTo(nav_dic);

        //Assert
        Assert.Equal(6, test_vm.DocumentList.Count);
    }
}
