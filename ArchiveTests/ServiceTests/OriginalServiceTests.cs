using NSubstitute;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using ServiceLayer;

namespace ArchiveTests.ServiceTests;

public class OriginalServiceTests
{
    [Fact]
    public async Task GetOriginalListTest()
    {
        //Arrange
        var originalRepo = Substitute.For<IOriginalRepo>();
        List<OriginalListDto> test_list =
            [
                new(){ Id = 1, OriginalInventoryNumber = 1, OriginalName = "test1", OriginalCaption = "caption1" },
                new(){ Id = 2, OriginalInventoryNumber = 3, OriginalName = "test2", OriginalCaption = "caption2" },
                new(){ Id = 3, OriginalInventoryNumber = 5, OriginalName = "test3", OriginalCaption = "caption3" }
            ];
        originalRepo.GetOriginalList().Returns(Result<List<OriginalListDto>>.Success(test_list));
        var originalService = new OriginalService(originalRepo);

        //Act
        var res = await originalService.GetOriginalListAsync();

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(3, res.Data[1].OriginalInventoryNumber);
    }
    [Fact]
    public async Task GetOriginalDetailCorrectly()
    {
        //Arrange
        var originalRepo = Substitute.For<IOriginalRepo>();
        List<OriginalDetailDto> test_list =
            [
                new() { Id = 1, InventoryNumber = 10, Name = "test_name1", Caption = "test_caption1" },
                new() { Id = 2, InventoryNumber = 20, Name = "test_name2", Caption = "test_caption2" },
                new() { Id = 3, InventoryNumber = 30, Name = "test_name3", Caption = "test_caption3" }
            ];
        originalRepo.GetOriginalDetailAsync(Arg.Any<int>()).Returns(y => Result<OriginalDetailDto>.Success(test_list.First(x => x.Id == (int)y[0])));
        var originalService = new OriginalService(originalRepo);

        //Act
        var res = await originalService.GetOriginalDetailAsync(1);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(10, res.Data.InventoryNumber);
    }
    [Fact]
    public async Task LastNumberTest()
    {
        //Arrange
        var originalRepo = Substitute.For<IOriginalRepo>();
        originalRepo.GetLastInventoryNumberAsync().Returns(Result<int>.Success(10));
        var originalService = new OriginalService(originalRepo);

        //Act
        var res = await originalService.GetLastInventoryNumber();

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(10, res.Data);
    }
    [Fact]
    public async Task FreeInventoryNumberTest()
    {
        //Arrange
        var originalRepo = Substitute.For<IOriginalRepo>();
        originalRepo.CheckInventoryNumberAsync(Arg.Any<int>()).Returns(Result<Nothing>.Success());
        var originalService = new OriginalService(originalRepo);

        //Act
        var res = await originalService.CheckInventoryNumber(10);

        //Assert
        Assert.True(res.IsSuccess);
    }
    [Fact]
    public async Task DeleteOriginalCorrectly()
    {
        //Arrange
        var originalRepo = Substitute.For<IOriginalRepo>();
        originalRepo.DeleteOriginal(Arg.Any<int>()).Returns(Result<Nothing>.Success());
        var originalService = new OriginalService(originalRepo);

        //Act
        var res = await originalService.DeleteOriginal(10);

        //Assert
        Assert.True(res.IsSuccess);
    }
    [Fact]
    public async Task UpsertOriginalCorrectly()
    {
        //Arrange
        var originalRepo = Substitute.For<IOriginalRepo>();
        List<OriginalListDto> test_list =
        [
            new() { Id = 1, OriginalInventoryNumber = 10, OriginalName = "test_name1"},
            new() { Id = 2, OriginalInventoryNumber = 20, OriginalName = "test_name2"},
            new() { Id = 3, OriginalInventoryNumber = 30, OriginalName = "test_name3"}
        ];
        originalRepo.UpsertOriginal(Arg.Any<OriginalDetailDto>()).Returns(Result<int>.Success(1));
        originalRepo.GetOriginalAsync(Arg.Any<int>()).Returns(x => Result<OriginalListDto>.Success(test_list.First(y => y.Id == (int)x[0])));

        var originalService = new OriginalService(originalRepo);

        //Act
        var res = await originalService.UpsertOriginal(new() { InventoryNumber = 0, Caption = "update_cap", Name = "update_name"});

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(10, res.Data.OriginalInventoryNumber);
    }
}
